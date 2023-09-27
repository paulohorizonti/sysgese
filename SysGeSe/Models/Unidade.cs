using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysGeSe.Models
{
    [Table("UNIDADE")]
    public class Unidade
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da UNIDADE é obrigatório", AllowEmptyStrings = false)]
        [Column("NOME")]
        public string Nome { get; set; }

       
        [Column("CEP")]
        public string Cep { get; set; }

        [Column("CIDADE")]
        public string Cidade { get; set; }


        [Column("ESTADO")]
        public string Estado { get; set; }


        [Column("LOGRADOURO")]
        public string Logradouro { get; set; }

        [Column("CNPJ")]
        public string Cnpj { get; set; }

        [Column("IE")]
        public string Ie { get; set; }

        [Column("BAIRRO")]
        public string Bairro { get; set; }

        [Column("TELEFONE")]
        public string Telefone { get; set; }

        [Column("CELULAR")]
        public string Celular { get; set; }

        [Column("OBSERVACOES")]
        public string Observacoes { get; set; }

        [Display(Name = "Ativo?")]
        [Column("STATUS")]
        public bool Status { get; set; }

        [Column("DATA_CAD")]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Cad { get; set; }

        //Retorna a data com formato Brasil
        [Display(Name = "Data do Cadastro")]
        public string DataCad
        {
            get { return Data_Cad?.ToShortDateString(); }
        }

        [Column("DATA_ALT")]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Alt { get; set; }

        [Display(Name = "Data da Alteração")]
        public string DataAlt
        {
            get { return Data_Alt?.ToShortDateString(); }
        }





    }
}