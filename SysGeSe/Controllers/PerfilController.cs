using PagedList;
using SysGeSe.Models;
using SysGeSe.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SysGeSe.Controllers
{
    public class PerfilController : Controller
    {
        //Objego context
        readonly SysGeseDbContext db;

        //Lista geral dos registros
        List<Perfil> listPerfis = new List<Perfil>();
        List<Acesso> listaAcessos = new List<Acesso>();

      
        //Nome da Perfil EM UMA CONSTANTE
        public const string NomePerfil = "PERFIL";

        //Construtor instanciando o contexto no obejto db
        public PerfilController()
        {
            db = new SysGeseDbContext();
        }


        /*** Index: Mostra todo o conteudo da Perfil com parametros para o pagedList
         * @author: PAULO ROBERTO NOGUEIRA
         * param: Parametro que retorna uma mensagem das actions: create, edit, delete
         * ordenacao: Parametro para guardar a ordenação dos dados na view
         * qtdSalvos: Recebe a msg de quantidade de salvos
         * qtdNaoSalvos: Idem anterios mas com msg de nao salvos
         * procuraPerfil: String para receber a procura pelo nome da Perfil
         * filtroNome: qual o filtro que esta ativo  nomemento
         * inputStatus: recebe o status caso o usuario muda na view
         * filtroCorrente: recebe qual filtro esta ativo no momento
         * produraAtivo: procura pelos ativos
         * filtroAtivo: define o filtro dos ativos
         * page: pagina que esta sendo exibida
         * numeroLinhas: quantas linhas devem ser exibidas na view sss
         */
        public ActionResult Index(
            string param,
            string ordenacao,
            string qtdSalvos,
            string qtdNaoSalvos,
            string procuraNome,
            string filtroNome,
            string procuraAtivo,
            string filtroAtivo,
            int? inputStatus,
            int? page,
            int? numeroLinhas)
        {

            /*
                TO-DO
                Fazer o mecaninsmo de verificação se o usuario tem acesso a Perfil, buscando os acesso e guardando na requisição
             */
            //if (Session["usuario"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            //else
            //{
            //    ViewBag.PerfilUsuario = Session["perfilId"]; //PEGA O ID ODO PERFIL DO USUARIO
            //}
            //Pega os acessos
            //this.listaAcessos = db.Acessos.ToList();
            //this.listaAcessos = this.acess.Where(x => x.IdPerfil == ViewBag.PerfilUsuario).ToList(); //aqui pega os acessos BASEADO NO ID

            //AQUI ELE PEGA O ACESSO DE ACORDO COM A Perfil
            //ViewBag.AcessoPermitido = this.listaAcessos.Where(x => x.Perfil.Equals(this.Perfil)).ToList();

            //Resultado do CREATE-EDIT-DELETE
            //Resultado do CREATE-EDIT-DELETE
            string resultado = "";

            if (TempData["resultado"] == null)
            {
                resultado = null;
            }
            else
            {
                //Resultado do CREATE-EDIT-DELETE
                resultado = (param != null) ? param : "";
            }

            //Verifica o resultado da solicitacao CRUD
          
            VerificaResultado(resultado);


            //Procura por nome: se o filstro for diferente de zero applica esse filtro, caso contrario procura por nome mesmo
            procuraNome = (filtroNome != null) ? filtroNome : procuraNome; //procura por nome
            procuraAtivo = (filtroAtivo != null) ? filtroAtivo : procuraAtivo; //procura por nome

            ViewBag.MensagemGravar = (param != null) ? param : "";

            //numero de linhas e status (ativo=1 ou inativo=0)
            ViewBag.NumeroLinhas = (numeroLinhas != null) ? numeroLinhas : 10;
            ViewBag.Status = (inputStatus != null) ? inputStatus : 2;

            //Ordenação
            ViewBag.Ordenacao = ordenacao; //viewBag da ordenação

            //Se nao vier nula a ordenacao aplicar por nome decrescente
            ViewBag.ParametroNome = String.IsNullOrEmpty(ordenacao) ? "Nome_desc" : "";

            //atribui 1 a pagina caso os parametros nao sejam nulos
            page = (procuraAtivo != null) || (procuraNome != null) ? 1 : page; //atribui 1 à pagina caso procurapor seja diferente de nullo

            //viewBag do filtro
            ViewBag.FiltroAtivo = procuraAtivo;
            if (procuraNome != null)
            {
                ViewBag.FiltroCorrentePerfil = (procuraNome);
            }

            //lista das Perfils instanciadas
            this.listPerfis = db.Perfis.ToList(); //geral

            //buscar os ativos
            switch (ViewBag.Status)
            {
                case 0://somente os inativos
                    this.listPerfis = this.listPerfis.Where(s => s.Status == false).ToList();
                    break;
                case 1: //somente ativos
                    this.listPerfis = this.listPerfis.Where(s => s.Status == true).ToList();
                    break;
                case 2: //todos
                    this.listPerfis = this.listPerfis.Where(s => s.Status == true || s.Status == false).ToList();
                    break;
            }

            //procura
            if (!String.IsNullOrEmpty(procuraNome))
            {
                this.listPerfis = listPerfis.Where(s => s.Descricao.Contains(procuraNome)).ToList();
            }

            //montar a pagina
            int tamanhoPagina = 0;

            //Ternario para tamanho da pagina
            tamanhoPagina = (ViewBag.NumeroLinha != null) ? ViewBag.NumeroLinhas : (tamanhoPagina = (numeroLinhas != 10) ? ViewBag.numeroLinhas : (int)numeroLinhas);

            int numeroPagina = (page ?? 1);

            ViewBag.MensagemGravar = (resultado != null) ? resultado : "";



            return View(this.listPerfis.ToPagedList(numeroPagina, tamanhoPagina));//retorna o pagedlist
        }



        public ActionResult Incluir()
        {

            var model = new PerfilViewModel();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(PerfilViewModel model)
        {
            string resultado = "";

            model.Data_Cad = DateTime.Now;
            model.Data_Alt = DateTime.Now;
            model.Status = true;
                    



            if (ModelState.IsValid)
            {
                var perfil = from s in db.Perfis select s;

                string descricao = model.Descricao.Trim(); //tira espaços em branco
                descricao = descricao.ToUpper(); //passa para maiusculo

                perfil = perfil.Where(s => s.Descricao.Contains(descricao));

                if (perfil.Count() > 0)
                {
                    resultado = "3";

                    return RedirectToAction("Index", new { param = resultado });

                }
                var perfil_novo = new Perfil();

                perfil_novo.Descricao = model.Descricao.ToUpper(); //passa para maiusculo

                perfil_novo.Status = model.Status;
                perfil_novo.Data_Cad = model.Data_Cad;
                perfil_novo.Data_Alt = model.Data_Alt;



                try
                {
                    db.Perfis.Add(perfil_novo);
                    db.SaveChanges();
                    resultado = "1";
                    TempData["resultado"] = resultado;
                    return RedirectToAction("Index", new { param = resultado });
                }
                catch
                {
                    

                    resultado = "0";
                    TempData["resultado"] = resultado;
                    return RedirectToAction("Index", new { param = resultado });
                }
            }


            return View(model);



        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }

            return View(perfil);


        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }

            return View(perfil);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? Id)
        {
            string resultado;
            Perfil perfil = db.Perfis.Find(Id);
            db.Perfis.Remove(perfil);

            try
            {
                db.SaveChanges();
                resultado = "2"; //2 = deletado
                return RedirectToAction("Index", new { param = resultado });

            }
            catch (Exception e)
            {
                string erro = e.Message;
                resultado = "0"; //não foi possivel
                return RedirectToAction("Index", new { param = resultado });
            }


        }

        //EDIÇÃO
        public ActionResult Edit(int? id, bool? atv)
        {


            if (atv != null)
            {
                var perfil_var = db.Perfis.Find(id);

                if (perfil_var.Status == true)
                {
                    perfil_var.Status = false;
                }
                else
                {
                    perfil_var.Status = true;
                }

                db.SaveChanges();

                return RedirectToAction("Index");

            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perfil perfil = db.Perfis.Find(id);
            if (perfil == null)
            {
                return HttpNotFound();
            }
            return View(perfil);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Descricao")] Perfil model)
        {
            string resultado;

            var perfil = db.Perfis.Find(model.Id);
            if (perfil == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            perfil.Descricao = model.Descricao.ToUpper();
            perfil.Data_Alt = DateTime.Now;

            try
            {
                db.SaveChanges();
                resultado = "1";
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });
            }
            catch
            {
                resultado = "0";
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });

            }
        }

        //Receber variavel com valor de qualquer action
        private void VerificaResultado(string resultado)
        {
            //Verifica cada estado do resultado na estrutura switch-case e atribui a mensagem ao TempData correspondente
            switch (resultado)
            {
                case "0":
                    TempData["error"] = "Problemas ao concluir a operação, tente novamente!!";
                    break;
                case "1":
                    TempData["success"] = "Registro salvo com sucesso!!";
                    break;
                case "2":
                    TempData["success"] = "Registro salvo com sucesso!!";
                    break;
                case "3":
                    TempData["warning"] = "Já existe um registro com essa descrição!!";
                    break;

            }
            TempData["resultado"] = null;
        }
    }
}