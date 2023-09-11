using System;
using System.ComponentModel.DataAnnotations;

namespace SysGeSe.Models.ViewModels
{
    public class TabelaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da TABELA é obrigatório", AllowEmptyStrings = false)]
        public string Nome { get; set; }

      
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
    }
}