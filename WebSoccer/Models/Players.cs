using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebSoccer.Validation;

namespace WebSoccer.Models
{
    public class Players
    {
        [Key]
        public Int64 Id_player { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(45)]
        public string Phonenumber { get; set; }

        [MaxLength(100)]
        [Email]
        public string Email { get; set; }

        [MaxLength(45)]
        public string Type { get; set; }

        [MaxLength(60)]
        public string Status { get; set; }

        //Definição de Chave estrangeira
        public int Users_id_users { get; set; }
        [ForeignKey("Users_id_users")]
        public Users User { get; set; }

        public int Id_Positions { get; set; }
        [ForeignKey("Id_Positions")]
        public Positions Positions { get; set; }
        
        public int Active { get; set; }

        [NotMapped]
        public List<Positions> ListPositions { get; set; }

        [NotMapped]
        public string Position { get; set; }

    }
}