//using HCF.BDO;
//using HCF.Utility;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Globalization;
//using System.Web.Script.Serialization;
//namespace HCF.BAL
//{
//    public static class FloorAssetRepository
//    {

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        /// <returns></returns>
//        public static TFloorAssets AddFloorAssets(TFloorAssets newItem)
//        {
//            if (newItem.Stop != null && newItem.StopId == 0)
//            {
//                if (!string.IsNullOrEmpty(newItem.Stop.StopName))
//                {
//                    var objstop = new StopMaster
//                    {
//                        StopCode = newItem.Stop.StopCode,
//                        StopName = newItem.Stop.StopName,
//                        FloorId = newItem.FloorId,
//                        CreatedBy = newItem.CreatedBy,
//                        IsActive = true
//                    };
//                    objstop.StopId = DAL.LocationRepository.SaveLocation(objstop);
//                    newItem.StopId = objstop.StopId;
//                }
//            }
//            newItem.FloorAssetsId = DAL.FloorAssetRepository.AddFloorAssets(newItem);

//            if (newItem.FloorAssetsId <= 0)
//                return newItem;

//            if (newItem.FloorAssetsInspection != null)
//            {
//                foreach (var data in newItem.FloorAssetsInspection)
//                {
//                    data.FloorAssetId = newItem.FloorAssetsId;
//                    data.InspectionDate = Conversion.ConvertToDateTime(data.InspectionDateTimeSpan);
//                    SaveFloorAssetsInspection(data);
//                }
//            }

//            //if (newItem.FloorId == null)
//            //    return newItem;

//            /// newItem.FloorAssetsId = floorassetId;
//            /// SaveLocation(newItem);
//            return newItem;
//        }
               
//        public static bool AddFloorAssetsToStop(TFloorAssets newfloorAssets)
//        {
//            return DAL.FloorAssetRepository.AddFloorAssetsToStop(newfloorAssets);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        //private static void SaveLocation(TFloorAssets newItem)
//        //{
//        //    if (newItem.FloorId != null)
//        //    {
//        //        List<FloorShapes> shapes = DAL.FloorShapesRepository.GetFloorShapes(floorId: newItem.FloorId.Value);
//        //        foreach (var item in shapes)
//        //        {
//        //            var serializer = new JavaScriptSerializer();
//        //            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

//        //            dynamic data = serializer.Deserialize(item.Data, typeof(object));
//        //            var lineList = data.lineList;
//        //            PointF point = new PointF(float.Parse(newItem.Xcoordinate, CultureInfo.InvariantCulture.NumberFormat),
//        //                float.Parse(newItem.Ycoordinate, CultureInfo.InvariantCulture.NumberFormat));

//        //            if (lineList.Count > 0)
//        //            {
//        //                PointF[] ptarray = new PointF[(int) lineList.Count];
//        //                for (int i = 0; i < lineList.Count; i++)
//        //                {
//        //                    ptarray[i] = new PointF(float.Parse((lineList[i].x).ToString(), CultureInfo.InvariantCulture.NumberFormat),
//        //                        float.Parse((lineList[i].y).ToString(), CultureInfo.InvariantCulture.NumberFormat));
//        //                }

