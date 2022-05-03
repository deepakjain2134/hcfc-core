using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace HCF.Localization
{
    public class Culture 
    {
        public Culture()
        {
          
        }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        public IList<Resource> Resources { get; set; }
    }
}