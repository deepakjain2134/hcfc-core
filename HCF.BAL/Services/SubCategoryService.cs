using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public class SubCategoryService : ISubCategoryService
    {
        public int Save(SubCategory subCategory)
        {
            return 1;
           // return HCF.DAL.SubCategoryRepository.Save(subCategory);
        }
    }
}
