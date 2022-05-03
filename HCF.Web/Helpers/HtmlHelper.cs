//using HCF.BAL;
//using HCF.BDO;
//using HCF.Utility;
//using HCF.Web.Models;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Web.Mvc;
//using HCF.Web.Base;
//using System.Reflection;
//using HCF.BAL.Interfaces.Services;
//using HCF.BAL.Ioc;
//using Microsoft.AspNetCore.Mvc.Rendering;

//namespace HCF.Web.Helpers
//{
//    public static class HtmlHelper
//    {

//        public static MvcHtmlString BuildingDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, bool isActiveOnly = true)
//        {
//            var listItems = new List<SelectListItem>();
//            var values = UnityContextFactory<IBuildingsService>.CreateContext().GetBuildings().ToList();
//            if (isActiveOnly)
//                values = values.Where(x => x.IsActive).ToList();

//            var sites = values.OrderBy(y => y.SiteName).GroupBy(x => x.SiteName);
//            if (!string.IsNullOrEmpty(firstValue))
//                listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var site in sites)
//            {
//                var optionGroup = new SelectListGroup() { Name = site.Key };
//                listItems.AddRange(site.Select(buildings => new SelectListItem() { Value = buildings.BuildingId.ToString(), Text = buildings.BuildingName, Group = optionGroup, Selected = (buildings.BuildingId == selectedValue) }));
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }

//        public static MvcHtmlString BuildingList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, string showbuilding, bool isActiveOnly = true)
//        {
//            var listItems = new List<SelectListItem>();
//            var values = UnityContextFactory<IBuildingsService>.CreateContext().GetBuildings().ToList();
//            List<Buildings> newist = new List<Buildings>();
//            string[] buildingval = showbuilding.Split(',');
//            foreach (var buildingitem in values)
//            {
//                if (buildingval.Length > 0)
//                {
//                    foreach (string building in buildingval)
//                    {
//                        if (buildingitem.BuildingId == Convert.ToInt32(building))
//                        {
//                            newist.Add(buildingitem);
//                        }

//                    }
//                }
//            }
//            if (isActiveOnly)
//                values = newist.Where(x => x.IsActive).ToList();
//            else
//                values = newist.ToList();

//            var sites = values.OrderBy(y => y.SiteName).GroupBy(x => x.SiteName);
//            if (!string.IsNullOrEmpty(firstValue))
//                listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });


//            foreach (var site in sites)
//            {
//                //var optionGroup = new SelectListGroup() { Name = site.Key };
//                listItems.AddRange(site.Select(buildings => new SelectListItem() { Value = buildings.BuildingId.ToString(), Text = string.Format("{0} - {1} [{2}] ", buildings.SiteCode, buildings.BuildingName, buildings.BuildingCode), Selected = (buildings.BuildingId == selectedValue) }));
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString MultiSelectBuildingDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes, bool isActiveOnly = true)
//        {
//            var listItems = new List<SelectListItem>();
//            var values = UnityContextFactory<IBuildingsService>.CreateContext().GetBuildings().ToList();
//            if (isActiveOnly)
//                values = values.Where(x => x.IsActive && x.SiteActive).ToList();

//            var sites = values.OrderBy(x => x.SiteName).GroupBy(x => x.SiteName);

//            if (!string.IsNullOrEmpty(selectedValue))
//                selectedValue = "," + selectedValue.Trim() + ",";

