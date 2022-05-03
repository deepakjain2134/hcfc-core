using System;
using System.Collections.Generic;
using System.Linq;
using TMS.HolyNameTmsService;

namespace TMS
{
    public static class TMSAssets
    {
        #region HolyName Assets Lists             

        public static List<HCF.BDO.AssetType> GetTmsAssets(int day)
        {
            DateTime createdDate = DateTime.Now;
            HolyNameTmsService.AssetCriteria1[] assetCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                assetCriteria = AssetCriteria(createdDate);
            }
            else
            {
                assetCriteria = new AssetCriteria1[1];
                AssetCriteria1 c1 = new AssetCriteria1();
                c1.fieldField = FIELDS5.DATE_CREATED;
                c1.fieldValueField = createdDate.AddDays(-4000);
                c1.operatorField = OPERATORS5.GREATER_OR_EQUAL;
                assetCriteria[0] = c1;
            }          

            HolyNameTmsService.HolyNameTmsClient client = new HolyNameTmsService.HolyNameTmsClient();
            HolyNameTmsService.Asset1[] holyNameTmseassets = client.GetAllAssets(assetCriteria);
            List<HCF.BDO.AssetType> assetTypes = holyNameTmseassets.GroupBy(i => i.categoryCodeField)
                .Select(g => new HCF.BDO.AssetType
                {
                    AssetTypeCode = g.Key,
                    Name = g.FirstOrDefault().categoryDescriptionField
                }).ToList();


            List<HCF.BDO.TFloorAssets> tfloorAssets = new List<HCF.BDO.TFloorAssets>();
            foreach (HolyNameTmsService.Asset1 asset in holyNameTmseassets)
            {
                HCF.BDO.TFloorAssets tfloorAsset = new HCF.BDO.TFloorAssets();
                tfloorAsset.DeviceNo = asset.assetNumberField;
                tfloorAsset.SerialNo = asset.serialNumberField;
                tfloorAsset.TmsReference = Convert.ToString(asset.assetIDField);
                //tfloorAsset.TmsAssetId = asset.assetIDField;
                tfloorAsset.AssetId = Convert.ToInt32(asset.subCategoryCodeField);
                tfloorAsset.BuildingCode = asset.buildingCodeField;
                tfloorAsset.LocationCode = asset.locationCodeField;
                tfloorAsset.Name = asset.descriptionField;
                tfloorAsset.NearBy = asset.locationDescriptionField;
                tfloorAsset.DateCreated = Convert.ToDateTime(asset.dateCreatedField);
                tfloorAsset.DateUpdated = Convert.ToDateTime(asset.dateUpdatedField);
                tfloorAsset.AssetCode = asset.subCategoryCodeField;
                tfloorAsset.Assets = new HCF.BDO.Assets()
                {
                    AssetType = new HCF.BDO.AssetType()
                    {
                        AssetTypeCode = asset.categoryCodeField,
                    }
                };
                tfloorAsset.StatusCode = asset.statusCodeField;
                tfloorAsset.BuildingCode = asset.buildingCodeField;
                tfloorAssets.Add(tfloorAsset);
            }
            foreach (HCF.BDO.AssetType assetType in assetTypes)
            {

                List<HCF.BDO.Assets> assets = holyNameTmseassets.Where(x => x.categoryCodeField == assetType.AssetTypeCode.ToString()).GroupBy(i => i.subCategoryCodeField)
                   .Select(g => new HCF.BDO.Assets
                   {
                       AssetCode = g.Key,
                       Name = g.FirstOrDefault().subCategoryDescriptionField
                   }).ToList();

                if (assets.Count > 0)
                {
                    assetType.Assets = assets;
                }

                foreach (HCF.BDO.Assets assetsfloor in assets)
                {
                    assetsfloor.TFloorAssets = new List<HCF.BDO.TFloorAssets>();
                    assetsfloor.TFloorAssets = tfloorAssets.Where(x => x.AssetCode == assetsfloor.AssetCode && x.Assets.AssetType.AssetTypeCode == assetType.AssetTypeCode).ToList();
                }
            }
            return assetTypes;
        }

        private static AssetCriteria1[] AssetCriteria(DateTime modifyDate)
        {
            HolyNameTmsService.AssetCriteria1[] assetCriteria = new AssetCriteria1[2];
            HolyNameTmsService.AssetCriteria1 c1 = new AssetCriteria1();
            c1.fieldField = FIELDS5.DATE_CREATED;
            c1.fieldValueField = modifyDate;
            c1.operatorField = OPERATORS5.GREATER_OR_EQUAL;
            c1.logicalOperatorField = LOGICAL_OPERATORS5.OR;
            assetCriteria[0] = c1;
           
            HolyNameTmsService.AssetCriteria1 c2 = new AssetCriteria1();
            c2.fieldField = FIELDS5.DATE_UPDATED;
            c2.fieldValueField = modifyDate;
            c2.operatorField = OPERATORS5.GREATER_OR_EQUAL;
            c1.logicalOperatorField = LOGICAL_OPERATORS5.OR;
            assetCriteria[1] = c2;
            return assetCriteria;
        }

        #endregion

        #region Burke Assets Lists 