//        //                bool status = IsPointInPolygon(ptarray, point);
//        //                if (status)
//        //                {
//        //                    TFloorAssetsShaps location = new TFloorAssetsShaps { FloorAssetsId = newItem.FloorAssetsId, ShapeId = item.Id };
//        //                    DAL.FloorShapesRepository.SaveTFloorAssetsShaps(location);
//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        public static void SaveLocation(FloorShapes newItem)
//        {
//            PointF[] ptarray = new PointF[1];
//            if (newItem.FloorId != null)
//            {
//                var tFloorAssets = DAL.FloorAssetRepository.GetFloorAssets(newItem.FloorId.Value);
//                var serializer = new JavaScriptSerializer();
//                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
//                dynamic data = serializer.Deserialize(newItem.Data, typeof(object));
//                var lineList = data.lineList;
//                if (lineList.Count > 0)
//                {
//                    ptarray = new PointF[(int)lineList.Count];
//                    for (int i = 0; i < lineList.Count; i++)
//                    {
//                        ptarray[i] = new PointF(float.Parse((lineList[i].x).ToString(), CultureInfo.InvariantCulture.NumberFormat),
//                            float.Parse((lineList[i].y).ToString(), CultureInfo.InvariantCulture.NumberFormat));
//                    }
//                }
//                foreach (var item in tFloorAssets)
//                {
//                    PointF point = new PointF(float.Parse(item.Xcoordinate, CultureInfo.InvariantCulture.NumberFormat),
//                        float.Parse(item.Ycoordinate, CultureInfo.InvariantCulture.NumberFormat));
//                    var status = IsPointInPolygon(ptarray, point);
//                    if (status)
//                    {
//                        TFloorAssetsShaps location = new TFloorAssetsShaps { FloorAssetsId = item.FloorAssetsId, ShapeId = newItem.Id };
//                        DAL.FloorShapesRepository.SaveTFloorAssetsShaps(location);
//                    }
//                }
//            }
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newItem"></param>
//        /// <returns></returns>
//        public static bool Movefloorassets(TFloorAssets newItem, string mode)
//        {
//            bool status = true;
//            TFloorAssetsLocations newLocation = new TFloorAssetsLocations();
//            // var status = DAL.FloorAssetRepository.Movefloorassets(newItem);
//            newLocation.Xcoordinate = newItem.Xcoordinate;
//            newLocation.Ycoordinate = newItem.Ycoordinate;
//            newLocation.ZoomLevel = newItem.ZoomLevel;
//            newLocation.FloorAssetsId = newItem.FloorAssetsId;
//            newLocation.FloorId = newItem.FloorId;
//            newLocation.CreatedBy = newItem.CreatedBy;
//            newLocation.AreaCode = newItem.AreaCode;
//            status = mode.ToLower() == "remove" ? DAL.LocationRepository.UpdateFloorAssetsLocation(newLocation) : DAL.LocationRepository.AddFloorAssetsLocation(newLocation);

//            //if (newItem != null && newItem.FloorAssetsId > 0)
//            //{
//            //    if (newItem.FloorId != null)
//            //        SaveLocation(newItem);
//            //}
//            return status;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newData"></param>
//        /// <returns></returns>
//        private static int SaveFloorAssetsInspection(FloorAssetsInspection newData)
//        {
//            return DAL.AssetsRepository.SaveFloorAssetsInspection(newData);
//        }

//        public static List<TFloorAssets> GetFloorAssets()
//        {
//            return DAL.FloorAssetRepository.GetFloorAssets();
//        }

//        public static List<TFloorAssets> GetAssetsByPrefix(string prefix)
//        {
//            return DAL.FloorAssetRepository.GetAssetsByPrefix(prefix);
//        }


//        public static List<TFloorAssets> GetTFloorAssets()
//        {
//            return DAL.FloorAssetRepository.GetTFloorAssets();
//        }

//        public static int UpdateInspectionDate(int floorAssetId, DateTime lastPmDate, string referenceNo)
//        {
//            return DAL.FloorAssetRepository.UpdateInspectionDate(floorAssetId, lastPmDate, referenceNo);

//        }

//        public static List<TFloorAssets> GetTFloorAssets(int assetId, int routId)
//        {
//            return DAL.FloorAssetRepository.GetTFloorAssets(assetId, routId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="lists"></param>
//        /// <param name="point"></param>
//        /// <returns></returns>
//        //public static bool IsPointInPolygon(List<PointF[]> lists, PointF point)
//        //{
//        //    bool isInside = false;
//        //    foreach (var list in lists)
//        //    {
//        //        PointF[] polygon = list;


//        //        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
//        //        {
//        //            if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
//        //            {
//        //                isInside = !isInside;
//        //            }
//        //        }
//        //    }
//        //    return isInside;
//        //}

//        public static TFloorAssets GetFeFloorAssetInspActivity(int floorAssetId, int userId)
//        {
//            return DAL.FloorAssetRepository.GetFeFloorAssetInspActivity(floorAssetId, userId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="polygon"></param>
//        /// <param name="point"></param>
//        /// <returns></returns>
//        private static bool IsPointInPolygon(PointF[] polygon, PointF point)
//        {
//            bool isInside = false;
//            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
//            {
//                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
//                {
//                    isInside = !isInside;
//                }
//            }

