using System;
using System.ComponentModel.DataAnnotations;

namespace SysGeSe.Models.ViewModels
{
    public class AcessoViewModel
    {
       
        public int Id { get; set; }

        public int IdTabela { get; set; }

     
        public bool Tabela_V { get; set; }

      
        public bool Tabela_I { get; set; }

       
        public bool Tabela_A { get; set; }

       
        public bool Tabela_E { get; set; }



        public string Obs { get; set; }

       
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Cad { get; set; }

        //Retorna a data com formato Brasil
        public string DataCad
        {
            get { return Data_Cad?.ToShortDateString(); }
        }

        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Alt { get; set; }

        public string DataAlt
        {
            get { return Data_Alt?.ToShortDateString(); }
        }

       
        public sbyte Status { get; set; }


      
        public int IdPerfil { get; set; }
    }
}