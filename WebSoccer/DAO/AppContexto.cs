using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebSoccer.Models;

namespace WebSoccer.DAO
{
    public class AppContexto : DbContext
    {
        public virtual DbSet<Users> User { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<Selection> Selection { get; set; }

        //Atributo da conexão do arquivo webConfig
        public AppContexto() : base("MyContext")
        {

        }

        /* O Entity Framework tem uma coisinha meio chata; Como nós criamos um DbSet com o nome de Filme o Entity na verdade vai procurar no banco de dados uma tabela com o nome de Filmees; Isso porque ele vai pluralizar colocando o 'es' na frente.
        Para bloquear isso, inclua o codigo abaixo ainda dentro da classe BancoContexto:*/
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}