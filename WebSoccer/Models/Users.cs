using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebSoccer.Validation;

namespace WebSoccer.Models
{
    public class Users
    {
        //Referencia para validações http://www.macoratti.net/12/07/mvc_valid1.htm
        //http://regexlib.com/Search.aspx?k=password&c=-1&m=-1&ps=20

        [Key]
        [Required]
        public int Id_users { get; set;}

        [Required(ErrorMessage="O campo nome é obrigatório.")]
        [MaxLength(150)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O campo email é obrigatório.")]
        [MaxLength(100)]
        [Email]
        public string Email { get; set;}

        [Required(ErrorMessage="O campo senha é obrigatório.")]
        [MaxLength(100)]
        public string Password { get; set;}

        [MaxLength(45)]
        public string Type { get; set; }

        public DateTime Create_date { get; set; }

        public DateTime Last_access { get; set; }

        public int Active { get; set; }

        [MaxLength(60)]
        public string Profile { get; set; }
        
        public string PasswordRecover { get; set; }

        [NotMapped]
        public string Captcha { get; set; }

    }
}