//            //listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var site in sites)
//            {
//                var optionGroup = new SelectListGroup() { Name = site.Key };
//                listItems.AddRange(site.Select(buildings => new SelectListItem() { Value = buildings.BuildingId.ToString(), Text = buildings.BuildingName, Group = optionGroup, Selected = selectedValue.Contains("," + Convert.ToString(buildings.BuildingId) + ",") }));
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString FloorsDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, int buildingId, string firstValue, object htmlAttributes, bool isActiveOnly = true)
//        {
//            var listItems = new List<SelectListItem>();
//            var floors = UnityContextFactory<IFloorService>.CreateContext().GetFloors().Where(x => x.BuildingId == buildingId && x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in floors)
//            {
//                var selItem = new SelectListItem { Text = item.FloorName, Value = Convert.ToString(item.FloorId) };
//                if (selItem != null && item.FloorId > 0 && selectedValue == item.FloorId)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString SiteDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var textes = new List<SelectListItem>();
//            var sites = UnityContextFactory<ISiteService>.CreateContext().GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
//            textes.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in sites)
//            {
//                var selItem = new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Code.Trim()) };
//                if (!string.IsNullOrEmpty(selectedValue) && item.Code.Trim().ToLower() == Convert.ToString(selectedValue).ToLower())
//                    selItem.Selected = true;
//                textes.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     textes,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString UsersList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, string firstValueId = "", bool isshowVendorUseronly = false)
//        {
//            var listItems = new List<SelectListItem>();
//            var users = UnityContextFactory<IUserService>.CreateContext().Get().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            // var users = Users.GetUsersProfileDetails().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            users = Base.Common.RemoveCRxUsers(users);
//            if (isshowVendorUseronly)
//            {
//                if (HCF.Web.Base.UserSession.CurrentUser.IsVendorUser)
//                    users = users.Where(x => x.VendorId == HCF.Web.Base.UserSession.CurrentUser.VendorId).ToList();
//            }
//            listItems.Add(new SelectListItem { Value = firstValueId, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.FullName.ToLower())} ({(string.IsNullOrEmpty(item.Email) ? "NA Email" : item.Email)})",
//                    Value = Convert.ToString(item.UserId)
//                };
//                if (selectedValue > 0 && item.UserId == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString UserDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, string firstValueId = "", bool isshowVendorUseronly = false)
//        {
//            var listItems = new List<SelectListItem>();
//            var users = UnityContextFactory<IUserService>.CreateContext().GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            // var users = Users.GetUsersProfileDetails().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            //users = Base.Common.RemoveCRxUsers(users);
//            if (isshowVendorUseronly)
//            {
//                if (UserSession.CurrentUser.IsVendorUser)
//                    users = users.Where(x => x.VendorId == UserSession.CurrentUser.VendorId).ToList();
//            }
//            listItems.Add(new SelectListItem { Value = firstValueId, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.FullName.ToLower())} ({(string.IsNullOrEmpty(item.Email) ? "NA Email" : item.Email)})",
//                    Value = Convert.ToString(item.UserId)
//                };
//                if (selectedValue > 0 && item.UserId == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ApprovalStatusDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var approvalStatus = from Enums.ApprovalStatus e in Enum.GetValues(typeof(Enums.ApprovalStatus))
//                                 select new
//                                 {
//                                     Value = (int)e,
//                                     Text = e.GetDescription().ToString()
//                                 };

//            //listItems.Add(new SelectListItem { Value = firstValueId, Text = firstValue });
//            foreach (var item in approvalStatus.OrderBy(x => x.Text))
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString RiskTypeDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();

//            var defaultSelectListItem = new SelectListItem { Text = "Risk Score", Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.RiskScore e in Enum.GetValues(typeof(Enums.RiskScore))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };


//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue.HasValue && item.Value == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        [OutputCache]
//        public static MvcHtmlString StaticImage(this System.Web.Mvc.HtmlHelper htmlHelper, object htmlAttributes)
//        {
//            var builder = new TagBuilder("image");
//            BindAttributes(htmlAttributes, builder);
//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }
//        public static MvcHtmlString SpanBoolLabel(this System.Web.Mvc.HtmlHelper htmlHelper, bool status, string firstValue, string secondValue, object htmlAttributes)
//        {
//            var builder = new TagBuilder("span");
//            builder.SetInnerText((status) ? firstValue : secondValue);
//            BindAttributes(htmlAttributes, builder);
//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
//        }
//        public static MvcHtmlString UserEPsDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, string firstValueId = "")
//        {
//            var listItems = new List<SelectListItem>();
//            var users = UnityContextFactory<IUserService>.CreateContext().GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            users = Base.Common.RemoveCRxUsers(users);
//            listItems.Add(new SelectListItem { Value = firstValueId, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{item.FullName} ({item.EpsCount}) {item.Email}",
//                    Value = Convert.ToString(item.UserId)
//                };
//                if (selectedValue > 0 && item.UserId == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                name,
//                listItems,
//                htmlAttributes);

