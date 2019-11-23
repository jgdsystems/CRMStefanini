using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSoccer.Models
{
    public class Team
    {
        [Key]
        public Int64 Id_team { get; set; }

        public string Name { get; set; }

        //Chave estrangeira
        public int Users_id_users { get; set; }

        [ForeignKey("Users_id_users")]
        public Users User { get; set; }


    }
}