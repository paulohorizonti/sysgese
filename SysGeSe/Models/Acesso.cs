using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysGeSe.Models
{
    [Table("TABELA")]
    public class Acesso
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione uma TABELA", AllowEmptyStrings = false)]
        [Column("TABELA_ID")]
        [ForeignKey("Tabela")]
        public int IdTabela { get; set; }

        [Required(ErrorMessage = "Informe se o acesso de VISUALIZAÇÃO é permitido", AllowEmptyStrings = false)]
        [Column("TABELA_V")]
        public sbyte Tabela_V { get; set; }

        [Required(ErrorMessage = "Informe se o acesso de INCLUSÃO é permitido", AllowEmptyStrings = false)]
        [Column("TABELA_I")]
        public sbyte Tabela_I { get; set; }

        [Required(ErrorMessage = "Informe se o acesso de ALTERAÇÃO é permitido", AllowEmptyStrings = false)]
        [Column("TABELA_A")]
        public sbyte Tabela_A { get; set; }

        [Required(ErrorMessage = "Informe se o acesso de EXCLUSÃO é permitido", AllowEmptyStrings = false)]
        [Column("TABELA_E")]
        public sbyte Tabela_E { get; set; }

              
       
        [Column("OBSERVACAO")]
        public string Obs { get; set; }

        [Column("DATA_CAD")]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Cad { get; set; }

        //Retorna a data com formato Brasil
        public string DataCad
        {
            get { return Data_Cad?.ToShortDateString(); }
        }

        [Column("DATA_ALT")]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        public DateTime? Data_Alt { get; set; }

        public string DataAlt
        {
            get { return Data_Alt?.ToShortDateString(); }
        }

        [Column("STATUS")]
        public sbyte Status { get; set; }


        [Required(ErrorMessage = "Selecione uma PERFIL", AllowEmptyStrings = false)]
        [Column("PERFIL_ID")]
        [ForeignKey("Perfil")]
        public int IdPerfil { get; set; }


        [JsonIgnore]
        public virtual Tabela Tabela { get; set; }

        [JsonIgnore]
        public virtual Perfil Perfil { get; set; }

    }
}