﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SysGeSe.Models
{
    [Table("FUNCAO")]
    public class Funcao
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da FUNCAO é obrigatório", AllowEmptyStrings = false)]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }


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
        public bool Status { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Usuario> Usuario { get; set; }
    }
}