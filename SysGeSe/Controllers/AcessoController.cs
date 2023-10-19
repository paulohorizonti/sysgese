using PagedList;
using SysGeSe.Models;
using SysGeSe.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SysGeSe.Controllers
{
    public class AcessoController : Controller
    { 
         //Objego context
        readonly SysGeseDbContext db;

       
        //Lista geral dos registros
        List<Tabela> listTabelas = new List<Tabela>();
        List<Acesso> listaAcessos = new List<Acesso>();
        List<Perfil> listaPerfil = new List<Perfil>();
        IEnumerable<Tabela> tabEnu = new List<Tabela>();

        //Nome da tabela EM UMA CONSTANTE
        public const string NomeTabela = "ACESSO";

        //Construtor instanciando o contexto no obejto db
        public AcessoController()
        {
            db = new SysGeseDbContext();
        }

        // GET: Acesso
        public ActionResult Index(
           string param,
           string procuraPerfil,
           string procuraTabela,
           string filtroTabela,
           string filtroPerfil,
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

            procuraTabela = (procuraTabela == "") ? null : procuraTabela;
            //Procura por nome: se o filstro for diferente de zero applica esse filtro, caso contrario procura por nome mesmo

            procuraAtivo = (filtroAtivo != null) ? filtroAtivo : procuraAtivo; //procura por filtro
            procuraPerfil = (filtroPerfil != null) ? filtroPerfil : procuraPerfil; //procura por Perfil
            procuraTabela = (filtroTabela != null) ? filtroTabela : procuraTabela; //procura por Perfil

            

         

            //numero de linhas e status (ativo=1 ou inativo=0)
            ViewBag.NumeroLinhas = (numeroLinhas != null) ? numeroLinhas : 10;
            ViewBag.Status = (inputStatus != null) ? inputStatus : 2;

           

          

            //viewBag do filtro
            ViewBag.FiltroAtivo = procuraAtivo;
           
            if (procuraPerfil != null)
            {
                ViewBag.FiltroCorrentePerfil = (procuraPerfil);
            }

            if (procuraTabela != null)
            {
                ViewBag.FiltroCorrenteTabela = (procuraTabela);
            }




            //lista das tabelas instanciadas
            this.listaAcessos = db.Acessos.ToList(); //geral
          
            //buscar os ativos
            switch (ViewBag.Status)
            {
                case 0://somente os inativos
                    this.listaAcessos = this.listaAcessos.Where(s => s.Status == 0).ToList();
                    break;
                case 1: //somente ativos
                    this.listaAcessos = this.listaAcessos.Where(s => s.Status == 1).ToList();
                    break;
                case 2: //todos
                    this.listaAcessos = this.listaAcessos.Where(s => s.Status == 0 || s.Status == 1).ToList();
                    break;
            }

            //temp data da tabela - busca
            if (TempData["tabelaAtivo"] == null)
            {
                if (procuraTabela != null)
                {
                    TempData["tabelaAtivo"] = procuraTabela;
                    ViewBag.FiltroCorrenteTabelaInt = int.Parse(procuraTabela);
                    TempData.Keep("tabelaAtivo");

                }

            }
            else
            {
                if (procuraTabela != null)
                {
                    if (TempData["tabelaAtivo"].ToString() != procuraTabela)
                    {
                        TempData["tabelaAtivo"] = procuraTabela;
                        ViewBag.FiltroCorrenteTabelaInt = int.Parse(procuraTabela);
                        TempData.Keep("tabelaAtivo");
                    }
                    else
                    {
                        ViewBag.FiltroCorrenteTabelaInt = int.Parse(TempData["tabelaAtivo"].ToString());
                    }
                }
                else
                {
                    procuraTabela = TempData["tabelaAtivo"].ToString();
                    ViewBag.FiltroCorrenteTabelaInt = int.Parse(procuraTabela);

                }
            }
            //busca pelo perfil
            if (!String.IsNullOrEmpty(procuraTabela))
            {
                this.listaAcessos = this.listaAcessos.Where(s => s.IdTabela.ToString() == procuraTabela).ToList();
            }
           


            //temp data do perfil - busca
            if (TempData["perfilAtivo"] == null)
            {
                if (procuraPerfil != null)
                {
                    TempData["perfilAtivo"] = procuraPerfil;
                    ViewBag.FiltroCorrentePerfilInt = int.Parse(procuraPerfil);
                    TempData.Keep("perfilAtivo");

                }

            }
            else
            {
                if (procuraPerfil != null)
                {
                    if (TempData["perfilAtivo"].ToString() != procuraPerfil)
                    {
                        TempData["perfilAtivo"] = procuraPerfil;
                        ViewBag.FiltroCorrentePerfilInt = int.Parse(procuraPerfil);
                        TempData.Keep("perfilAtivo");
                    }
                    else
                    {
                        ViewBag.FiltroCorrentePerfilInt = int.Parse(TempData["perfilAtivo"].ToString());
                    }
                }
                else
                {
                    procuraPerfil = TempData["perfilAtivo"].ToString();
                    ViewBag.FiltroCorrentePerfilInt = int.Parse(procuraPerfil);

                }
            }
            //busca pelo perfil
            if (!String.IsNullOrEmpty(procuraPerfil)) 
            {
                this.listaAcessos = this.listaAcessos.Where(s => s.IdPerfil.ToString() == procuraPerfil).ToList();
            }
            else
            {
                this.listaAcessos = this.listaAcessos.Where(s => s.Perfis.Descricao.ToString() == "ADMINISTRATIVO").ToList();
            }


            if (TempData["perfilAtivo"] != null)
            {
                if (procuraPerfil != null)
                {
                    if (TempData["perfilAtivo"].ToString() == procuraPerfil)
                    {
                        //atribui 1 a pagina caso os parametros nao sejam nulos
                        page = (procuraAtivo != null) || (procuraTabela != null) ? 1 : page; //atribui 1 à pagina caso procurapor seja diferente de nullo

                    }
                    else
                    {
                        page = (procuraAtivo != null) || (procuraPerfil != null) || (procuraTabela != null) ? 1 : page; //atribui 1 à pagina caso procurapor seja diferente de nullo
                       
                    }
                }
                else
                {
                    procuraPerfil = TempData["perfilAtivo"].ToString();
                    ViewBag.FiltroCorrentePerfilInt = int.Parse(procuraPerfil);

                }


            }
          
            //montar a pagina
            int tamanhoPagina = 0;

            //Ternario para tamanho da pagina
            tamanhoPagina = (ViewBag.NumeroLinha != null) ? ViewBag.NumeroLinhas : (tamanhoPagina = (numeroLinhas != 10) ? ViewBag.numeroLinhas : (int)numeroLinhas);

            int numeroPagina = (page ?? 1);

            ViewBag.MensagemGravar = (resultado != null) ? resultado : "";

            ViewBag.Perfil = db.Perfis.AsNoTracking().OrderBy(s => s.Descricao).ToList();

           
            this.tabEnu = db.Tabelas.AsNoTracking().OrderBy(s => s.Nome).ToList();


            ViewBag.Tabela = this.tabEnu;

            return View(this.listaAcessos.ToPagedList(numeroPagina, tamanhoPagina));//retorna o pagedlist
        }


        //incluir novos acessos ao perfil: o usuario pode criar novos perfis desde que tenha acesso a isso
        public ActionResult Incluir()
        {
            //TO-DO
            //FILTRAR AS TABELAS POR PERFIL

            ViewBag.Perfil = db.Perfis.AsNoTracking().OrderBy(s => s.Descricao).ToList();
            ViewBag.Tabela = db.Tabelas.AsNoTracking().OrderBy(s => s.Nome).ToList();
            var model = new AcessoViewModel();

            return View(model);

           

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(AcessoViewModel model)
        {
            string resultado = "";

            model.Data_Cad = DateTime.Now;
            model.Data_Alt = DateTime.Now;
            model.Status = 1;

            model.Obs = (model.Obs == null) ? "" : model.Obs;

            if (ModelState.IsValid)
            {

                var acesso = from s in db.Acessos select s; //banco de acessos
                acesso = acesso.Where(s => s.IdTabela == model.IdTabela && s.IdPerfil == model.IdPerfil);//busca se tem tabela e o perfil para esse acesso
                if (acesso.Count() > 0)
                {
                    resultado = "4";
                    TempData["resultado"] = resultado;
                   
                    return RedirectToAction("Index", new { param = resultado});
                }

                //objeto para ser salvo
                Acesso access = new Acesso();

                access.IdTabela = model.IdTabela;
                access.Tabela_V = model.Tabela_V;
                access.Tabela_I = model.Tabela_I;
                access.Tabela_A = model.Tabela_A;
                access.Tabela_E = model.Tabela_E;
                access.Obs = model.Obs.ToUpper();
                access.Data_Cad = model.Data_Cad;
                access.Data_Alt = model.Data_Alt;
                access.Status = model.Status;
                access.IdPerfil = model.IdPerfil;
                try
                {
                    db.Acessos.Add(access);
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



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acesso acesso = db.Acessos.Find(id);
            if (acesso == null)
            {
                return HttpNotFound();
            }

            return View(acesso);


        }


        //EDIÇÃO
        public ActionResult Edit(int? id, bool? atv, bool? tab_v, bool? tab_i, bool? tab_a, bool? tab_e)
        {
            var acesso_var = db.Acessos.Find(id);

            if (atv != null)
            {
                acesso_var.Status = (acesso_var.Status == 1) ? acesso_var.Status = 0 : acesso_var.Status = 1;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

          
            if (tab_v != null)
            {
                acesso_var.Tabela_V = (acesso_var.Tabela_V == true) ? acesso_var.Tabela_V = false : acesso_var.Tabela_V = true;

               
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            if (tab_i != null)
            {
                acesso_var.Tabela_I = (acesso_var.Tabela_I == true) ? acesso_var.Tabela_I = false : acesso_var.Tabela_I = true;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            if (tab_a != null)
            {
                acesso_var.Tabela_A = (acesso_var.Tabela_A == true) ? acesso_var.Tabela_A = false : acesso_var.Tabela_A = true;

                db.SaveChanges();

                return RedirectToAction("Index");

            }

            if (tab_e != null)
            {
                acesso_var.Tabela_E = (acesso_var.Tabela_E == true) ? acesso_var.Tabela_E = false : acesso_var.Tabela_E = true;

                db.SaveChanges();

                return RedirectToAction("Index");

            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Acesso acesso = db.Acessos.Find(id);
            ViewBag.Perfil = db.Perfis.AsNoTracking().OrderBy(s => s.Descricao).ToList();
            ViewBag.Tabela = db.Tabelas.AsNoTracking().OrderBy(s => s.Nome).ToList();
            if (acesso == null)
            {
                return HttpNotFound();
            }
            return View(acesso);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,IdPerfil, IdTabela, Tabela_V, Tabela_I, Tabela_A, Tabela_E, Obs")] Acesso model)
        {
            string resultado;

            var acesso = db.Acessos.Find(model.Id);
            if (acesso == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            acesso.Data_Alt = DateTime.Now;
            acesso.Tabela_A = model.Tabela_A;
            acesso.Tabela_E = model.Tabela_E;
            acesso.Tabela_I = model.Tabela_I;
            acesso.Tabela_V = model.Tabela_V;
            acesso.Obs = model.Obs;
            try
            {
                db.SaveChanges();
                resultado = "1";
                TempData["resultado"] = resultado;
               
                return RedirectToAction("Index", new { param = resultado });
            }
            catch (Exception e)
            {
                resultado = "0";
                TempData["resultado"] = resultado;
               
                return RedirectToAction("Index", new { param = resultado });

            }




        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acesso acesso = db.Acessos.Find(id);
            if (acesso == null)
            {
                return HttpNotFound();
            }

            return View(acesso);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? Id)
        {
            string resultado;

            Acesso acesso = db.Acessos.Find(Id);

            db.Acessos.Remove(acesso);

            try
            {
                db.SaveChanges();
                resultado = "2"; //2 = deletado
                TempData["resultado"] = resultado;
                return RedirectToAction("Index", new { param = resultado });

            }
            catch (Exception e)
            {
                
                resultado = "0"; //não foi possivel
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
                case "4":
                    TempData["error"] = "ACESSO já cadastrado";
                    break;
            }
            TempData["resultado"] = null;
        }



        public ActionResult FiltrarPerfil(int id)
        {
            // ViewBag.PefilFiltrado = db.Perfis.AsNoTracking().OrderBy(s => s.Descricao).ToList();
           
            var consulta1 = from t in db.Tabelas
                           where !(from a in db.Acessos
                                   join tb in db.Tabelas on a.IdTabela equals tb.Id
                                   join  p in db.Perfis  on a.IdPerfil equals p.Id
                                   where a.IdPerfil == id select a.IdTabela).Contains(t.Id)
                           select t;

   


            //ViewBag.Filtrados1 = consulta1.ToList();

            //var consulta = from a in db.ViewAcessos where a.ID_PERFIL == id select a;

            //////var consultaTabela = from t in db.Tabelas select t.Id;


            ////ViewBag.ListaTabela = consultaTabela.ToList();

            //ViewBag.Lista1 = consulta.ToList();

            //ViewBag.Lista = consulta1.ToList();

            
            ViewBag.Filtrados = consulta1;

            //ViewBag.FiltradosLista = consulta.ToList();



            return View();

        }
    }
}