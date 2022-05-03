using HCF.BDO.NBIH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NbiBuildingsWS;

namespace TMS.NBIH
{
    public static class Buildings
    {
        public static List<TMSNBIBuildingsResults> GetTmsBuildings()
        {
            var authHeader = SetNbiHeader();
            var client = new BuildingWSSoapClient(BuildingWSSoapClient.EndpointConfiguration.BuildingWSSoap);
            var Buildinglist = client.GetCodesAsync(authHeader, 51, true).Result;
            List<HCF.BDO.NBIH.TMSNBIBuildingsResults> TMS_Results = ConverttoResult(Buildinglist);
            return TMS_Results;
        }

        private static List<TMSNBIBuildingsResults> ConverttoResult(Building[] buildinglist)
        {
            List<TMSNBIBuildingsResults> lists = new List<TMSNBIBuildingsResults>();
            foreach (var item in buildinglist)
            {
                HCF.BDO.NBIH.TMSNBIBuildingsResults obj = new TMSNBIBuildingsResults();
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.SiteCode = item.SiteCode;
                obj.SiteDescription = item.Description;
                obj.Description = item.Description;
                obj.SegmentID = item.SegmentID;
                obj.Show = item.Show;
                obj.ShowInQuery = item.ShowInQuery;
                obj.ModuleId = Convert.ToInt32(item.ModuleId);
                obj.ModuleCode = ((NbiStatusWS.MODULES)item.ModuleId).ToString();
                lists.Add(obj);
            }
            return lists;
        }

        private static AuthenticationHeader SetNbiHeader()
        {
            AuthenticationHeader authHeader = new AuthenticationHeader();
            authHeader = NbihAuthenticationHeader.GetHeader<AuthenticationHeader>(authHeader);
            return authHeader;
        }
    }
}