        public static List<HCF.BDO.AssetType> GetBurkeTmsAssets(int day)
        {
            DateTime createdDate = DateTime.Now;
            AssetCriteria[] assetCriteria;
            if (day > 0)
            {
                createdDate = createdDate.AddDays(-day);
                assetCriteria = AssetBurkeCriteria(createdDate);
            }
            else
            {
                assetCriteria = new AssetCriteria[1];
                AssetCriteria c1 = new AssetCriteria();
                c1.fieldField = FIELDS2.DATE_CREATED;
                c1.fieldValueField = createdDate;
                c1.operatorField = OPERATORS2.LESS_OR_EQUAL;
                assetCriteria[0] = c1;
            }

            BurkeAssetsClient client = new BurkeAssetsClient();
            Asset[] holyNameTmseassets = client.GetBurkeAllAssets(assetCriteria);
            List<HCF.BDO.AssetType> assetTypes = holyNameTmseassets.GroupBy(i => i.categoryCodeField)
                .Select(g => new HCF.BDO.AssetType
                {
                    AssetTypeCode = g.Key,
                    Name = g.FirstOrDefault().categoryDescriptionField
                }).ToList();


            List<HCF.BDO.TFloorAssets> tfloorAssets = new List<HCF.BDO.TFloorAssets>();
            foreach (var asset in holyNameTmseassets)
            {
                HCF.BDO.TFloorAssets tfloorAsset = new HCF.BDO.TFloorAssets();
                tfloorAsset.DeviceNo = asset.assetNumberField;
                tfloorAsset.SerialNo = asset.serialNumberField;
                tfloorAsset.TmsReference = Convert.ToString(asset.assetIDField);
                //tfloorAsset.TmsAssetId = asset.assetIDField;
                //tfloorAsset.AssetId = Convert.ToInt32(asset.subCategoryCodeField);
                tfloorAsset.BuildingCode = asset.buildingCodeField;
                tfloorAsset.LocationCode = asset.locationCodeField;
                tfloorAsset.Name = asset.descriptionField;
                tfloorAsset.NearBy = asset.locationDescriptionField;
                tfloorAsset.DateCreated = Convert.ToDateTime(asset.dateCreatedField);
                tfloorAsset.DateUpdated = Convert.ToDateTime(asset.dateUpdatedField);
                tfloorAsset.AssetCode = asset.subCategoryCodeField;
                tfloorAsset.Assets = new HCF.BDO.Assets()
                {
                    AssetType = new HCF.BDO.AssetType()
                    {
                        AssetTypeCode = asset.categoryCodeField,
                    }
                };                
                tfloorAsset.StatusCode = asset.statusCodeField;
                tfloorAsset.BuildingCode = asset.buildingCodeField;
                tfloorAssets.Add(tfloorAsset);
            }
            foreach (HCF.BDO.AssetType assetType in assetTypes)
            {

                List<HCF.BDO.Assets> assets = holyNameTmseassets.Where(x => x.categoryCodeField == assetType.AssetTypeCode.ToString()).GroupBy(i => i.subCategoryCodeField)
                   .Select(g => new HCF.BDO.Assets
                   {
                       AssetCode = g.Key,                      
                       Name = g.FirstOrDefault().subCategoryDescriptionField
                   }).ToList();

                if (assets.Count > 0)
                {
                    assetType.Assets = assets;
                }

                foreach (HCF.BDO.Assets assetsfloor in assetType.Assets)
                {
                    assetsfloor.TFloorAssets = new List<HCF.BDO.TFloorAssets>();
                    assetsfloor.TFloorAssets = tfloorAssets.Where(x => x.AssetCode == assetsfloor.AssetCode && x.Assets.AssetType.AssetTypeCode == assetType.AssetTypeCode).ToList();
                }
            }         

            return assetTypes;

        }


        private static AssetCriteria[] AssetBurkeCriteria(DateTime modifyDate)
        {
            HolyNameTmsService.AssetCriteria[] assetCriteria = new AssetCriteria[2];
            HolyNameTmsService.AssetCriteria c1 = new AssetCriteria();
            c1.fieldField = FIELDS2.DATE_CREATED;
            c1.fieldValueField = modifyDate;
            c1.operatorField = OPERATORS2.GREATER_OR_EQUAL;
            c1.logicalOperatorField = LOGICAL_OPERATORS2.OR;
            assetCriteria[0] = c1;

            HolyNameTmsService.AssetCriteria c2 = new AssetCriteria();
            c2.fieldField = FIELDS2.DATE_UPDATED;
            c2.fieldValueField = modifyDate;
            c2.operatorField = OPERATORS2.GREATER_OR_EQUAL;
            c1.logicalOperatorField = LOGICAL_OPERATORS2.OR;
            assetCriteria[1] = c2;

            //HolyNameTmsService.AssetCriteria c3 = new AssetCriteria();
            //c3.fieldField = FIELDS3.CATEGORY_CODE;
            //c3.fieldValueField = "MOTOR";
            //c3.operatorField = OPERATORS3.EQUAL;
            //c3.logicalOperatorField = LOGICAL_OPERATORS3.AND;
            //assetCriteria[0] = c3;
            return assetCriteria;
        }

        #endregion

        #region Burke Assets WorkOrders

        public static List<HCF.BDO.WorkOrder> GetBurkeAssetsWO(int assetId)
        {
            List<HCF.BDO.WorkOrder> workOrders = new List<HCF.BDO.WorkOrder>();
            try
            {
                BurkeAssetsClient client = new BurkeAssetsClient();
                WorkOrder[] assetsWorkOrders = client.GetBurkeAssetWorkOrders(assetId).OrderByDescending(x => x.dateCreatedField).Take(1).ToArray();
                foreach (var item in assetsWorkOrders)
                {
                    HCF.BDO.WorkOrder wo = new HCF.BDO.WorkOrder();
                    wo.DateCreated = item.dateCreatedField;
                    wo.WorkOrderId = item.workOrderIDField;
                    wo.WorkOrderNumber = item.workOrderNumberField.ToString();
                    workOrders.Add(wo);
                }
            }
            catch (Exception ex)
            {
                HCF.Utility.ErrorLog.LogMsg(string.Format("TMS -> Error While updating first Inspection Date of assets {0} -> {1}", assetId.ToString(), ex.Message));
            }
            return workOrders;
        }

        #endregion
    }
}
