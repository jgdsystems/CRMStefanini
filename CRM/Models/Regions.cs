using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Regions
    {
        [Key]
        [Required]
        public int IdRegion { get; set; }

        public string Description { get; set; }

        public int Citys_IdCity { get; set; }

        [ForeignKey("Citys_IdCity")]
        public Citys City { get; set; }
    }
}