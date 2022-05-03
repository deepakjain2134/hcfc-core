using System.Collections.Generic;

namespace TMS
{
    public static class Buildings
    {
        public static List<HCF.BDO.Buildings> GetTmsBuildings()
        {
            var buildings = new List<HCF.BDO.Buildings>();
            var client = new HolyNameTmsService.HolyNameBuildingClient();
            var lclient = new HolyNameTmsService.HolyNameLocationsClient();
            var lists = client.GetCodesBuilding(11, true);
            foreach (HolyNameTmsService.Building list in lists)
            {
                var location = lclient.LoadLocations(11, true, list.siteCodeField, list.codeField);
                var data = new HCF.BDO.Buildings();
                var Floors = new List<HCF.BDO.Floor>();
                data.BuildingCode = list.codeField;
                data.BuildingName = list.descriptionField;
                data.SiteCode = list.siteCodeField;
                foreach (var item in location)
                {
                    var floor = new HCF.BDO.Floor
                    {
                        FloorCode = item.codeField,
                        FloorName = item.descriptionField,
                        Alias = item.codeField
                    };
                    Floors.Add(floor);
                }
                data.Floor = new List<HCF.BDO.Floor>();
                data.Floor = Floors;
                buildings.Add(data);
            }
            return buildings;
        }

        public static List<HCF.BDO.Buildings> GetBurkeTmsBuildings()
        {
            var buildings = new List<HCF.BDO.Buildings>();
            var client = new HolyNameTmsService.BurkeBuildingClient();
            var lclient = new HolyNameTmsService.BurkeLocationsClient();
            var lists = client.GetBurkeCodesBuilding(1, false);
            var Floors = new List<HCF.BDO.Floor>();
            foreach (var list in lists)
            {
                var location = lclient.LoadBurkeLocations(1, false, list.siteCodeField, list.codeField);
                var data = new HCF.BDO.Buildings
                {
                    BuildingCode = list.codeField,
                    BuildingName = list.descriptionField,
                    SiteCode = list.siteCodeField
                };
                foreach (var item in location)
                {
                    var floor = new HCF.BDO.Floor
                    {
                        FloorCode = item.codeField,
                        FloorName = item.descriptionField,
                        Alias = item.codeField
                    };
                    Floors.Add(floor);
                }
                data.Floor = new List<HCF.BDO.Floor>();
                data.Floor = Floors;
                buildings.Add(data);
            }
            return buildings;
        }

    }
}