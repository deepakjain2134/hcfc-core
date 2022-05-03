using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;

        }
        public int Save(Category category)
        {
            return _repository.Save(category);
        }
    }
}
