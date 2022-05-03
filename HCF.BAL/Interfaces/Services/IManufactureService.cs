using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface IManufactureService
    {
        Manufactures Get(int makeId);
        List<Manufactures> GetAll();
        int Save(Manufactures newManufactures);
    }
}