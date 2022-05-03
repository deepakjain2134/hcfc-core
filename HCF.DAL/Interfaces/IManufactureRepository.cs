using HCF.BDO;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface IManufactureRepository
    {
        List<Manufactures> GetManufactures();
        int Save(Manufactures newManufacture);
    }
}