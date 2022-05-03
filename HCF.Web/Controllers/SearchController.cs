using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using HCF.BAL;
using HCF.BDO;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IAssetsService _assetService;
        private readonly ISearchService _searchService;
        private readonly IOrganizationService _organizationService;
        private readonly IEpService _epService;

        public SearchController(
            IEpService epService,            
            IAssetsService assetService,
            ISearchService searchService,
            IOrganizationService organizationService) 
        {
            _epService = epService;
            _assetService = assetService;
            _searchService = searchService;
            _organizationService = organizationService;
        }

        #region Search

        public ActionResult SearchView()
        {
            if (!Directory.Exists(Utility.CommonUtility.GetlucenePath()))
                Directory.CreateDirectory(Utility.CommonUtility.GetlucenePath());

            GoLucene.AddUpdateLuceneIndex(_searchService.GetAll());
            SearchFilter filter = new SearchFilter();
            return View(filter);
        }

        // [OutputCache(Duration = 60 * 60 * 24)]
        public ActionResult SearchFilters(SearchFilter objSearchFilter)
        {
            GoLucene.AddUpdateLuceneIndex(_searchService.GetILSM());
            ViewBag.Assets = _assetService.GetAllAsset();
            ViewBag.Scores = _organizationService.GetScores();
            return PartialView("_SearchFilters", objSearchFilter);
        }

        public void getIlsm()
        {
            GoLucene.AddUpdateLuceneIndex(_searchService.GetILSM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResults(SearchFilter searchLists)
        {
            searchLists = FilterData(searchLists);
            return PartialView("_SearchResults", searchLists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchView(SearchFilter filter)
        {
            filter = FilterData(filter);
            return View(filter);
        }

        private SearchFilter FilterData(SearchFilter filter)
        {
            filter.OwnerID = Base.UserSession.CurrentUser.UserId;
            if (filter.IsSaveSearchData)
                _searchService.Save(filter);
            if (filter.SearchType != 4)
                filter = _searchService.GetSearchResult(filter);
            else
                filter.SampleData = SearchLucene(filter.SearchTerm, filter.SearchField, true);
            return filter;
        }

        private static List<SampleData> SearchLucene(string searchTerm, string searchField, bool? searchDefault)
        {

            // GoLucene.
            List<SampleData> searchResults;
            if (searchDefault == true)
                searchResults = (string.IsNullOrEmpty(searchField)
                                   ? GoLucene.SearchDefault(searchTerm)
                                   : GoLucene.SearchDefault(searchTerm, searchField)).ToList();
            else
                searchResults = (string.IsNullOrEmpty(searchField)
                                   ? GoLucene.Search(searchTerm)
                                   : GoLucene.Search(searchTerm, searchField)).ToList();
            if (string.IsNullOrEmpty(searchTerm) && !searchResults.Any())
                searchResults = GoLucene.GetAllIndexRecords().ToList();


            return searchResults;
        }


        public ActionResult SearchPopUp()
        {
            var filter = new SearchFilter();
            return PartialView("_SeachPopUp", filter);
        }

        public ActionResult BindSearchFilter(Guid filterId)
        {
            //int ownerId = Base.UserSession.CurrentUser.UserId;
            var searchFilter = _searchService.GetSearchFilter(filterId);
            return Json(searchFilter);
        }

        #endregion

        #region Lucene       

        public ActionResult Search(string searchTerm, string searchField, string searchDefault)
        {
            return RedirectToAction("Index", new { searchTerm, searchField, searchDefault });
        }

        public ActionResult CreateIndex()
        {
            GoLucene.AddUpdateLuceneIndex(_searchService.GetAll());
            TempData["Result"] = "Search index was created successfully!";
            return RedirectToAction("SearchView");
        }


        public ActionResult ClearIndex()
        {
            if (GoLucene.ClearLuceneIndex())
                TempData["Result"] = "Search index was cleared successfully!";
            else
                TempData["ResultFail"] = "Index is locked and cannot be cleared, try again later or clear manually!";
            return RedirectToAction("SearchView");
        }


        public ActionResult OptimizeIndex()
        {
            GoLucene.Optimize();
            TempData["Result"] = "Search index was optimized successfully!";
            return RedirectToAction("SearchView");
        }

        #endregion


        #region

        public ActionResult SearchIndex()
        {
            if (!Directory.Exists(Utility.CommonUtility.GetlucenePath()))
                Directory.CreateDirectory(Utility.CommonUtility.GetMasterLucenePath());

            var dataTable = _epService.GetMasterEPsData();
            GoLucene.AdEPsLuceneIndex(dataTable);
            return View();
        }

        #endregion

    }
}