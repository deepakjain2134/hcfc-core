using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BDO;
using HCF.Web.Base;
using HCF.BAL;
using HCF.Utility;
using HCF.BAL.Interfaces.Services;
using HCF.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HCF.Utility.AppConfig;
using HCF.Web.Base.Factory;

namespace HCF.Web.Controllers
{
    public class TipsController : BaseController
    {

        private readonly IMenuService _menuService;
        private readonly ITipsService _tipsService;
        private readonly IEmailService _emailService;
        private readonly ICommonService _commonService;
        private readonly IOrganizationService _orgService;
        private readonly IAppSetting _appSetting;

        public TipsController(IAppSetting appSetting, IOrganizationService orgService, IMenuService menuService, ITipsService tipsService,
            IEmailService emailService, ICommonService commonSservice)
        {
            _appSetting = appSetting;
            _menuService = menuService;
            _tipsService = tipsService;
            _emailService = emailService;
            _commonService = commonSservice;
            _orgService = orgService;
        }

        public ActionResult Tips()
        {
            UISession.AddCurrentPage("tips_index", 0, "Tips");
            List<Tips> tips;
            tips = _tipsService.GetAllTips(UserSession.CurrentOrg.ClientNo);
            var routeData = Getrouteurldata();
            foreach (var tip in tips)
            {
                tip.RouteUrl = routeData.FirstOrDefault(x => x.Item2 == tip.RouteUrl)?.Item1;
            }
            return View(tips);
        }



        public ActionResult ViewTips(string pageUrl)
        {
            List<Tips> tips;
            tips = _tipsService.GetTipsByClientNo(UserSession.CurrentOrg.ClientNo, pageUrl);
            return View(tips);
        }

        [HttpGet]
        public ActionResult AddOrEditTips(string routeUrl = "", int id = 0)
        {
            if (!string.IsNullOrEmpty(routeUrl))
            {
                var menu = new Menus
                {
                    Alias = routeUrl
                };
                var pageInfo = UISession.GetBreadCrumbs().Where(x => x.Screen.ToLower() == routeUrl.ToLower()).FirstOrDefault();
                if (pageInfo != null)
                    menu.Name = pageInfo.DisplayName;
                else
                    menu.Name = routeUrl.Replace("_", "");

                menu.IsActive = true;
                menu.CreatedBy = UserSession.CurrentUser.UserId;
                menu.Type = 2;
                menu.Id = _menuService.Update(menu);
            }

            TempData["RouteURL"] = routeUrl ?? "";
            UISession.AddCurrentPage("tips_addoredit", 0, "Manage Tips");
            var tip = new Tips();
            if (id > 0)
            {
                tip = _tipsService.GetAllTips(UserSession.CurrentOrg.ClientNo).FirstOrDefault(x => x.TipId == id);
            }

            if (tip.TipId == 0)
            {
                tip.RouteUrl = string.IsNullOrEmpty(tip.RouteUrl) ? routeUrl : tip.RouteUrl;
                tip.IsActive = true;
                // return View(tip);
            }
            return View(tip);
        }

        //[ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditTips(Tips tip)
        {
            var routeData = Getrouteurldata();
            tip.RouteName = routeData.FirstOrDefault(x => x.Item2 == tip.RouteUrlPath).Item1;
            var existtip = _tipsService.GetAllTips(UserSession.CurrentOrg.ClientNo).FirstOrDefault(x => x.TipId == tip.TipId);
            tip.IsApproved = tip.TipType == BDO.Enums.TipTypes.HelpPageText || tip.TipType == BDO.Enums.TipTypes.FAQHowTo ? 1 : tip.IsApproved;
            if (existtip != null)
            {
                tip.IsApproved = tip.Title == tip.RouteName && existtip.IsApproved == 1 ? 1 : tip.IsApproved;
            }
            if (tip.TipType == 0 && tip.TipId != 0)
            {
                tip.TipType = existtip.TipType;
                tip.IsApproved = existtip.IsApproved;
            }
            if (tip.TipId == 0)
            {
                tip.UpdatedBy = tip.CreatedBy = UserSession.CurrentUser.UserId;
                //if (CommonRepository.IsSendMail(Enums.NotificationCategory.Tips.ToString(), Enums.NotificationEvent.OnCreated.ToString()))
                //{
                //    Email.SendTipsCreatedMail(tip, Convert.ToInt32(Enums.NotificationCategory.Tips));
                //}
            }
            else
            {
                tip.UpdatedBy = UserSession.CurrentUser.UserId;
            }

            SuccessMessage = UserSession.CurrentUser.IsSystemUser ? ConstMessage.Tip_Added : ConstMessage.Tip_send_for_approval;

            tip.ClientNo = UserSession.CurrentOrg.ClientNo;

            _tipsService.InsertOrUpdateTip(tip);
            return RedirectToRoute("tips");
        }

