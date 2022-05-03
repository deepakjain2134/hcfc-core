//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HCF.BDO;

//namespace HCF.Module.Core.Models
//{
//    public class UsedPassword:BaseEntity
//    {
//        public UsedPassword()
//        {
//            CreatedDate = DateTimeOffset.Now;
//        }

//        [Key, Column(Order = 0)]
//        public string HashPassword { get; set; }
//        public DateTimeOffset CreatedDate { get; set; }
//        [Key, Column(Order = 1)]
//        public int UserID { get; set; }
//        public virtual UserProfile AppUser { get; set; }
//    }
//}
