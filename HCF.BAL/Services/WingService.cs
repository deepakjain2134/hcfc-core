using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL
{
    public  class WingService :IWingService
    {
        private readonly IWingRepository _wingRepository;

        public WingService(IWingRepository wingRepository)
        {
            _wingRepository = wingRepository;

        }
        public  int AddWing(Wing newWings)
        {
            return _wingRepository.Save(newWings);
        }

        public  List<Wing> GetWings()
        {
            return _wingRepository.GetWings();
        }

        public  List<Wing> GetBuildingWings(int buildingId)
        {
            return _wingRepository.GetWings().Where(x => x.BuildingId == buildingId).ToList();
        }

        public  Wing GetWing(int wingId)
        {
            return _wingRepository.GetWings().FirstOrDefault(x => x.WingId == wingId);
        }

        public  List<Wing> GetFloorWing(int floorId)
        {
            return _wingRepository.GetWings(floorId);
        }

        public  int  UpdateWing(Wing updateWing)
        {
            return _wingRepository.Save(updateWing);
        }       
    }
}
