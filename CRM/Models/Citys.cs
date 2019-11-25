using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Citys
    {
        [Key]
        [Required]
        public int IdCity { get; set; }

        public string Description { get; set; }

    }
}