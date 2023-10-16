using PagedList;
using SysGeSe.Models;
using SysGeSe.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SysGeSe.Controllers.Util
{
    public class Util : Controller
    {
        
        public static void verificaResultado(string resultado)
        {
            switch (resultado)
            {
                case "0":
                    TempData["error"] = "Problemas ao concluir a operação, tente novamente!!";
                    break;
                case "1":
                    TempData["success"] = "Registro salvo com sucesso!!";
                    break;
                case "2":
                    TempData["info"] = "Registro Deletado com sucesso!!";
                    break;
                case "3":
                    TempData["warning"] = "Já existe um registro com essa descrição!!";
                    break;

            }
            TempData["resultado"] = null;
        }
    }
}