//        }
//        public static MvcHtmlString FrequencypDownList(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var users = UnityContextFactory<IFrequencyService>.CreateContext().GetFrequency().Where(x => x.IsActive).ToList();
//            //var users = FrequencyRepository.GetFrequency().ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.DisplayName,
//                    Value = Convert.ToString(item.FrequencyId)
//                };
//                if (selectedValue.HasValue && item.FrequencyId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString UserSearchDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFilters = new List<SearchFilter>();//  //_searchService.GetSearchFilter(UserSession.CurrentUser.UserId);
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in searchFilters)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.FilterName,
//                    Value = Convert.ToString(item.FilterId)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && item.FilterId.ToString() == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString StandardDownList(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IStandardService>.CreateContext().GetStandards().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.TJCStandard,
//                    Value = Convert.ToString(item.StDescID)
//                };
//                if (selectedValue.HasValue && item.StDescID.ToString() == selectedValue.Value.ToString())
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString CommonSeachDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFieldList = new
//              List<SelectedList> {
//                                         new SelectedList {Text = "(All Fields)", Value =  string.Empty},
//                                         new SelectedList {Text = "Asset #", Value = "AssetNo"},
//                                         new SelectedList {Text = "Barcode #", Value = "BarCodeNo"},
//                                         new SelectedList {Text = "Binder Name", Value = "BinderName"},
//                                         new SelectedList {Text = "ILSM Incident #", Value = "IncidentNo"}
//                                     };
//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue.HasValue && item.Value == selectedValue.Value.ToString())
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString ManufacturerDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();

//            var users = UnityContextFactory<IManufactureService>.CreateContext().GetAll().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.ManufactureName,
//                    Value = Convert.ToString(item.ManufactureId)
//                };
//                if (selectedValue.HasValue && item.ManufactureId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString RoundCategoryDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            // int userid = Base.UserSession.CurrentUser.UserId;
//            var listItems = new List<SelectListItem>();
//            var roundsCateg = UnityContextFactory<IRoundsService>.CreateContext().GetRoundCategories_Data().ToList(); //.Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in roundsCateg)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.CategoryName,
//                    Value = Convert.ToString(item.RoundCatId)
//                };
//                if (selectedValue.HasValue && item.RoundCatId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }

//        public static MvcHtmlString RoundCommonCategoryDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var roundsCateg = UnityContextFactory<IRoundsService>.CreateContext().GetCommonRoundCategory().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in roundsCateg)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.CategoryName,
//                    Value = Convert.ToString(item.RoundCatId)
//                };
//                if (selectedValue.HasValue && item.RoundCatId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString AssetStatusDropDown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFieldList = new
//              List<SelectedList> {
//                                         new SelectedList {Text = firstValue, Value =  string.Empty},
//                                         new SelectedList {Text = "ACTIVE -In Use", Value = "ACTIV"},
//                                         new SelectedList {Text = "INACT -In Inventory", Value = "INACT"},
//                                         new SelectedList {Text = "RETIR -Obsolete", Value = "RETIR"},

//                                     };
//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && selectedValue == selItem.Value)
//                {
//                    selItem.Selected = true;
//                }
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }

//        public static MvcHtmlString ActiveStatusDropDown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFieldList = new
//              List<SelectedList> {
//                                         new SelectedList {Text = firstValue, Value =  string.Empty},
//                                         new SelectedList {Text = "ACTIVE", Value = "ACTIV"},
//                                         new SelectedList {Text = "INACTIV", Value = "INACTIV"}

//                                     };
//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && selectedValue == selItem.Value)
//                {
//                    selItem.Selected = true;
//                }
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }

//        public static MvcHtmlString RouteUrlDropdown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var tipRouteUrl = AppSettings.TipRouteUrls.Split(',').ToList();
//            var searchFieldList = new List<SelectedList> { new SelectedList() { Text = "Select route URL", Value = "" } };
//            foreach (var tiprouteurl in tipRouteUrl)
//            {
//                searchFieldList.Add(new SelectedList() { Text = tiprouteurl, Value = tiprouteurl });
//            }

//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && selectedValue == selItem.Value)
//                {
//                    selItem.Selected = true;
//                }
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString TipTypeDropdown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var tipTypes = AppSettings.TipType.Split(',').ToList();
//            var searchFieldList = new List<SelectedList>
//            {
//                new SelectedList() { Text = "Select Tip Type", Value = "" }
//            };
//            foreach (var tiptyp in tipTypes)
//            {
//                var index = tipTypes.IndexOf(tiptyp) + 1;
//                searchFieldList.Add(new SelectedList() { Text = tiptyp, Value = index.ToString() });
//            }

//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && selectedValue == selItem.Value)
//                {
//                    selItem.Selected = true;
//                }
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString TipStatusDropdown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFieldList = new List<SelectedList>
//            {
//                new SelectedList() { Text = "Pending", Value = 2.ToString() },
//                new SelectedList() { Text = "Approved", Value = 1.ToString() },
//                new SelectedList() { Text = "Rejected", Value = 0.ToString() }
//            };

