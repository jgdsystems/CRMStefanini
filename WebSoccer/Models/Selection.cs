using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSoccer.Models
{
    public class Selection
    {
        [Key]
        public Int64 Id_selection { get; set; }

        public DateTime Date { get; set; }

        public int Active { get; set; }

        //Definição de chaves estrangeiras

        public Int64 Team_id_team { get; set; }
        [ForeignKey("Team_id_team")]
        public Team Team { get; set; }
                
        public Int64? Players_id_player { get; set; }
        [ForeignKey("Players_id_player")]
        public Players Players { get; set; }

        public int Users_id_users { get; set; }
        [ForeignKey("Users_id_users")]
        public Users Users { get; set; }

        public int Positions_id_positions { get; set; }
        [ForeignKey("Positions_id_positions")]
        public Positions Positions { get; set; }

        //Definição de listas de seleção
        [NotMapped]
        public List<Team> ListTeam { get; set; }
                
        [NotMapped]
        public List<Positions> ListPosition { get; set; }
        
        [NotMapped]
        public List<Players> ListPlayer { get; set; }

        


    }
}