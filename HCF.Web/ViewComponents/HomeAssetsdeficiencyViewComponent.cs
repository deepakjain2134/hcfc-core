using HCF.BAL;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class HomeAssetsdeficiencyViewComponent : ViewComponent
    {
        private readonly IInsActivityService _inspectionActivityService;
        private readonly ITypeService _typeService;
        private readonly IHCFSession _session;
        public HomeAssetsdeficiencyViewComponent(IHCFSession session, IInsActivityService inspectionActivityService, ITypeService typeService)
        {
            _inspectionActivityService = inspectionActivityService;
            _typeService = typeService;
            _session = session;

        }
        public async Task<IViewComponentResult> InvokeAsync(string Taggedid, string orgId)
        {
            var taggedMaster = new List<TaggedMaster>();
            List<TInspectionActivity> inspectionActivity = new List<TInspectionActivity>();
            var userid = UserSession.CurrentUser.UserId;
            if (Taggedid != "0" && Taggedid != null)
            {
                ViewBag.TaggedID = Taggedid;
                taggedMaster = _typeService.GetTaggedList(userid, Guid.Parse(Taggedid), Convert.ToInt32(BDO.Enums.TaggedType.Deficiency));
            }
            else { taggedMaster = _typeService.GetTaggedList(userid, null, Convert.ToInt32(BDO.Enums.TaggedType.Deficiency)); }

            //Session["IsExistTagDeficiency"] = false;
            _session.Set(SessionKey.IsExistTagDeficiency, false);

            var actIdFrmDshb = _session.Get<string>(SessionKey.ActIdFrmDshb);
            ViewBag.actIdFrmDshb = actIdFrmDshb;
            ViewBag.IsGuestUser = _session.Get<string>(SessionKey.IsGuestUser);
            var activityIdFrmdshbrd = (string.IsNullOrEmpty(actIdFrmDshb)) ? null : actIdFrmDshb;
            if (!string.IsNullOrEmpty(activityIdFrmdshbrd))
            {
                inspectionActivity = _inspectionActivityService.GetDeficiencyAssets(0, activityIdFrmdshbrd, null);
            }
            else
            {
                if (!string.IsNullOrEmpty(Taggedid) && Taggedid != "0")
                {
                    taggedMaster = taggedMaster.Where(x => x.TaggedId == Guid.Parse(Taggedid)).ToList();
                    var result = taggedMaster.FirstOrDefault();
                    if (result == null)
                    {
                        ViewBag.IsExistTagDeficiency = "TagNOTExists";
                    }
                    else
                    {
                        if (result.TaggedId.ToString() == Taggedid.ToLower())
                        {
                            double days = (DateTime.Now - result.CreatedDate).TotalDays;
                            if (days > 14)
                            {
                                ViewBag.IsExistTagDeficiency = "TagExpired";
                            }
                            else
                            {
                                ViewBag.IsExistTagDeficiency = "true";
                                // Session["IsExistTagDeficiency"] = true;
                                _session.Set(SessionKey.IsExistTagDeficiency, true);
                            }
                        }
                    }

                    var actIdFrmDshbSession = _session.Get<string>(SessionKey.ActIdFrmDshb);
                    var activityid = (!string.IsNullOrEmpty(actIdFrmDshbSession)) ? actIdFrmDshbSession : null;

                    if (taggedMaster.Count > 0)
                    {

                        List<string> activityIds = new List<string>();
                        foreach (var id in taggedMaster)
                        {
                            if (!string.IsNullOrEmpty(id.ActivityId.ToString()))
                                activityIds.Add(id.ActivityId.ToString());
                        }
                        foreach (var id in activityIds)
                        {
                            var item = _inspectionActivityService.GetDeficiencyAssets(0, id, null);
                            if (item.Count > 0)
                            {
                                inspectionActivity.Add(item.FirstOrDefault());
                            }
                        }
                    }

                }
                else
                {
                    ViewBag.IsExistTagDeficiency = "false";
                    inspectionActivity = _inspectionActivityService.GetDeficiencyAssets(userid, null, null);
                }
            }
            if (inspectionActivity.Count > 0)
            {
                inspectionActivity = inspectionActivity.GroupBy(test => test.ActivityId)
                      .Select(grp => grp.First())
                      .ToList();
            }
            foreach (var item in inspectionActivity)
            {
                item.TaggedMaster = taggedMaster.Where(x => x.ActivityId == item.ActivityId).ToList();
            }
            return await Task.FromResult(View(inspectionActivity));
        }
    }
}