//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (!string.IsNullOrEmpty(selectedValue) && selectedValue == selItem.Value)
//                {
//                    selItem.Selected = true;
//                }
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString EPAssetTypeDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IAssetsService>.CreateContext().GetAssetsByType(Base.UserSession.CurrentUser.UserId, 0).Where(y => y.Assets.Any(x => x.Count > 0 && x.StandardEps.Any())).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name,
//                    Value = Convert.ToString(item.TypeId)
//                };
//                if (selectedValue > 0 && item.TypeId == selectedValue)
//                    selItem.Selected = true;
//                else
//                    selItem.Selected = false;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString EPAssetsDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IAssetsService>.CreateContext().GetAssetsByType(Base.UserSession.CurrentUser.UserId, 0).ToList();
//            listItems.Add(new SelectListItem { Value = "-1", Text = firstValue.ToTitleCase() });
//            //listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists.Where(x => x.IsActive))
//            {
//                foreach (var asset in item.Assets.Where(y => y.Count > 0 && y.StandardEps.Any() && y.IsActive).OrderBy(x => x.Name))
//                {
//                    var selItem = new SelectListItem
//                    {
//                        Text = asset.Name,
//                        Value = Convert.ToString(asset.AssetId)
//                    };
//                    if (!string.IsNullOrEmpty(selectedValue) && asset.AssetId == Convert.ToInt32(selectedValue))
//                        selItem.Selected = true;
//                    else
//                        selItem.Selected = false;
//                    listItems.Add(selItem);
//                }

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString ScheduleDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = new List<Schedules>(); //ScheduleRepository.GetSchedule().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.ReferenceName.CastToNA(),
//                    Value = Convert.ToString(item.ScheduleId)
//                };
//                if (selectedValue > 0 && item.ScheduleId == selectedValue)
//                    selItem.Selected = true;
//                else
//                    selItem.Selected = false;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString QHeaderDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IQuestionnairesService>.CreateContext().QuestionnaireHeaderTypes().Where(x => x.IsActive).ToList();
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name,
//                    Value = Convert.ToString(item.QuestionnaireHeaderTypeId)
//                };
//                selItem.Selected = selectedValue > 0 && selItem.Value == Convert.ToString(selectedValue);
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString WeekDropDown(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var searchFieldList = new List<SelectedList> {
//                                         new SelectedList {Text = "First", Value =  "1"},
//                                         new SelectedList {Text = "Second", Value = "2"},
//                                         new SelectedList {Text = "Third", Value = "3"},
//                                         new SelectedList {Text = "Fourth", Value = "4"},
//                                         new SelectedList {Text = "Last", Value = "5"},

//                                     };
//            foreach (var item in searchFieldList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ActivityTypeDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IGoalMasterService>.CreateContext().GetActivityType();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name.CastToNA(),
//                    Value = Convert.ToString(item.ActivityTypeId)
//                };
//                if (selectedValue > 0 && item.ActivityTypeId == selectedValue)
//                    selItem.Selected = true;
//                else
//                    selItem.Selected = false;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString AssetTypeDownList(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IAssetTypeService>.CreateContext().GetAssetMaster().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue.ToTitleCase() });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name.ToTitleCase(),
//                    Value = Convert.ToString(item.TypeId)
//                };
//                if (selectedValue.HasValue && item.TypeId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString AssetsDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            List<Assets> lists = UnityContextFactory<IAssetsService>.CreateContext().Get().Where(x => x.IsActive).ToList();
//            if (selectedValue != "-2")
//            {
//                int[] values = lists.Select(x => x.AssetId).ToArray();
//                String.Join(",", values.ToArray());
//                if (selectedValue != null && selectedValue == "-1")
//                {
//                    listItems.Add(new SelectListItem { Value = "-1", Text = firstValue.ToTitleCase(), Selected = true });
//                }
//                else
//                {
//                    listItems.Add(new SelectListItem { Value = "-1", Text = firstValue.ToTitleCase() });
//                }
//            }
//            else
//                listItems.Add(new SelectListItem { Value = "", Text = firstValue.ToTitleCase() });



//            if (selectedValue != null && selectedValue == "-1")
//            {
//                foreach (var item in lists)
//                {
//                    var selItem = new SelectListItem
//                    {
//                        Text = item.Name.ToTitleCase(),
//                        Value = Convert.ToString(item.AssetId),
//                        Selected = true,
//                    };

//                    listItems.Add(selItem);
//                }
//            }
//            else
//            {
//                foreach (var item in lists)
//                {
//                    var selItem = new SelectListItem
//                    {
//                        Text = item.Name.ToTitleCase(),
//                        Value = Convert.ToString(item.AssetId),

//                    };
//                    if (!string.IsNullOrEmpty(selectedValue))
//                    {
//                        foreach (var select in selectedValue.Split(','))
//                        {
//                            if (!string.IsNullOrEmpty(@select) && item.AssetId == Convert.ToInt32(select))
//                                selItem.Selected = true;
//                        }
//                    }
//                    listItems.Add(selItem);
//                }
//            }

//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString ICRAListDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var lists = UnityContextFactory<IConstructionService>.CreateContext().GetIcras().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{item.ProjectName} ({item.ProjectNo})",
//                    Value = Convert.ToString(item.TicraId)
//                };
//                if (selectedValue > 0 && item.TicraId == selectedValue)
//                    selItem.Selected = true;
//                else
//                    selItem.Selected = false;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString StepsOptionsDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string showOnlyData, string firstValue, object htmlAttributes)
//        {
//            var mainList = UnityContextFactory<IQuestionnairesService>.CreateContext().QuestionnaireHeaderTypes().Where(x => x.IsActive == true).ToList();

//            var listItems = new List<SelectListItem>
//            {
//                new SelectListItem { Value = string.Empty, Text = firstValue }
//            };
//            var array = Array.ConvertAll(showOnlyData.Split(','), int.Parse);
//            var lists = mainList.Where(d => array.Contains(d.QuestionnaireHeaderTypeId));
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name,
//                    Value = item.Name//Convert.ToString(item.QuestionnaireHeaderTypeId);
//                };
//                if (!(string.IsNullOrEmpty(selectedValue)) && selItem.Text == Convert.ToString(selectedValue))
//                    selItem.Selected = true;
//                else
//                    selItem.Selected = false;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }

//        //public static MvcHtmlString VendorDropDownlist(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes,IVendorService vendorService)
//        //{
//        //    var listItems = new List<SelectListItem>();
//        //    var vendors = vendorService.GetVendors().Where(x => x.IsActive).ToList();
//        //    listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//        //    foreach (var item in vendors)
//        //    {
//        //        var selItem = new SelectListItem
//        //        {
//        //            Text = item.Name,
//        //            Value = Convert.ToString(item.VendorId)
//        //        };
//        //        if (selectedValue.HasValue && item.VendorId == selectedValue.Value)
//        //            selItem.Selected = true;
//        //        listItems.Add(selItem);

//        //    }
//        //    return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        //}
//        public static MvcHtmlString ICRAProjectDropDownlist(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();


//            var icraProjectList = AsyncHelpers.RunSync(() => UnityContextFactory<ITIcraProjectService>.CreateContext().GetAllTIcraProject());
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in icraProjectList)
//            {
//                SelectListItem selItem = new SelectListItem
//                {
//                    Text = $@"{item.ProjectName} ({item.ProjectNumber})",
//                    Value = Convert.ToString(item.ProjectId)
//                };
//                if (selectedValue.HasValue && item.ProjectId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ICRAParentProjectDropDownlist(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();

//            bool HideInactiveValue = true;
//            if (selectedValue.HasValue && selectedValue.Value > 0)
//            {
//                HideInactiveValue = false;
//            }
//            var data = AsyncHelpers.RunSync(() => UnityContextFactory<ITIcraProjectService>.CreateContext().GetAllTIcraProject());
//            if (!HideInactiveValue)
//            {
//                data = data.Where(x => x.Status == true || x.ProjectId == selectedValue.Value).ToList();
//            }
//            else
//            {
//                data = data.Where(x => x.Status == true).ToList();
//            }
//            var icraProjectList = data.Where(x => x.ParentProjectId == null).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in icraProjectList)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{item.ProjectName} ({item.ProjectNumber})",
//                    Value = Convert.ToString(item.ProjectId)
//                };
//                if (selectedValue.HasValue && item.ProjectId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ICRAProjectDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            bool HideInactiveValue = true;
//            if (selectedValue.HasValue && selectedValue.Value > 0)
//            {
//                HideInactiveValue = false;
//            }
//            var projects = UnityContextFactory<ITIcraProjectService>.CreateContext().GetAllActiveTIcraProject(HideInactiveValue).ToList();

//            if (!HideInactiveValue)
//            {
//                projects = projects.Where(x => x.Status == true || x.ProjectId == selectedValue.Value).ToList();
//            }
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in projects)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.ProjectName,
//                    Value = Convert.ToString(item.ProjectId)
//                };
//                if (selectedValue.HasValue && item.ProjectId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString SubProjectDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, int projectid, string firstValue, object htmlAttributes, bool isActiveOnly = true)
//        {
//            var listItems = new List<SelectListItem>();
//            bool HideInactiveValue = true;
//            if (selectedValue > 0)
//            {
//                HideInactiveValue = false;
//            }
//            var values = UnityContextFactory<ITIcraProjectService>.CreateContext().GetAllActiveTIcraProject(HideInactiveValue).Where(x => x.ParentProjectId == projectid || x.ParentProjectId == selectedValue && (x.Status == true || x.ProjectId == selectedValue)).ToList();

