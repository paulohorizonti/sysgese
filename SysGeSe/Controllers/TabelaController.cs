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
    public class TabelaController : Controller
    {
        //Objego context
        readonly SysGeseDbContext db;

        //Lista geral dos registros
        List<Tabela> listTabelas = new List<Tabela>();
        List<Acesso> listaAcessos = new List<Acesso>();

        //Nome da tabela EM UMA CONSTANTE
        public const string NomeTabela = "TABELA";

        //Construtor instanciando o contexto no obejto db
        public TabelaController()
        {
            db = new SysGeseDbContext();
        }
       

        /*** Index: Mostra todo o conteudo da tabela com parametros para o pagedList
         * @author: PAULO ROBERTO NOGUEIRA
         * param: Parametro que retorna uma mensagem das actions: create, edit, delete
         * ordenacao: Parametro para guardar a ordenação dos dados na view
         * qtdSalvos: Recebe a msg de quantidade de salvos
         * qtdNaoSalvos: Idem anterios mas com msg de nao salvos
         * procuraTabela: String para receber a procura pelo nome da tabela
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
                Fazer o mecaninsmo de verificação se o usuario tem acesso a tabela, buscando os acesso e guardando na requisição
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

            //AQUI ELE PEGA O ACESSO DE ACORDO COM A TABELA
            //ViewBag.AcessoPermitido = this.listaAcessos.Where(x => x.Tabela.Equals(this.Tabela)).ToList();

            //Resultado do CREATE-EDIT-DELETE
            string resultado = param;
            if(resultado == "0")
            {
                TempData["error"] = "Problemas ao concluir a operação, tente novamente!!";
            }
            if(resultado == "1")
            {
                TempData["success"] = "Registro salvo com sucesso!!";
            }
            if(resultado == "2")
            {
                TempData["info"] = "Registro Deletado com sucesso!!";
            }
            if(resultado == "3")
            {
                TempData["warning"] = "Já existe um registro com essa descrição!!";
            }
          

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
                ViewBag.FiltroCorrenteTabela = (procuraNome);
            }

            //lista das tabelas instanciadas
            this.listTabelas = db.Tabelas.ToList(); //geral

            //buscar os ativos
            switch (ViewBag.Status)
            {
                case 0://somente os inativos
                    this.listTabelas = this.listTabelas.Where(s => s.Status == 0).ToList();
                    break;
                case 1: //somente ativos
                    this.listTabelas = this.listTabelas.Where(s => s.Status == 1).ToList();
                    break;
                case 2: //todos
                    this.listTabelas = this.listTabelas.Where(s => s.Status == 0 || s.Status == 1).ToList();
                    break;
            }

            //procura
            if (!String.IsNullOrEmpty(procuraNome))
            {
                this.listTabelas = listTabelas.Where(s => s.Nome.Contains(procuraNome)).ToList();
            }

            //montar a pagina
            int tamanhoPagina = 0;

            //Ternario para tamanho da pagina
            tamanhoPagina = (ViewBag.NumeroLinha != null) ? ViewBag.NumeroLinhas : (tamanhoPagina = (numeroLinhas != 10) ? ViewBag.numeroLinhas : (int)numeroLinhas);

            int numeroPagina = (page ?? 1);

            ViewBag.MensagemGravar = (resultado != null) ? resultado : "";
          
           

            return View(this.listTabelas.ToPagedList(numeroPagina, tamanhoPagina));//retorna o pagedlist
        }



        public ActionResult Incluir()
        {
            
            var model = new TabelaViewModel();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(TabelaViewModel model)
        {
            string resultado = "";
           
            model.Data_Cad = DateTime.Now;
            model.Data_Alt = DateTime.Now;
            model.Status = 1;
          
            model.Obs = (model.Obs == null) ? "" : model.Obs;

           

            if (ModelState.IsValid)
            {
                var tabela = from s in db.Tabelas select s;

                string nome = model.Nome.Trim(); //tira espaços em branco
                nome = nome.ToUpper(); //passa para maiusculo

                tabela = tabela.Where(s => s.Nome.Contains(nome));

                if (tabela.Count() > 0)
                {
                    resultado = "3";
                    
                    return RedirectToAction("Index", new { param = resultado});

                }
                var tabela_nova = new Tabela();

                tabela_nova.Nome = model.Nome.ToUpper(); //passa para maiusculo
                tabela_nova.Obs = model.Obs.ToUpper();
                tabela_nova.Status = model.Status;
                tabela_nova.Data_Cad = model.Data_Cad;
                tabela_nova.Data_Alt = model.Data_Alt;

                

                try
                {
                    db.Tabelas.Add(tabela_nova);
                    db.SaveChanges();
                    resultado = "1";
                    return RedirectToAction("Index", new { param = resultado });
                }
                catch (Exception e)
                {
                    string ex = e.ToString();
                    
                    resultado = "0";
                }
            }
           

                return View(model);
            

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabela tabela = db.Tabelas.Find(id);
            if (tabela == null)
            {
                return HttpNotFound();
            }
         
            return View(tabela);
          

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabela tabela = db.Tabelas.Find(id);
            if (tabela == null)
            {
                return HttpNotFound();
            }

            return View(tabela);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? Id)
        {
            string resultado;
            Tabela tabela = db.Tabelas.Find(Id);
            db.Tabelas.Remove(tabela);

            try
            {
                db.SaveChanges();
                resultado = "2"; //2 = deletado
                return RedirectToAction("Index", new { param = resultado });

            }
            catch (Exception e)
            {
                resultado = "0"; //não foi possivel
                return RedirectToAction("Index", new { param = resultado });
            }


        }
        //EDIÇÃO
        public ActionResult Edit(int? id, bool? atv)
        {


            if (atv != null)
            {
                var tabela_var = db.Tabelas.Find(id);

                if (tabela_var.Status == 1)
                {
                    tabela_var.Status = 0;
                }
                else
                {
                    tabela_var.Status = 1;
                }

                db.SaveChanges();

                return RedirectToAction("Index");

            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tabela tabela = db.Tabelas.Find(id);
            if (tabela == null)
            {
                return HttpNotFound();
            }
            return View(tabela);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Nome, Obs")] Tabela model)
        {
            string resultado;
          
                var tabela = db.Tabelas.Find(model.Id);
                if (tabela == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tabela.Nome = model.Nome.ToUpper();
                tabela.Obs = model.Obs.ToUpper();
                tabela.Data_Alt = DateTime.Now;

            try
            {
                db.SaveChanges();
                resultado = "1";
                return RedirectToAction("Index", new { param = resultado });
            }
            catch (Exception e)
            {
                resultado = "0";
                return RedirectToAction("Index", new { param = resultado });

            }

           


        }
    }
}