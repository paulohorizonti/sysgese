using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SysGeSe.Controllers
{
    public class UtilController : Controller
    {
        // GET: Util
        public ActionResult Index()
        {
            return View();
        }

        public  void VerificaResultado(string resultado)
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