//            var projects = values.GroupBy(x => x.ProjectName);
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var project in projects)
//            {
//                var optionGroup = new SelectListGroup() { Name = project.Key };
//                listItems.AddRange(project.Select(proj => new SelectListItem() { Value = proj.ProjectId.ToString(), Text = proj.ProjectName + "(" + proj.ProjectNumber + ")", Selected = (proj.ProjectId == selectedValue) }));
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString StateDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            // var sites = UnityContextFactory<ISiteService>.CreateContext().GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
//            var listItems = new List<SelectListItem>();
//            var state = UnityContextFactory<ISiteService>.CreateContext().GetStates().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in state)
//            {
//                var selItem = new SelectListItem { Text = item.StateName + " ( " + item.StateCode + " )", Value = Convert.ToString(item.StateId) };
//                if (selItem != null && item.StateId > 0 && selectedValue == item.StateId)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }

//        public static MvcHtmlString StateCodeDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            // var sites = UnityContextFactory<ISiteService>.CreateContext().GetSites().Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
//            var listItems = new List<SelectListItem>();
//            var state = UnityContextFactory<ISiteService>.CreateContext().GetStates().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in state)
//            {
//                var selItem = new SelectListItem { Text = item.StateName + " ( " + item.StateCode + " )", Value = item.StateCode };
//                if (selItem != null && selectedValue == item.StateCode)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }


