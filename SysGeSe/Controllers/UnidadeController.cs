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
    public class UnidadeController : Controller
    {
        //Objego context
        readonly SysGeseDbContext db;
        List<Unidade> listUnidades = new List<Unidade>();
       

        //Nome da tabela EM UMA CONSTANTE
        public const string NomeTabela = "UNIDADE";

        public UnidadeController()
        {
            db = new SysGeseDbContext();
        }


        // GET: Unidade
        
        public ActionResult Index(
            string param,
            string ordenacao,
            string procuraNome,
            string filtroNome,
            string procuraAtivo,
            string filtroAtivo,
            int? inputStatus,
            int? page,
            int? numeroLinhas)
        {
            string resultado = "";
           
            if(TempData["resultado"] ==null)
            {
                resultado = null;
            }
            else
            {               
                //Resultado do CREATE-EDIT-DELETE
                resultado = (param != null) ? param : "";
            }
         


            if (resultado == "0")
            {
                TempData["error"] = "Problemas ao concluir a operação, tente novamente!!";
                TempData["resultado"] = null;
            }
            if (resultado == "1")
            {
                TempData["success"] = "Registro salvo com sucesso!!";
                TempData["resultado"] = null;
            }
            if (resultado == "2")
            {
                TempData["info"] = "Registro Deletado com sucesso!!";
                TempData["resultado"] = null;
            }
            if (resultado == "3")
            {
                TempData["warning"] = "Já existe um registro com essa descrição!!";
                TempData["resultado"] = null;
            }

            //Procura por nome: se o filstro for diferente de zero applica esse filtro, caso contrario procura por nome mesmo
            procuraNome  = (filtroNome != null) ? filtroNome : procuraNome; //procura por nome
            procuraAtivo = (filtroAtivo != null) ? filtroAtivo : procuraAtivo; //procura por status

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
                ViewBag.FiltroCorrenteUnidade = (procuraNome);
            }


            //lista das tabelas instanciadas
            this.listUnidades = db.Unidades.ToList(); //geral

            //buscar os ativos
            switch (ViewBag.Status)
            {
                case 0://somente os inativos
                    this.listUnidades = this.listUnidades.Where(s => s.Status == false).ToList();
                    break;
                case 1: //somente ativos
                    this.listUnidades = this.listUnidades.Where(s => s.Status == true).ToList();
                    break;
                case 2: //todos
                    this.listUnidades = this.listUnidades.Where(s => s.Status == false || s.Status == true).ToList();
                    break;
            }

            //procura
            if (!String.IsNullOrEmpty(procuraNome))
            {
                this.listUnidades = listUnidades.Where(s => s.Nome.Contains(procuraNome)).ToList();
            }

            //montar a pagina
            int tamanhoPagina = 0;

            //Ternario para tamanho da pagina
            tamanhoPagina = (ViewBag.NumeroLinha != null) ? ViewBag.NumeroLinhas : (tamanhoPagina = (numeroLinhas != 10) ? ViewBag.numeroLinhas : (int)numeroLinhas);

            int numeroPagina = (page ?? 1);

      

            return View(this.listUnidades.ToPagedList(numeroPagina, tamanhoPagina));//retorna o pagedlist


        }


        public ActionResult Incluir()
        {

            var model = new UnidadeViewModel();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Incluir(UnidadeViewModel model)
        {
            string resultado = "";

            model.Data_Cad = DateTime.Now;
            model.Data_Alt = DateTime.Now;
            model.Status = true;


            if (ModelState.IsValid)
            {


                var unidade = from s in db.Unidades select s;


                string nome = model.Nome.Trim(); //tira espaços em branco
                unidade = unidade.Where(s => s.Nome.Contains(nome));

                if (unidade.Count() > 0)
                {
                    resultado = "3";
                    TempData["resultado"] = resultado;
                    return RedirectToAction("Index", new { param = resultado });

                }
                var unidade_nova = new Unidade();

                unidade_nova.Nome = model.Nome.Trim(); //passa para maiusculo
                unidade_nova.Logradouro = model.Logradouro;
                unidade_nova.Cep = model.Cep;
                unidade_nova.Cidade = model.Cidade;
                unidade_nova.Estado = model.Estado;
                unidade_nova.Cnpj = model.Cnpj;
                unidade_nova.Ie = model.Ie;
                unidade_nova.Bairro = model.Bairro;
                unidade_nova.Telefone = model.Telefone;
                unidade_nova.Celular = model.Celular;
                unidade_nova.Observacoes = model.Observacoes;


                unidade_nova.Status = model.Status;
                unidade_nova.Data_Cad = model.Data_Cad;
                unidade_nova.Data_Alt = model.Data_Alt;



                try
                {
                    db.Unidades.Add(unidade_nova);
                    db.SaveChanges();
                    resultado = "1";
                    TempData["resultado"] = resultado;
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

        public ActionResult Edit(int? id, bool? atv)
        {


            if (atv != null)
            {
                var uniade_var = db.Unidades.Find(id);

                if (uniade_var.Status == true)
                {
                    uniade_var.Status = false;
                }
                else
                {
                    uniade_var.Status = true;
                }

                db.SaveChanges();

                return RedirectToAction("Index");

            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = db.Unidades.Find(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }
            return View(unidade);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Nome, Cep, Cidade, Estado, Logradouro, Cnpj, Ie, Bairro, Telefone, Celular, Observacoes")] Unidade model)
        {
            string resultado;

            var unidade = db.Unidades.Find(model.Id);
            if (unidade == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unidade.Nome = model.Nome.ToUpper();
            unidade.Logradouro = model.Logradouro;
            unidade.Cep = model.Cep;
            unidade.Cidade = model.Cidade;
            unidade.Estado = model.Estado;
            unidade.Cnpj = model.Cnpj;
            unidade.Ie = model.Ie;
            unidade.Bairro = model.Bairro;
            unidade.Telefone = model.Telefone;
            unidade.Celular = model.Celular;
            unidade.Observacoes = model.Observacoes;
            unidade.Data_Alt = DateTime.Now;


            if (model.Observacoes!=null)
            {
                unidade.Observacoes = model.Observacoes.ToUpper();
            }
           
           

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

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unidade unidade = db.Unidades.Find(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }

            return View(unidade);


        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             Unidade unidade = db.Unidades.Find(id);
            if (unidade == null)
            {
                return HttpNotFound();
            }

            return View(unidade);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DeleteConfirmed(int? Id)
        {
            string resultado;
            Unidade unidade = db.Unidades.Find(Id);
            db.Unidades.Remove(unidade);

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
    }
}