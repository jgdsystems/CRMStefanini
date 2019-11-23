using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSoccer.Models
{
    public class Match
    {
        [Key]
        public int Id_match { get; set; }
                
        public Int64 Team_id_team_A { get; set; }
        
        public Int64 Team_id_team_B { get; set; }

        public int Score_a { get; set; }

        public int Score_b { get; set; }

        public Int64 Winner { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public DateTime Confirmation { get; set; }

        public string Status { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        //Definição de Chave estrangeira
        public int Users_id_users { get; set; }
        
        [ForeignKey("Users_id_users")]
        public Users User { get; set; }
                
        [ForeignKey("Team_id_team_A")]
        public Team TeamA { get; set; }
        
        [ForeignKey("Team_id_team_B")]
        public Team TeamB { get; set; }
        
    }
}