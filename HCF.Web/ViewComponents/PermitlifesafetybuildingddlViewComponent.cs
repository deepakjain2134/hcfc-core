using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class PermitlifesafetybuildingddlViewComponent :ViewComponent
    {
        private readonly IBuildingsService _buildingsService;
        public PermitlifesafetybuildingddlViewComponent(IBuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string type, string buildingId = null, string sequence = null)
        {
           var newist = _buildingsService.GetBuildings().ToList();
           var buildings = new List<Buildings>();

            if (!string.IsNullOrEmpty(buildingId))
            {

                string[] buildingval = buildingId.Split(',');
                foreach (var buildingitem in newist)
                {
                    if (buildingval.Length > 0)
                    {
                        foreach (string building in buildingval)
                        {
                            if (!string.IsNullOrEmpty(building))
                            {
                                if (buildingitem.BuildingId == Convert.ToInt32(building))
                                {
                                    buildings.Add(buildingitem);
                                }

                            }

                        }
                    }
                }
            }
            else
            {
                buildings = newist;
            }
            ViewBag.type = type;
            ViewBag.sequence = sequence != null ? sequence : string.Empty;
            return await Task.FromResult(View(buildings));
        }
    }
}
