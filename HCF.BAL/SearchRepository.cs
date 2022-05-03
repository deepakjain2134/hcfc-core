//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HCF.BDO;

//namespace HCF.BAL
//{
//    public static class SearchRepository
//    {
//        #region Search filter

//        public static int Save(SearchFilter objSearchFilter)
//        {
//            return DAL.SearchRepository.AddSearchFilter(objSearchFilter);
//        }

//        public static List<SearchFilter> GetSearchFilter(int userId)
//        {
//            return DAL.SearchRepository.GetSearchFilter(userId);
//        }


//        public static SearchFilter GetSearchFilter(Guid filterId)
//        {
//            return DAL.SearchRepository.GetSearchFilter(filterId);
//        }

//        #endregion

//        #region Search Result

//        public static SearchFilter GetSearchResult(SearchFilter objSearchFilter)
//        {
//            return DAL.SearchRepository.GetSearchResult(objSearchFilter);
//        }

//        #endregion


//        #region Lucene

//        public static SampleData Get(int id)
//        {
//            return GetAll().SingleOrDefault(x => x.Id.Equals(id));
//        }
//        public static List<SampleData> GetAll()
//        {
//            var lists = new List<SampleData>();
          
//            var binders = DocumentsRepository.Get();
//            var tfloorAssets = FloorAssetRepository.GetFloorAssets();
//            var ilsms = IlsmRepository.GetAllIlsm();
//            foreach (var item in binders)
//            {
//                SampleData list = new SampleData() { Id = item.BinderId, Type = 4,  AssetNo = "", BarCodeNo = "", BinderName = item.BinderName, IncidentNo = "" };
//                lists.Add(list);
//            }

//            foreach (var item in tfloorAssets)
//            {
//                SampleData list = new SampleData() { Id = item.FloorAssetsId, Type = 2, BarCodeNo = item.SerialNo, AssetNo = item.AssetNo, BinderName = "", IncidentNo = "" };
//                lists.Add(list);
//            }

           
//            foreach (var item in ilsms)
//            {
//                SampleData list = new SampleData() { Id = item.TIlsmId, Type = 5, AssetNo = "", BarCodeNo = "", BinderName = "", IncidentNo = item.IncidentId };
//                lists.Add(list);
//            }

//            return lists;
//        }


//        public static List<SampleData> GetILSM()
//        {
//            var lists = new List<SampleData>();
//            var binders = IlsmRepository.GetAllIlsm();
//            foreach (var item in binders)
//            {
//                SampleData list = new SampleData() { Id = item.TIlsmId, Type = 5, AssetNo = "", BarCodeNo = "", BinderName="", IncidentNo = item.IncidentId };
//                lists.Add(list);
//            }
//            return lists;
//        }


//        #endregion
//    }
//}
