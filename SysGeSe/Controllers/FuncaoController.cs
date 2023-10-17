using PagedList;
using SysGeSe.Models;
using SysGeSe.Models.ViewModels;
using SysGeSe.Models.ViewModels.SysGeSe.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SysGeSe.Controllers
{
    public class FuncaoController : Controller
    {
        //Objego context
        readonly SysGeseDbContext db;

        //Lista geral dos registros
        List<Funcao> listFuncoes = new List<Funcao>();
     

        //Nome da Perfil EM UMA CONSTANTE
        public const string NomePerfil = "FUNCAO";

        //Construtor instanciando o contexto no obejto db
        public FuncaoController()
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
            this.listFuncoes = db.Funcoes.ToList(); //geral

            //buscar os ativos
            switch (ViewBag.Status)
            {
                case 0://somente os inativos
                    this.listFuncoes = this.listFuncoes.Where(s => s.Status == false).ToList();
                    break;
                case 1: //somente ativos
                    this.listFuncoes = this.listFuncoes.Where(s => s.Status == true).ToList();
                    break;
                case 2: //todos
                    this.listFuncoes = this.listFuncoes.Where(s => s.Status == true || s.Status == false).ToList();
                    break;
            }

            //procura
            if (!String.IsNullOrEmpty(procuraNome))
            {
                this.listFuncoes = listFuncoes.Where(s => s.Descricao.Contains(procuraNome)).ToList();
            }

            //montar a pagina
            int tamanhoPagina = 0;

            //Ternario para tamanho da pagina
            tamanhoPagina = (ViewBag.NumeroLinha != null) ? ViewBag.NumeroLinhas : (tamanhoPagina = (numeroLinhas != 10) ? ViewBag.numeroLinhas : (int)numeroLinhas);

            int numeroPagina = (page ?? 1);


            return View(this.listFuncoes.ToPagedList(numeroPagina, tamanhoPagina));//retorna o pagedlist
        }



        public ActionResult Incluir()
        {

            var model = new PerfilViewModel();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(FuncaoViewModel model)
        {
            string resultado = "";

            model.Data_Cad = DateTime.Now;
            model.Data_Alt = DateTime.Now;
            model.Status = true;

            if (ModelState.IsValid)
            {
                var funcao = from s in db.Funcoes select s;

                string descricao = model.Descricao.Trim(); //tira espaços em branco


                funcao = funcao.Where(s => s.Descricao.Contains(descricao));

                if (funcao.Count() > 0)
                {
                    resultado = "3";
                    TempData["resultado"] = resultado;
                    return RedirectToAction("Index", new { param = resultado });

                }
                var funcao_nova = new Funcao();

                funcao_nova.Descricao = model.Descricao.ToUpper(); //passa para maiusculo

                funcao_nova.Status = model.Status;
                funcao_nova.Data_Cad = model.Data_Cad;
                funcao_nova.Data_Alt = model.Data_Alt;



                try
                {
                    db.Funcoes.Add(funcao_nova);
                    db.SaveChanges();
                    resultado = "1";
                    TempData["resultado"] = resultado;
                    return RedirectToAction("Index", new { param = resultado });
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                    resultado = "0";
                    TempData["resultado"] = resultado;
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
            Funcao funcao = db.Funcoes.Find(id);
            if (funcao == null)
            {
                return HttpNotFound();
            }

            return View(funcao);


        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcao funcao = db.Funcoes.Find(id);
            if (funcao == null)
            {
                return HttpNotFound();
            }

            return View(funcao);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? Id)
        {
            string resultado;
            Funcao funcao = db.Funcoes.Find(Id);
            db.Funcoes.Remove(funcao);

            try
            {
                db.SaveChanges();
                resultado = "2"; //2 = deletado
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });

            }
            catch (Exception e)
            {
                string ex = e.ToString(); //desaparecer o aviso de nao uso de variavel
                resultado = "0"; //não foi possivel
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });
            }


        }

        //EDIÇÃO
        public ActionResult Edit(int? id, bool? atv)
        {


            if (atv != null)
            {
                var funcao_var = db.Funcoes.Find(id);

                if (funcao_var.Status == true)
                {
                    funcao_var.Status = false;
                }
                else
                {
                    funcao_var.Status = true;
                }

                db.SaveChanges();

                return RedirectToAction("Index");

            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcao funcao = db.Funcoes.Find(id);
            if (funcao == null)
            {
                return HttpNotFound();
            }
            return View(funcao);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Descricao, Status ")] Funcao model)
        {
            string resultado;

            var funcao = db.Funcoes.Find(model.Id);
            if (funcao == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            funcao.Descricao = model.Descricao;
            funcao.Data_Alt = DateTime.Now;
            funcao.Status = model.Status;

            try
            {
                db.SaveChanges();
                resultado = "1";
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });
            }
            catch (Exception e)
            {
                string ex = e.ToString(); //desaparecer o aviso de nao uso de variavel

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