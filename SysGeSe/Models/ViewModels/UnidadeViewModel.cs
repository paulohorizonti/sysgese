using System;
using System.ComponentModel.DataAnnotations;


namespace SysGeSe.Models.ViewModels
{
    public class UnidadeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da UNIDADE é obrigatório", AllowEmptyStrings = false)]
        public string Nome { get; set; }

        public string Cep { get; set; }

        public string Cidade { get; set; }

        public string Logradouro { get; set; }

        public string Estado { get; set; }

        public string Cnpj { get; set; }

        public string Ie { get; set; }

        public string Bairro { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Observacoes { get; set; }


        public bool Status { get; set; }

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
    }
}