//            return isInside;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetId"></param>
//        /// <returns></returns>
//        public static TFloorAssets GetFloorAssetDescription(int floorAssetId)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetDescription(floorAssetId);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="floorAssetId"></param>
//        /// <returns></returns>
//        public static TFloorAssets GetFloorAsset(int floorAssetId)
//        {
//            return DAL.FloorAssetRepository.GetFloorAsset(floorAssetId);
//        }

//        public static TFloorAssets GetFloorAssetByTmsAssetId(int tmsAssetid)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetByTmsAssetId(tmsAssetid);
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="typeId"></param>
//        /// <returns></returns>
//        public static List<TFloorAssets> GetFloorAssetsByAssetType(int typeId, int userId)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetsByAssetType(typeId, userId, null);
//        }

//        public static List<TFloorAssets> GetFloorAssetsEps(int typeId,int floorAssetid, int userId)
//        {
//            return DAL.FloorAssetRepository.GetFloorAssetsEps(typeId, userId, floorAssetid);
//        }

//        public static List<Buildings> GetAssetsBuilding(int assetId)
//        {
//            return DAL.FloorAssetRepository.GetAssetsBuilding(assetId);
//        }

//        public static bool UpdateTfloorassetstatus(int floorId, int assetId, string statusCode)
//        {
//            return DAL.FloorAssetRepository.UpdateTfloorassetstatus(floorId, assetId, statusCode);
//        }

//        #region import asset from csv file 

//        /// <summary>
//        /// inserts/updates imported floor assets data, if the floor or building or stop is new , it will first import else update
//        /// </summary>
//        public static TFloorAssets AddImportedFloorAssets(TFloorAssets newItem)
//        {
//            try
//            {
//                if (newItem.Stop != null && newItem.StopId == 0)
//                {
//                    if (!string.IsNullOrEmpty(newItem.Stop.StopName))
//                    {
//                        StopMaster objstop = new StopMaster();
//                        objstop.StopCode = newItem.Stop.StopCode;
//                        objstop.StopName = newItem.Stop.StopName;
//                        objstop.FloorId = newItem.FloorId;
//                        objstop.CreatedBy = newItem.CreatedBy;
//                        objstop.IsActive = true;
//                        objstop.StopId = DAL.LocationRepository.SaveLocation(objstop);
//                        newItem.StopId = objstop.StopId;
//                    }
//                }
//                var ds = DAL.FloorAssetRepository.AddImportedFloorAssets(newItem);

//                var aasetSerializedDataset = JsonConvert.SerializeObject(ds);
//                newItem.AssetDataset = aasetSerializedDataset;

//                //HttpContext.Current.Session["AssetDataset"] = ds;
//                if (ds != null)
//                {
//                    var assetId = ds.Tables.Count > 3 ? Convert.ToString(ds.Tables[0].Rows[0].ItemArray[0]) : "0";
//                    newItem.FloorAssetsId = int.TryParse(assetId, out int id) ? int.Parse(assetId) : 0;
//                }

//                if (newItem.FloorAssetsId <= 0)
//                    return newItem;

//                if (newItem.FloorAssetsInspection != null)
//                {
//                    foreach (var data in newItem.FloorAssetsInspection)
//                    {
//                        data.FloorAssetId = newItem.FloorAssetsId;
//                        data.InspectionDate = Conversion.ConvertToDateTime(data.InspectionDateTimeSpan);
//                        SaveFloorAssetsInspection(data);
//                    }
//                }

//                //if (newItem.FloorId == null)
//                //    return newItem;

//                /// newItem.FloorAssetsId = floorassetId;
//                /// SaveLocation(newItem);
//            }
//            catch (Exception ex)
//            {
//                ErrorLog.LogError(ex);
//            }
//            return newItem;
//        }

//        #endregion


//        public static TFloorAssets GetAssetsOpenItems(int floorAssetId)
//        {
//            return DAL.FloorAssetRepository.GetAssetsOpenItems(floorAssetId);
//        }

        
//    }
//}
