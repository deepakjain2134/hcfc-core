using HCF.Infrastructure;
using HCF.Module.LuceneSearch.Areas.LuceneSearch.ViewModel;
using HCF.Module.LuceneSearch.Data;
using HCF.Module.LuceneSearch.Model;
using HCF.Module.LuceneSearch.Models;
using HCF.Module.LuceneSearch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HCF.Module.LuceneSearch.Areas.LuceneSearch.Controllers
{
    [Authorize]
    [Area("lucenesearch")]
    public class SearchDataController : Controller
    {
        private readonly IGoLucene _goLucene;
        
        public SearchDataController(IGoLucene goLucene)
        {
            _goLucene = goLucene;            
        }

        [HttpGet("search")]
        public ActionResult Index(string searchTerm, string searchField, bool? searchDefault, int? limit)
        {
            // create default Lucene search index directory
            string path = Path.Combine(GlobalConfiguration.WebRootPath, "lucene_index");           

            _goLucene.GetDirectory(path);

            // perform Lucene search
            List<SampleData> _searchResults;
            if (searchDefault == true)
                _searchResults = (string.IsNullOrEmpty(searchField)
                                   ? _goLucene.SearchDefault<SampleData>(searchTerm, LuceneFileDirectory.defaultpath)
                                   : _goLucene.SearchDefault<SampleData>(searchTerm, LuceneFileDirectory.defaultpath,searchField)).ToList();
            else
                _searchResults = (string.IsNullOrEmpty(searchField)
                                   ? _goLucene.Search<SampleData>(searchTerm, LuceneFileDirectory.defaultpath)
                                   : _goLucene.Search<SampleData>(searchTerm, LuceneFileDirectory.defaultpath,searchField)).ToList();
            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = _goLucene.GetAllIndexRecords<SampleData>().ToList();


            // setup and return view model
            var search_field_list = new
                List<SelectedList> {
                                         new SelectedList {Text = "(All Fields)", Value = ""},
                                         new SelectedList {Text = "Id", Value = "Id"},
                                         new SelectedList {Text = "Name", Value = "Name"},
                                         new SelectedList {Text = "Description", Value = "Description"}
                                     };

            // limit display number of database records
            var limitDb = limit == null ? 3 : Convert.ToInt32(limit);
            List<SampleData> allSampleData;
            if (limitDb > 0)
            {
                allSampleData = SampleDataRepository.GetAll().Take(limitDb).ToList();
                ViewBag.Limit = SampleDataRepository.GetAll().Count - limitDb;
            }
            else allSampleData = SampleDataRepository.GetAll();

            return View(new SearchIndexViewModel
            {
                AllSampleData = allSampleData,
                SearchIndexData = _searchResults,
                SampleData = new SampleData { Id = "9", Name = "El-Paso", Description = "City in Texas" },
                SearchFieldList = search_field_list,
            });
        }

        public ActionResult Search(string searchTerm, string searchField, string searchDefault)
        {
            return RedirectToAction("Index", new { searchTerm, searchField, searchDefault });
        }

        [HttpGet("create/index")]
        public ActionResult CreateIndex()
        {
            _goLucene.AddUpdateLuceneIndex<SampleData>(SampleDataRepository.GetAll());
            TempData["Result"] = "Search index was created successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddToIndex(SampleData sampleData)
        {
            _goLucene.AddUpdateLuceneIndex(sampleData);
            TempData["Result"] = "Record was added to search index successfully!";
            return RedirectToAction("Index");
        }

        public ActionResult ClearIndex()
        {
            if (_goLucene.ClearLuceneIndex())
                TempData["Result"] = "Search index was cleared successfully!";
            else
                TempData["ResultFail"] = "Index is locked and cannot be cleared, try again later or clear manually!";
            return RedirectToAction("Index");
        }

        public ActionResult ClearIndexRecord(int id)
        {
            _goLucene.ClearLuceneIndexRecord(id);
            TempData["Result"] = "Search index record was deleted successfully!";
            return RedirectToAction("Index");
        }

        public ActionResult OptimizeIndex()
        {
            _goLucene.Optimize();
            TempData["Result"] = "Search index was optimized successfully!";
            return RedirectToAction("Index");
        }
    }
}

