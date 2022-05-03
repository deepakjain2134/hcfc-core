using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ICategoryRepository
    {
        List<Category> GetCategory();
        int Save(Category category);
    }
}