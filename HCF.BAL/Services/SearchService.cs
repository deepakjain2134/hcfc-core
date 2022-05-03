using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public class SearchService : ISearchService
    {
        private readonly ILoggingService _loggingService;
        private readonly IIlsmService _ilsmService;
        private readonly IDocumentsService _documentsService;
        private readonly IFloorAssetRepository _floorAssetRepository;
        private readonly ISearchRepository _searchRepository;
        public SearchService(ILoggingService loggingService, IFloorAssetRepository floorAssetRepository,IIlsmService ilsmService,IDocumentsService documentsService,
            ISearchRepository searchRepository)
        {
            _floorAssetRepository = floorAssetRepository;
            _loggingService = loggingService;
            _documentsService = documentsService;
            _ilsmService = ilsmService;
            _searchRepository = searchRepository;
        }

        #region Search filter

        public  int Save(SearchFilter objSearchFilter)
        {
            return _searchRepository.AddSearchFilter(objSearchFilter);
        }

        public  List<SearchFilter> GetSearchFilter(int userId)
        {
            return _searchRepository.GetSearchFilter(userId);
        }


        public  SearchFilter GetSearchFilter(Guid filterId)
        {
            return _searchRepository.GetSearchFilter(filterId);
        }

        #endregion

        #region Search Result

        public  SearchFilter GetSearchResult(SearchFilter objSearchFilter)
        {
            return _searchRepository.GetSearchResult(objSearchFilter);
        }

        #endregion


        #region Lucene

        public  SampleData Get(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
        }
        public  List<SampleData> GetAll()
        {
            var lists = new List<SampleData>();
          
            var binders = _documentsService.Get();
            var tfloorAssets = _floorAssetRepository.GetFloorAssets();
            var ilsms = _ilsmService.GetAllIlsm();
            foreach (var item in binders)
            {
                SampleData list = new SampleData() { Id = item.BinderId, Type = 4,  AssetNo = "", BarCodeNo = "", BinderName = item.BinderName, IncidentNo = "" };
                lists.Add(list);
            }

            foreach (var item in tfloorAssets)
            {
                SampleData list = new SampleData() { Id = item.FloorAssetsId, Type = 2, BarCodeNo = item.SerialNo, AssetNo = item.AssetNo, BinderName = "", IncidentNo = "" };
                lists.Add(list);
            }

           
            foreach (var item in ilsms)
            {
                SampleData list = new SampleData() { Id = item.TIlsmId, Type = 5, AssetNo = "", BarCodeNo = "", BinderName = "", IncidentNo = item.IncidentId };
                lists.Add(list);
            }

            return lists;
        }


        public  List<SampleData> GetILSM()
        {
            var lists = new List<SampleData>();
            var binders = _ilsmService.GetAllIlsm();
            foreach (var item in binders)
            {
                SampleData list = new SampleData() { Id = item.TIlsmId, Type = 5, AssetNo = "", BarCodeNo = "", BinderName="", IncidentNo = item.IncidentId };
                lists.Add(list);
            }
            return lists;
        }


        #endregion
    }
}
