using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Module.Survey.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "code is required")]      
        public string code { get; set; }
    }
}