//        public static MvcHtmlString CityDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, int stateId, string firstValue, object htmlAttributes)
//        {

//            var listItems = new List<SelectListItem>();
//            var cities = UnityContextFactory<ISiteService>.CreateContext().GetCities(stateId).Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in cities)
//            {
//                var selItem = new SelectListItem { Text = item.CityName, Value = Convert.ToString(item.CityId) };
//                if (selItem != null && item.CityId > 0 && selectedValue == item.CityId)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString EpUserAssignementDropDown(this System.Web.Mvc.HtmlHelper htmlHelper, string name, List<UserProfile> users, int selectedValue, string firstValue, object htmlAttributes, bool showEmail, bool isShowCount = true, string firstValueId = "")
//        {
//            ///var users = Users.GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email) && x.IsCRxUser == false).OrderBy(x => x.FullName).ToList();
//            //users = Base.Common.RemoveCRxUsers(users);
//            var select = new TagBuilder("select");
//            var options = "";
//            foreach (var item in users)
//            {
//                var text = "";
//                if (isShowCount)
//                    text = item.FullName + " (" + ((item.IsPowerUser) ? " PU " + item.EpsCount : item.EpsCount.ToString()) + " )";
//                else
//                    text = item.FullName + " " + ((item.IsPowerUser) ? " PU " : ""); ;

//                //var text = item.FullName + " (" + subText + ")";
//                if (showEmail)
//                    text = text + " <br/>" + item.Email;

//                var option = new TagBuilder("option");
//                option.MergeAttribute("value", item.UserId.ToString());
//                option.MergeAttribute("data-domain", item.Email);
//                option.MergeAttribute("data-content", text);

//                if (selectedValue == item.UserId)
//                    option.MergeAttribute("selected", "selected");
//                option.SetInnerText(item.FullName);

//                if (item.IsPowerUser)
//                    option.MergeAttribute("enabled", "enabled");
//                options += option.ToString(TagRenderMode.Normal) + "\n";
//            }
//            select.MergeAttribute("id", name);
//            select.MergeAttribute("name", name);
//            BindAttributes(htmlAttributes, @select);
//            select.InnerHtml = options;
//            return new MvcHtmlString(select.ToString(TagRenderMode.Normal));
//        }
//        public static MvcHtmlString SiteTypeDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var siteTypes = UnityContextFactory<ISiteService>.CreateContext().GetSiteType().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in siteTypes)
//            {
//                var selItem = new SelectListItem { Text = item.SiteTypeName, Value = Convert.ToString(item.SiteTypeId) };
//                if (selItem != null && selectedValue == item.SiteTypeId)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }

