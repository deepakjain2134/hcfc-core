using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class PermitPermitFormSettingsMatrixViewComponent :ViewComponent
    {
        private readonly IPermitService _permitService;
        public PermitPermitFormSettingsMatrixViewComponent(IPermitService permitService)
        {
            _permitService = permitService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string mode, string id)
        {
            var data = new List<PermitSetting>();
            data = _permitService.GetPermitWorkFlowSettings();
            var assetlist=_permitService.GetAssetDevicePath();
            ViewBag.AssetDeviceChangePath = assetlist;
            if (data != null)
            {
                foreach (int val in Enum.GetValues(typeof(BDO.Enums.PermitType)))
                {
                    if (data != null && !data.Any(x => x.PermitType == val))
                    {
                        if(val!=13)
                        {
                            var PermitSetting = new PermitSetting
                            {
                                Required = true,
                                ControlType = 1,
                                LabelType = "Requester",
                                LabelText = "Requester",
                                PermitType = Convert.ToInt32(val),
                                Sequence = 1
                            };
                            data.Add(PermitSetting);
                        }
                        else
                        {
                            foreach(var assert in assetlist)
                            {
                                var PermitSetting = new PermitSetting
                                {
                                    Required = true,
                                    ControlType = 1,
                                    LabelType = "Requester",
                                    LabelText = "Requester",
                                    PermitType = Convert.ToInt32(val),
                                    Sequence = 1,
                                    PathId= assert.PathId,

                                };
                                data.Add(PermitSetting);
                            }
                        }
                     
                    }
                }
            }
            return await Task.FromResult(View(data));
        }
    }
}