        public ActionResult CancelTips()
        {
            //object routeurl = TempData["RouteURL"] != "" ? TempData["RouteURL"] : null;
            if (TempData["RouteURL"] != null)
            {
                var routeurl = TempData["RouteURL"]; ;
                return Redirect(_appSetting.WebUrlPath + routeurl);
            }
            return RedirectToRoute("tips");
        }

        public ActionResult ApproveTip(int tipId, int approveStatus)
        {
            var tipItem = _tipsService.GetAllTips(UserSession.CurrentOrg.ClientNo).FirstOrDefault(x => x.TipId == tipId);
            var status = _tipsService.ApproveTip(tipId, approveStatus);
            //    var tip = BAL.TipsRespository.GetAllTips(UserSession.CurrentOrg.ClientNo);
            if (approveStatus == 0)
            {
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Tips.ToString(), BDO.Enums.NotificationEvent.OnFailed.ToString()))
                {
                    _emailService.SendTipsFailMail(tipItem, Convert.ToInt32(BDO.Enums.NotificationCategory.Tips));
                }
            }
            else
            {
                if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Tips.ToString(), BDO.Enums.NotificationEvent.OnPassed.ToString()))
                {
                    _emailService.SendTipsPassMail(tipItem, Convert.ToInt32(BDO.Enums.NotificationCategory.Tips));
                }
            }

            return Json(new { success = status });
        }

        public ActionResult SaveTip(string description, string pagename, string mode)
        {
            var tip = new Tips();
            var menus = _menuService.GetMenus();
            var routeData = Getrouteurldata();
            tip.UpdatedBy = tip.CreatedBy = UserSession.CurrentUser.UserId;
            tip.ClientNo = UserSession.CurrentOrg.ClientNo;
            tip.Description = description;
            tip.IsActive = true;
            tip.IsApproved = 2;
            tip.RouteUrlPath = pagename;
            tip.RouteName = tip.Title = routeData.FirstOrDefault(x => x.Item2 == tip.RouteUrlPath).Item1;
            tip.TipType = mode == "tip" ? BDO.Enums.TipTypes.HelpPageText : BDO.Enums.TipTypes.CRXSuggestion;
            tip.ContributorName = UserSession.CurrentUser.FullName; //user.FullName;
            tip.ContributorOrg = "HCF";
            if (_commonService.IsSendMail(BDO.Enums.NotificationCategory.Tips.ToString(), BDO.Enums.NotificationEvent.OnCreated.ToString()))
            {
                _emailService.SendTipsCreatedMail(tip, Convert.ToInt32(BDO.Enums.NotificationCategory.Tips));
            }
            var value = _tipsService.InsertOrUpdateTip(tip);
            var result = UserSession.CurrentUser.IsSystemUser ? ConstMessage.Tip_Added : ConstMessage.Tip_send_for_approval;
            var response = new
            {
                result
            };
            return Json(response);
        }

        public void GetTipType(int TipId)
        {
            TempData["SelectedTip"] = TipId;
        }
        public JsonResult RouteUrlData()
        {
            var routeData = Getrouteurldata();
            return Json(routeData);
        }


        private List<Tuple<string, string, string, string>> Getrouteurldata()
        {
            var routeData = new List<Tuple<string, string, string, string>>();
            var menus = _menuService.GetMenus();
            foreach (var menu in menus.Where(x => x.ParentId == 0 || x.ParentId == 1).OrderBy(x => x.Seq))
            {
                var pageText = menu.Name;
                var pageUrl = menu.PageUrl;
                var alias = menu.Alias;
                var data = GetModuleTip(pageText, pageUrl, alias, menu.Id, menu.ParentId);
                routeData.Add(data);
                foreach (var menu2 in menus.Where(x => x.ParentId == menu.Id).OrderBy(x => x.Seq))
                {
                    var pageText1 = menu2.Name;
                    var pageUrl1 = menu2.PageUrl;
                    var alias1 = menu2.Alias;
                    var data1 = GetModuleTip(pageText1, pageUrl1, alias1, menu2.Id, menu2.ParentId);
                    routeData.Add(data1);
                    foreach (var menu3 in menus.Where(x => x.ParentId == menu2.Id).OrderBy(x => x.Seq))
                    {
                        var pageText2 = menu3.Name;
                        var pageUrl2 = menu3.PageUrl;
                        var alias2 = menu3.Alias;
                        var data2 = GetModuleTip(pageText2, pageUrl2, alias2, menu3.Id, menu2.ParentId);
                        routeData.Add(data2);
                    }
                }
            }

            //foreach (var menu in menus.Where(x => x.ParentId == 0))
            //{
            //    var pageText = menu.Name;
            //    var pageUrl = menu.PageUrl;

            //        if (pageUrl == "activityDashboard") { pageText = "Activity Dashboard"; }
            //        var route = ((System.Web.Routing.Route)Url.RouteCollection[pageText]).Defaults;
            //        var routeValue = route["Controller"] + "_" + route["Action"];
            //        routeData.Add(new KeyValuePair<string, string>(pageText, routeValue.ToLower()));

            //}
            return routeData.Where(x => x.Item1 != "" && x.Item2 != "" && x.Item3 != "").ToList();
        }


        private Tuple<string, string, string, string> GetModuleTip(string pageText, string pageUrl, string alias, int id, int parentid)
        {
            return new Tuple<string, string, string, string>(pageText, alias.ToLower(), id.ToString(), parentid.ToString());
        }

        #region HowTo

        //[CrxAuthorize(Roles = "Tips_HowTo")]
        public ActionResult HowTo()
        {
            UISession.AddCurrentPage("tips_HowTo", 0, "How To");
            TipsViewModel tipsvm = new TipsViewModel()
            {
                Tips = MenuBasedTips(),
                Menus = _orgService.GetUserDashBoard(HCF.Web.Base.UserSession.CurrentUser.UserId, 0).Services.ToList()
            };
            //var helps = MenuBasedTips();
            return View(tipsvm);
        }

        private List<Tips> MenuBasedTips()
        {
            List<Tips> tips = new List<Tips>();
            var helps = UserSession.PageHelp;
            var menu = _orgService.GetUserDashBoard(UserSession.CurrentUser.UserId, 0).Services;
            foreach (var howto in menu)
            {
                var selectedTip = helps.Where(x => x.RouteName == howto.Name).ToList();
                if (selectedTip.Count != 0)
                {
                    foreach (var tip in selectedTip)
                        tips.Add(tip);
                }
            }
            return tips.GroupBy(x => x.Title, (key, group) => group.First()).ToList();
        }
        public ActionResult CRxHelp()
        {
            var helps = MenuBasedTips();
            return PartialView("_crxhelp", helps);
        }

        public ActionResult FAQs()
        {
            var helps = MenuBasedTips();
            return PartialView("_FAQsHowto", helps);
        }

        public ActionResult LinkHowTo(string routename, string mode, int id)
        {
            ViewBag.Active = id;
            var helps = MenuBasedTips();
            if (routename == "All" && mode == "FAQsHowto")
            {
                return PartialView("_FAQsHowto", helps);
            }
            else if (routename == "All" && mode == "CrxHelp")
            {
                return PartialView("_crxhelp", helps);
            }
            else
            {
                var lists = routename == "" ? helps : helps.Where(x => x.RouteName == routename).ToList();  /*x.RouteName.Contains(routename)).ToList()*/
                if (mode == "FAQsHowto")
                    return PartialView("_FAQsHowto", lists);
                else return PartialView("_crxhelp", lists);
            }
        }

        public ActionResult SearchBox(string routename, string mode, int id)
        {
            var helps = MenuBasedTips();
            var lists = routename == "" ? helps : helps.Where(x => x.RouteUrl.Contains(routename)).ToList();  /*x.RouteName.Contains(routename)).ToList()*/
            if (mode == "FAQsHowto")
                return PartialView("_FAQsHowto", lists);
            else return PartialView("_crxhelp", lists);
        }

        #endregion
    }
}