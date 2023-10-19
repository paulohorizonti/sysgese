using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysGeSe.Models
{
    [Table("TODOS_ACESSOS")]
    public class ViewAcesso
    {
       
        [Column("REGISTRO")]
        public int ID { get; set; }

        
        
      
        [Column("ID_TABELA")]
        public int ID_TABELA { get; set; }


        [Column("DESC_TABELA")]
        public string DESC_TABELA { get; set; }
                      
        [Column("VISUALIZAR")]
        public bool VISUALIZAR { get; set; }

       
        [Column("INCLUIR")]
        public bool INCLUIR { get; set; }

        [Column("DELETAR")]
        public bool DELETAR { get; set; }

      
        [Column("EDITAR")]
        public bool EDITAR { get; set; }

              
       
        [Column("OBSERVACAO")]
        public string OBSEVACAO { get; set; }

        [Column("ID_PERFIL")]
        public int ID_PERFIL { get; set; }


        [Column("DESC_PERFIL")]
        public string DESC_PERFIL { get; set; }


       

    }
}