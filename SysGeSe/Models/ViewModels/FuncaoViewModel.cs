using System;
using System.ComponentModel.DataAnnotations;
namespace SysGeSe.Models.ViewModels
{

    namespace SysGeSe.Models.ViewModels
    {
        public class FuncaoViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "O nome da FUNÇÃO  é obrigatório", AllowEmptyStrings = false)]
            public string Descricao { get; set; }



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


            public bool Status { get; set; }

        }
    }
}