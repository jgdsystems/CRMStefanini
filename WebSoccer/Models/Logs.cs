using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebSoccer.Models
{
    public class Logs
    {
        [Key]
        [Required]
        public int Id_log { get; set; }

        public DateTime Datetime { get; set; }

        public string Description { get; set; }

        public int Id_user { get; set; }

        //1=Event and 2=Error
        public int Type { get; set; }


    }
}