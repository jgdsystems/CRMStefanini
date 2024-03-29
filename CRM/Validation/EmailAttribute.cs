﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRM.Validation
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute() : base(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$")
        {
            ErrorMessage = "Formato de email incorreto.";
        }
    }
    
}