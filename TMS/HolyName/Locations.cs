using System.Collections.Generic;

namespace TMS
{
    public static class Locations
    {
        public static List<HCF.BDO.Floor> GetTmsLocations()
        {
            List<HCF.BDO.Floor> lists = new List<HCF.BDO.Floor>();
            HolyNameTmsService.HolyNameLocationsClient client = new HolyNameTmsService.HolyNameLocationsClient();
            HolyNameTmsService.Location[] locations = client.GetCodesLocation(11,true);
            foreach (HolyNameTmsService.Location list in locations)
            {
                HCF.BDO.Floor data = new HCF.BDO.Floor();
                data.FloorCode = list.codeField;
                data.FloorName = list.descriptionField;
              //  data.SiteCode = list.siteCodeField;
                lists.Add(data);
            }
            return lists;
        }
    }
}