//        public static MvcHtmlString VendorDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            //var vendorRepository = (IVendorService)provider.GetService(typeof(IVendorService));
//            var listItems = new List<SelectListItem>();
//            //GetVendor
//            var activeVendor = UnityContextFactory<IVendorService>.CreateContext().GetVendorDetails().Where(x => x.IsActive).ToList(); //vendorRepository.GetVendorDetails().Where(x => x.IsActive).ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in activeVendor)
//            {
//                var selItem = new SelectListItem { Text = item.Name, Value = Convert.ToString(item.VendorId) };
//                if (selItem != null && selectedValue == item.VendorId)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }

//        public static MvcHtmlString TipTypesDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, bool showAll, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.TipTypes e in Enum.GetValues(typeof(Enums.TipTypes))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };
//            foreach (var item in lists)
//            {
//                if (showAll == false && !UserSession.CurrentUser.IsSystemUser)
//                {
//                    if (item.Value == (int)Enums.TipTypes.HelpPageText ||
//                        item.Value == (int)Enums.TipTypes.FAQHowTo)
//                    {
//                        break;
//                    }
//                }

//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ParentRoleDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            try
//            {
//                var parentId = htmlAttributes.ToString().Replace("{", "").Replace("}", "").Split(',')[0].Split('=')[1].Trim();
//                var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//                listItems.Add(defaultSelectListItem);
//                var lists = UnityContextFactory<IUserService>.CreateContext().GetUserRoles("0");
//                foreach (var item in lists)
//                {
//                    var selItem = new SelectListItem
//                    {
//                        Text = item.DisplayText,
//                        Value = Convert.ToString(item.RoleId)
//                    };
//                    if (!String.IsNullOrEmpty(parentId) && item.RoleId == int.Parse(parentId))
//                        selItem.Selected = true;
//                    listItems.Add(selItem);
//                }
//            }
//            catch (Exception ex)
//            {
//                string exception = ex.Message;
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString PermitDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, bool showAll, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.PermitType e in Enum.GetValues(typeof(Enums.PermitType))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };
//            foreach (var item in lists.OrderBy(x => x.Text))
//            {

//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString FrequencyDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, bool showAll, object htmlAttributes, string FrequencyIds = "")
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);


//            var lists = from Enums.Frequency e in Enum.GetValues(typeof(Enums.Frequency))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };

//            if (!string.IsNullOrEmpty(FrequencyIds))
//            {
//                int[] frequencyIds = Array.ConvertAll(FrequencyIds.Split(','), int.Parse);
//                lists = lists.Where(x => frequencyIds.Contains(x.Value));
//            }

//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;


//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString UsDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes, string firstValueId = "", bool isshowVendorUseronly = false)
//        {
//            var listItems = new List<SelectListItem>();
//            var users = UnityContextFactory<IUserService>.CreateContext().GetUsers().Where(x => x.IsActive && !string.IsNullOrEmpty(x.Email)).OrderBy(x => x.FullName).ToList();
//            users = Base.Common.RemoveCRxUsers(users);
//            if (isshowVendorUseronly)
//            {
//                if (HCF.Web.Base.UserSession.CurrentUser.IsVendorUser)
//                    users = users.Where(x => x.VendorId == HCF.Web.Base.UserSession.CurrentUser.VendorId).ToList();
//            }
//            listItems.Add(new SelectListItem { Value = firstValueId, Text = firstValue });
//            foreach (var item in users)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = $@"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.FullName.ToLower())} ({(string.IsNullOrEmpty(item.Email) ? "NA Email" : item.Email)})",
//                    Value = Convert.ToString(item.UserId)
//                };
//                if (selectedValue > 0 && item.UserId == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);

//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString ProjectTypeDropDownlist(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var projecttype = UnityContextFactory<IConstructionService>.CreateContext().GetProjectType().ToList();
//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in projecttype)
//            {
//                SelectListItem selItem = new SelectListItem
//                {
//                    Text = item.Name,
//                    Value = Convert.ToString(item.ProjectTypeId)
//                };
//                if (selectedValue.HasValue && item.ProjectTypeId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString RiskAreaDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var projecttype = UnityContextFactory<IConstructionService>.CreateContext().GetICRARiskAra().Where(x => x.IsActive).ToList();

