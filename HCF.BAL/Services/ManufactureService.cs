using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public class ManufactureService : IManufactureService
    {
        private readonly IManufactureRepository _manufactureRepository;
        public ManufactureService(IManufactureRepository manufactureRepository)
        {
            _manufactureRepository = manufactureRepository;

        }
        public int Save(Manufactures newManufactures) => _manufactureRepository.Save(newManufactures);

        public List<Manufactures> GetAll() => _manufactureRepository.GetManufactures();

        public Manufactures Get(int makeId) => _manufactureRepository.GetManufactures().FirstOrDefault(x => x.ManufactureId == makeId);
    }
}
