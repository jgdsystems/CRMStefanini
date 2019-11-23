using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSoccer.Models
{
    public class Positions
    {
        [Key]
        public int Id_Positions { get; set; }

        public string Name { get; set; }
        
        public int Active { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        //Definição de Chave estrangeira
        public int Id_Users { get; set; }

        [ForeignKey("Id_Users")]
        public Users Users { get; set; }

    }
}