using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.Infrastructure.Models;

namespace HCF.BDO.Users
{
    public class UsedPassword : EntityBase
    {
        public UsedPassword()
        {
            CreatedDate = DateTime.Now;
        }
        public string HashPassword { get; set; }
        public DateTime CreatedDate { get; set; }      
        public long UserID { get; set; }       
    }
}