//            listItems.Add(new SelectListItem { Text = firstValue, Value = "" });
//            foreach (var item in projecttype)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Name,
//                    Value = Convert.ToString(item.RiskAreaId)
//                };
//                if (selectedValue.HasValue && item.RiskAreaId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);

//        }
//        public static MvcHtmlString FireRiskTypeDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, bool showAll, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.FireRiskType e in Enum.GetValues(typeof(Enums.FireRiskType))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };
//            foreach (var item in lists.OrderBy(x => x.Text))
//            {

//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString BinderListDropDown(this System.Web.Mvc.HtmlHelper helper, string name, int? selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var binders = UnityContextFactory<IDocumentsService>.CreateContext().GetBindersList().Where(x => x.IsActive).OrderBy(x => x.BinderName).ToList();
//            listItems.Add(new SelectListItem { Text = firstValue, Value = "" });
//            foreach (var item in binders)
//            {
//                SelectListItem selItem = new SelectListItem
//                {
//                    Text = item.BinderName,
//                    Value = Convert.ToString(item.BinderId)
//                };
//                if (selectedValue.HasValue && item.BinderId == selectedValue.Value)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }

//        #region  Common Methods

//        private static void BindAttributes(object htmlAttributes, TagBuilder @select)
//        {
//            var htmlAttrs = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
//            foreach (var thisAttribute in htmlAttrs)
//            {
//                if (!select.Attributes.ContainsKey(thisAttribute.Key))
//                    @select.Attributes.Add(thisAttribute.Key, thisAttribute.Value.ToString());
//            }
//        }

//        public static MvcHtmlString ControllerNameDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedController, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var asm = Assembly.GetExecutingAssembly();
//            var controllers = asm.GetTypes()
//                .Where(type => typeof(Controller).IsAssignableFrom(type) && type.IsClass)
//                .Select(m => m.Name.Replace("Controller", ""))
//                .ToList();

//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            foreach (var item in controllers)
//            {
//                var selItem = new SelectListItem { Text = item, Value = item };
//                if (selItem != null && selectedController == item)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);
//        }
//        public static MvcHtmlString ActionNameDropDownList(this System.Web.Mvc.HtmlHelper helper, string name, string selectedActionMethod, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var asm = Assembly.GetExecutingAssembly();

//            var actionMethods = asm.GetTypes()
//              .Where(type => typeof(Controller).IsAssignableFrom(type))
//              .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
//              .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
//              .Select(x => new
//              {
//                  Controller = x.DeclaringType.Name,
//                  ActionMethod = x.Name,
//                  // Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
//              })
//     .ToList();

//            listItems.Add(new SelectListItem { Value = string.Empty, Text = firstValue });
//            var controllerName = htmlAttributes.ToString().Replace("{", "").Replace("}", "").Split(',')[0].Split('=')[1].Trim().ToLower() + "controller";
//            actionMethods = actionMethods.Where(x => x.Controller.ToLower() == controllerName).Distinct().ToList();
//            foreach (var item in actionMethods)
//            {
//                var selItem = new SelectListItem { Text = item.ActionMethod, Value = item.ActionMethod };
//                if (selItem != null && selectedActionMethod == item.ActionMethod)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper,
//                                                                     name,
//                                                                     listItems,
//                                                                     htmlAttributes);

//        }
//        public static MvcHtmlString DeficiencyStatusDropdown(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.DeficiencyStatus e in Enum.GetValues(typeof(Enums.DeficiencyStatus))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        public static MvcHtmlString DocCategoryDropdown(this System.Web.Mvc.HtmlHelper helper, string name, int selectedValue, string firstValue, object htmlAttributes)
//        {
//            var listItems = new List<SelectListItem>();
//            var defaultSelectListItem = new SelectListItem { Text = firstValue, Value = "" };
//            listItems.Add(defaultSelectListItem);
//            var lists = from Enums.DocCategory e in Enum.GetValues(typeof(Enums.DocCategory))
//                        select new
//                        {
//                            Value = (int)e,
//                            Text = e.GetDescription()
//                        };
//            foreach (var item in lists)
//            {
//                var selItem = new SelectListItem
//                {
//                    Text = item.Text,
//                    Value = Convert.ToString(item.Value)
//                };
//                if (selectedValue > 0 && item.Value == selectedValue)
//                    selItem.Selected = true;
//                listItems.Add(selItem);
//            }
//            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, listItems, htmlAttributes);
//        }
//        #endregion
//    }
//}