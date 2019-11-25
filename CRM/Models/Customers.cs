using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Customers
    {
        [Key]
        public Int64 Id_customer { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [MaxLength(150)]
        public string Name { get; set; }

        //Chave estrangeira
        public int Users_id_users { get; set; }

        [ForeignKey("Users_id_users")]
        public Users User { get; set; }

        [MaxLength(1)]
        public string Classification { get; set; }

        [MaxLength(40)]
        public string Phone { get; set; }

        [MaxLength(1)]
        public string Sex { get; set; }

        [MaxLength(150)]
        public string City { get; set; }

        [MaxLength(150)]
        public string Region { get; set; }

        [Display(Name = "Ultima compra")]
        public DateTime LastPurchase { get; set; }

        [Display(Name = "Até")]
        public DateTime Until { get; set; }
    }
}