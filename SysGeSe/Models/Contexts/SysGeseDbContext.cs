using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SysGeSe.Models
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class SysGeseDbContext : DbContext
    {
        //public DbSet<Acesso> Acessos { get; set; }
        //public DbSet<Perfil> Perfis { get; set; }

        public DbSet<Tabela> Tabelas { get; set; }


    }
}