using HCF.BDO.MaintenanceConnection;
using HCF.BDO;
using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using HCF.BDO.NBIH;

namespace HCF.DAL
{
    public class TmsConnectorRepsitory : ITmsConnectorRepsitory
    {
        private readonly ISqlHelper _sqlHelper;

        public TmsConnectorRepsitory(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public int AddTMSRecords(TMS_Results newData)
        {
            int newId = 0;
            const string sql = StoredProcedures.TMSAkron_InsertResults;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PK", newData.PK);
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Name", newData.Name);
                command.Parameters.AddWithValue("@IsLocation", newData.IsLocation);
                command.Parameters.AddWithValue("@IsUp", newData.IsUp);
                command.Parameters.AddWithValue("@PurchaseCost", newData.PurchaseCost);
                command.Parameters.AddWithValue("@CurrentValue", newData.CurrentValue);
                command.Parameters.AddWithValue("@ReplacementCost", newData.ReplacementCost);
                command.Parameters.AddWithValue("@InstructionsToWO", newData.InstructionsToWO);
                command.Parameters.AddWithValue("@IsMeter", newData.IsMeter);
                command.Parameters.AddWithValue("@Meter1Reading", newData.Meter1Reading);
                command.Parameters.AddWithValue("@Meter2Reading", newData.Meter2Reading);
                command.Parameters.AddWithValue("@InternalAssetNum", newData.InternalAssetNum);
                command.Parameters.AddWithValue("@Model", newData.Model);
                command.Parameters.AddWithValue("@Serial", newData.Serial);
                command.Parameters.AddWithValue("@Vicinity", newData.Vicinity);
                command.Parameters.AddWithValue("@GenLedger", newData.GenLedger);
                command.Parameters.AddWithValue("@WarrantyExpire", newData.WarrantyExpire);
                command.Parameters.AddWithValue("@Address", newData.Address);
                command.Parameters.AddWithValue("@City", newData.City);
                command.Parameters.AddWithValue("@State", newData.State);
                command.Parameters.AddWithValue("@Zip", newData.Zip);
                command.Parameters.AddWithValue("@Country", newData.Country);
                command.Parameters.AddWithValue("@PurchasedDate", newData.PurchasedDate);
                command.Parameters.AddWithValue("@PurchaseOrder", newData.PurchaseOrder);
                command.Parameters.AddWithValue("@InstallDate", newData.InstallDate);
                command.Parameters.AddWithValue("@StartupDate", newData.StartupDate);
                command.Parameters.AddWithValue("@ReplaceDate", newData.ReplaceDate);
                command.Parameters.AddWithValue("@Life", newData.Life);
                command.Parameters.AddWithValue("@DisposalDate", newData.DisposalDate);
                command.Parameters.AddWithValue("@LicenseNumber", newData.LicenseNumber);
                command.Parameters.AddWithValue("@Instructions", newData.Instructions);
                command.Parameters.AddWithValue("@InsuranceCarrier", newData.InsuranceCarrier);
                command.Parameters.AddWithValue("@InsurancePolicy", newData.InsurancePolicy);
                command.Parameters.AddWithValue("@LeaseNumber", newData.LeaseNumber);
                command.Parameters.AddWithValue("@RegistrationDate", newData.RegistrationDate);
                command.Parameters.AddWithValue("@xcoordinate", newData.xcoordinate);
                command.Parameters.AddWithValue("@ycoordinate", newData.ycoordinate);
                command.Parameters.AddWithValue("@zcoordinate", newData.zcoordinate);
                command.Parameters.AddWithValue("@parcelid", newData.parcelid);
                command.Parameters.AddWithValue("@Technology", newData.Technology);
                command.Parameters.AddWithValue("@TechnologyDesc", newData.TechnologyDesc);
                command.Parameters.AddWithValue("@Meter1TrackHistory", newData.Meter1TrackHistory);
                command.Parameters.AddWithValue("@Meter2TrackHistory", newData.Meter2TrackHistory);
                command.Parameters.AddWithValue("@LastMaintained", newData.LastMaintained);
                command.Parameters.AddWithValue("@LastMaintained_WOPK", newData.LastMaintained_WOPK);
                command.Parameters.AddWithValue("@Icon", newData.Icon);
                command.Parameters.AddWithValue("@Photo", newData.Photo);
                command.Parameters.AddWithValue("@DrawingUpdatesNeeded", newData.DrawingUpdatesNeeded);
                command.Parameters.AddWithValue("@DisplayOrder", newData.DisplayOrder);
                command.Parameters.AddWithValue("@RiskAssessmentRequired", newData.RiskAssessmentRequired);
                command.Parameters.AddWithValue("@LastAssessed", newData.LastAssessed);
                command.Parameters.AddWithValue("@PMRequired", newData.PMRequired);
                command.Parameters.AddWithValue("@PlanForImprovement", newData.PlanForImprovement);
                command.Parameters.AddWithValue("@HIPPARelated", newData.HIPPARelated);
                command.Parameters.AddWithValue("@StatementOfConditions", newData.StatementOfConditions);
                command.Parameters.AddWithValue("@StatementOfConditionsCompliant", newData.StatementOfConditionsCompliant);
                command.Parameters.AddWithValue("@County", newData.County);
                command.Parameters.AddWithValue("@YearBuilt", newData.YearBuilt);
                command.Parameters.AddWithValue("@MajorRenovations", newData.MajorRenovations);
                command.Parameters.AddWithValue("@SquareFootage", newData.SquareFootage);
                command.Parameters.AddWithValue("@NumberOfStories", newData.NumberOfStories);
                command.Parameters.AddWithValue("@FloodZone", newData.FloodZone);
                command.Parameters.AddWithValue("@QuakeZone", newData.QuakeZone);
                command.Parameters.AddWithValue("@Ext100Feet", newData.Ext100Feet);
                command.Parameters.AddWithValue("@OperatingUnits", newData.OperatingUnits);
                command.Parameters.AddWithValue("@EstimatedValue", newData.EstimatedValue);
                command.Parameters.AddWithValue("@Meter1RollDown", newData.Meter1RollDown);
                command.Parameters.AddWithValue("@Meter2RollDown", newData.Meter2RollDown);
                command.Parameters.AddWithValue("@Meter1RollDownMethod", newData.Meter1RollDownMethod);
                command.Parameters.AddWithValue("@Meter2RollDownMethod", newData.Meter2RollDownMethod);
                command.Parameters.AddWithValue("@Meter1ReadingLife", newData.Meter1ReadingLife);
                command.Parameters.AddWithValue("@Meter2ReadingLife", newData.Meter2ReadingLife);
                command.Parameters.AddWithValue("@IsLinear", newData.IsLinear);
                command.Parameters.AddWithValue("@PMCycleStartDate", newData.ID);
                command.Parameters.AddWithValue("@PMCounter", newData.PMCounter);
                command.Parameters.AddWithValue("@ModelNumber", newData.ModelNumber);
                command.Parameters.AddWithValue("@ModelNumberMFG", newData.ModelNumberMFG);
                command.Parameters.AddWithValue("@ResourceForScheduling", newData.ResourceForScheduling);
                command.Parameters.AddWithValue("@RotatingRepairCenterPK", newData.RotatingRepairCenterPK);
                command.Parameters.AddWithValue("@RotatingLocationPK", newData.RotatingLocationPK);
                command.Parameters.AddWithValue("@RotatingLaborPK", newData.RotatingLaborPK);
                command.Parameters.AddWithValue("@RotatingBin", newData.RotatingBin);
                command.Parameters.AddWithValue("@GISLayer", newData.GISLayer);
                command.Parameters.AddWithValue("@GISKeyValue", newData.GISKeyValue);
                command.Parameters.AddWithValue("@GISTable", newData.GISTable);
                command.Parameters.AddWithValue("@ReceiveDate", newData.ReceiveDate);
                command.Parameters.AddWithValue("@Latitude", newData.Latitude);
                command.Parameters.AddWithValue("@Longitude", newData.Longitude);
                command.Parameters.AddWithValue("@UDFChar1", newData.UDFChar1);
                command.Parameters.AddWithValue("@UDFChar2", newData.UDFChar2);
                command.Parameters.AddWithValue("@UDFChar3", newData.UDFChar3);
                command.Parameters.AddWithValue("@UDFChar4", newData.UDFChar4);
                command.Parameters.AddWithValue("@UDFChar5", newData.UDFChar5);
                command.Parameters.AddWithValue("@UDFChar6", newData.UDFChar6);
                command.Parameters.AddWithValue("@UDFChar7", newData.UDFChar7);
                command.Parameters.AddWithValue("@UDFChar8", newData.UDFChar8);
                command.Parameters.AddWithValue("@UDFChar9", newData.UDFChar9);
                command.Parameters.AddWithValue("@UDFChar10", newData.UDFChar10);
                command.Parameters.AddWithValue("@UDFDate1", newData.UDFDate1);
                command.Parameters.AddWithValue("@UDFDate2", newData.UDFDate2);
                command.Parameters.AddWithValue("@UDFDate3", newData.UDFDate3);
                command.Parameters.AddWithValue("@UDFDate4", newData.UDFDate4);
                command.Parameters.AddWithValue("@UDFDate5", newData.UDFDate5);
                command.Parameters.AddWithValue("@UDFBit1", newData.UDFBit1);
                command.Parameters.AddWithValue("@UDFBit2", newData.UDFBit2);
                command.Parameters.AddWithValue("@UDFBit3", newData.UDFBit3);
                command.Parameters.AddWithValue("@UDFBit4", newData.UDFBit4);
                command.Parameters.AddWithValue("@UDFBit5", newData.UDFBit5);
                command.Parameters.AddWithValue("@RowVersionUserPK", newData.RowVersionUserPK);
                command.Parameters.AddWithValue("@RowVersionAction", newData.RowVersionAction);
                command.Parameters.AddWithValue("@LastModifiedDate", newData.LastModifiedDate);
                command.Parameters.AddWithValue("@ResourceUri", newData.ResourceUri);
                command.Parameters.AddWithValue("@AssetUUID", newData.AssetUUID);
                command.Parameters.AddWithValue("@AssetLevel", newData.AssetLevel);
                command.Parameters.AddWithValue("@ParentRefPK", (newData.ParentRef != null) ? newData.ParentRef.PK : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ParentRefID", (newData.ParentRef != null) ? newData.ParentRef.ID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ParentRefName", (newData.ParentRef != null) ? newData.ParentRef.Name : (object)DBNull.Value);

                command.Parameters.AddWithValue("@ClassificationRefPK", (newData.ClassificationRef != null) ? newData.ClassificationRef.PK : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ClassificationRefID", (newData.ClassificationRef != null) ? newData.ClassificationRef.ID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ClassificationRefName", (newData.ClassificationRef != null) ? newData.ClassificationRef.Name : (object)DBNull.Value);

                //command.Parameters.AddWithValue("@OperatorRef", newData.OperatorRef);

                command.Parameters.AddWithValue("@DepartmentRefPK", (newData.DepartmentRef != null) ? newData.DepartmentRef.PK : (object)DBNull.Value);
                command.Parameters.AddWithValue("@DepartmentRefID", (newData.DepartmentRef != null) ? newData.DepartmentRef.ID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@DepartmentRefName", (newData.DepartmentRef != null) ? newData.DepartmentRef.Name : (object)DBNull.Value);

                //command.Parameters.AddWithValue("@CustomerRef", newData.CustomerRef);

                command.Parameters.AddWithValue("@AccountRefPK", (newData.AccountRef != null) ? newData.AccountRef.PK : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AccountRefID", (newData.AccountRef != null) ? newData.AccountRef.ID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AccountRefName", (newData.AccountRef != null) ? newData.AccountRef.Name : (object)DBNull.Value);

                command.Parameters.AddWithValue("@RepairCenterRefPK", (newData.RepairCenterRef != null) ? newData.RepairCenterRef.PK : (object)DBNull.Value);
                command.Parameters.AddWithValue("@RepairCenterRefID", (newData.RepairCenterRef != null) ? newData.RepairCenterRef.ID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@RepairCenterRefName", (newData.RepairCenterRef != null) ? newData.RepairCenterRef.Name : (object)DBNull.Value);

                //command.Parameters.AddWithValue("@ShopRef", newData.ShopRef);
                //command.Parameters.AddWithValue("@VendorRef", newData.VendorRef);
                //command.Parameters.AddWithValue("@ManufacturerRef", newData.ManufacturerRef);
                //command.Parameters.AddWithValue("@ResponsibilitySafetyRef", newData.ResponsibilitySafetyRef);
                //command.Parameters.AddWithValue("@ResponsibilityRepairRef", newData.ResponsibilityRepairRef);
                //command.Parameters.AddWithValue("@ResponsibilityPMRef", newData.ResponsibilityPMRef);
                //command.Parameters.AddWithValue("@ServiceRepairRef", newData.ServiceRepairRef);
                //command.Parameters.AddWithValue("@ServicePMRef", newData.ServicePMRef);
                //command.Parameters.AddWithValue("@ContactRef", newData.ContactRef);
                //command.Parameters.AddWithValue("@ZoneRef", newData.ZoneRef);
                //command.Parameters.AddWithValue("@StatusDetails", newData.StatusDetails);

                command.Parameters.AddWithValue("@TypeDetailsValue", (newData.TypeDetails != null) ? newData.TypeDetails.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TypeDetailsDescription", (newData.TypeDetails != null) ? newData.TypeDetails.Description : (object)DBNull.Value);


                //command.Parameters.AddWithValue("@SystemDetails", newData.SystemDetails);
                //command.Parameters.AddWithValue("@PriorityDetails", newData.PriorityDetails);
                //command.Parameters.AddWithValue("@TechnologyDetails", newData.TechnologyDetails);
                //command.Parameters.AddWithValue("@Meter1UnitsDetails", newData.Meter1UnitsDetails);
                //command.Parameters.AddWithValue("@Meter2UnitsDetails", newData.Meter2UnitsDetails);
                //command.Parameters.AddWithValue("@PurchaseTypeDetails", newData.PurchaseTypeDetails);

                command.Parameters.AddWithValue("@ClassIndustryDetailsValue", (newData.ClassIndustryDetails != null) ? newData.ClassIndustryDetails.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ClassIndustryDetailsDescription", (newData.ClassIndustryDetails != null) ? newData.ClassIndustryDetails.Description : (object)DBNull.Value);
                command.Parameters.AddWithValue("@RiskAssessmentGroupDetailsValue", (newData.RiskAssessmentGroupDetails != null) ? newData.RiskAssessmentGroupDetails.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@RiskAssessmentGroupDetailsDescription", (newData.RiskAssessmentGroupDetails != null) ? newData.RiskAssessmentGroupDetails.Description : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@ConstructionCodeDetails", newData.ConstructionCodeDetails);
                //command.Parameters.AddWithValue("@ISOProtectionDetails", newData.ISOProtectionDetails);
                //command.Parameters.AddWithValue("@AutoSprinklerDetails", newData.AutoSprinklerDetails);
                // command.Parameters.AddWithValue("@SmokeAlarmDetails", newData.SmokeAlarmDetails);
                //command.Parameters.AddWithValue("@HeatAlarmDetails", newData.HeatAlarmDetails);
                command.Parameters.AddWithValue("@PMCycleStartByDetailsValue", (newData.PMCycleStartByDetails != null) ? newData.PMCycleStartByDetails.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@PMCycleStartByDetailsDescription", (newData.PMCycleStartByDetails != null) ? newData.PMCycleStartByDetails.Description : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@ModelLineDetails", newData.ModelLineDetails);
                //command.Parameters.AddWithValue("@ModelSeriesDetails", newData.ModelSeriesDetails);
                //command.Parameters.AddWithValue("@SystemPlatformDetails", newData.SystemPlatformDetails);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");    
            }
            return newId;
        }

        public List<TMS_Results> GetAllTMSRecords()
        {
            List<TMS_Results> lists = new List<TMS_Results>();
            const string sql = StoredProcedures.TMS_GetAllTMSRecords;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<TMS_Results>(ds.Tables[0]);
            }
            return lists;
        }

        public List<TFloorAssets> GetAllTMSAsset()
        {
            List<TFloorAssets> lists = new List<TFloorAssets>();
            const string sql = StoredProcedures.TMSAkron_GetAllTMSAsset;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<TFloorAssets>(ds.Tables[0]);
            }
            return lists;
        }

        public int SaveAssets(TFloorAssets newfloorAssets)
        {
            int newId;
            const string sql = StoredProcedures.TMSAkron_InsertAssets;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorId", newfloorAssets.FloorId);
                command.Parameters.AddWithValue("@AssetId", newfloorAssets.AssetId);
                command.Parameters.AddWithValue("@Name", newfloorAssets.Name);
                command.Parameters.AddWithValue("@CreatedBy", newfloorAssets.CreatedBy);
                command.Parameters.AddWithValue("@SerialNo", newfloorAssets.SerialNo);
                command.Parameters.AddWithValue("@Xcoordinate", newfloorAssets.Xcoordinate);
                command.Parameters.AddWithValue("@Ycoordinate", newfloorAssets.Ycoordinate);
                command.Parameters.AddWithValue("@AreaCode", newfloorAssets.AreaCode);
                command.Parameters.AddWithValue("@ZoomLevel", newfloorAssets.ZoomLevel);
                command.Parameters.AddWithValue("@Notes", newfloorAssets.Notes);
                command.Parameters.AddWithValue("@DeviceNo", newfloorAssets.DeviceNo);//assets #
                command.Parameters.AddWithValue("@NearBy", newfloorAssets.NearBy);
                command.Parameters.AddWithValue("@DateCreated", newfloorAssets.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", newfloorAssets.DateUpdated);
                command.Parameters.AddWithValue("@LocationCode", newfloorAssets.LocationCode);//assets #
                command.Parameters.AddWithValue("@BuildingCode", newfloorAssets.BuildingCode);
                command.Parameters.AddWithValue("@description", newfloorAssets.Description);
                command.Parameters.AddWithValue("@StatusCode", !string.IsNullOrEmpty(newfloorAssets.StatusCode) ? newfloorAssets.StatusCode : BDO.Enums.StatusCode.ACTIV.ToString());
                command.Parameters.AddWithValue("@TmsReference", newfloorAssets.TmsReference);
                command.Parameters.AddWithValue("@ManufactureId", newfloorAssets.ManufactureId);
                command.Parameters.AddWithValue("@AscId", newfloorAssets.AscId);
                command.Parameters.AddWithValue("@StopId", newfloorAssets.StopId);
                command.Parameters.AddWithValue("@FETypeId", newfloorAssets.FETypeId);
                command.Parameters.AddWithValue("@ModelNo", newfloorAssets.Model);
                command.Parameters.AddWithValue("@Rating", newfloorAssets.Rating);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        #region WorkOrder

        public int SaveTMSWorkOrderToLocal(TMS_WorkOrderRes data)
        {
            int newId = 0;
            const string sql = StoredProcedures.TMSAkron_InsertWorkORders;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PK", data.PK);
                command.Parameters.AddWithValue("@ID", data.ID);
                command.Parameters.AddWithValue("@IsOpen", data.IsOpen);
                command.Parameters.AddWithValue("@LaborReport", data.LaborReport);
                command.Parameters.AddWithValue("@TargetDate", data.TargetDate);
                command.Parameters.AddWithValue("@Reason", data.Reason);
                command.Parameters.AddWithValue("@RequesterEmail", data.RequesterEmail);
                command.Parameters.AddWithValue("@RequesterName", data.RequesterName);
                command.Parameters.AddWithValue("@RequesterPhone", data.RequesterPhone);
                command.Parameters.AddWithValue("@StatusDetailsValue", data.StatusDetails.Value);
                command.Parameters.AddWithValue("@LastModifiedDate", data.LastModifiedDate);
                command.Parameters.AddWithValue("@PriorityDetailsValue", data.PriorityDetails.Value);
                command.Parameters.AddWithValue("@TypeDetailsValue", data.TypeDetails.Value);
                if (data.RepairCenterRef != null)
                {
                    command.Parameters.AddWithValue("@RepairCenterRefPK", data.RepairCenterRef.PK);
                    command.Parameters.AddWithValue("@RepairCenterRefID", data.RepairCenterRef.ID);
                    command.Parameters.AddWithValue("@RepairCenterRefName", data.RepairCenterRef.Name);
                }
                if (data.AssetRef != null)
                {
                    command.Parameters.AddWithValue("@AssetRefPK", data.AssetRef.PK);
                    command.Parameters.AddWithValue("@AssetRefID", data.AssetRef.ID);
                    command.Parameters.AddWithValue("@AssetRefName", data.AssetRef.Name);
                }
                command.Parameters.AddWithValue("@RequestedDate", data.RequestedDate);
                command.Parameters.AddWithValue("@CanceledDate", data.CanceledDate);
                command.Parameters.AddWithValue("@IssuedDate", data.IssuedDate);
                command.Parameters.AddWithValue("@StatusDate", data.StatusDate);
                command.Parameters.AddWithValue("@CompleteDate", data.CompleteDate);
                command.Parameters.AddWithValue("@IsAssigned", data.IsAssigned);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;

        }

        public int SaveWorkOrderAssignmentToLocal(TMS_WorkOrderAssignment data)
        {
            int newId = 0;
            const string sql = StoredProcedures.TMS_MC_WorkOrderAssignment;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PK", data.PK);
                command.Parameters.AddWithValue("@WorkOrderPK", data.WorkOrderPK);
                command.Parameters.AddWithValue("@IsAssigned", data.IsAssigned);
                command.Parameters.AddWithValue("@AssignedDate", data.AssignedDate);
                command.Parameters.AddWithValue("@AssignedHours", data.AssignedHours);
                command.Parameters.AddWithValue("@LaborID", data.LaborRef.ID);
                command.Parameters.AddWithValue("@LaborPK", data.LaborRef.PK);
                command.Parameters.AddWithValue("@LaborName", data.LaborRef.Name);
                command.Parameters.AddWithValue("@IsAssignedLead", data.IsAssignedLead);
                command.Parameters.AddWithValue("@IsAssignedPDA", data.IsAssignedPDA);
                command.Parameters.AddWithValue("@WOlaborPK", data.WOlaborPK);
                command.Parameters.AddWithValue("@CompletePercent", data.CompletePercent);
                command.Parameters.AddWithValue("@CompletedDate", data.CompletedDate);
                command.Parameters.AddWithValue("@IsActive", data.IsActive);
                command.Parameters.AddWithValue("@IsAnytime", data.IsAnytime);
                command.Parameters.AddWithValue("@ResourceUri", data.ResourceUri);
                command.Parameters.AddWithValue("@LastModifiedDate", data.LastModifiedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;

        }

        public int SaveWorkOrderTasksToLocal(TMS_WorkOrderTasks data)
        {
            int newId = 0;
            const string sql = StoredProcedures.TMS_MC_InsertWorkOrderTask;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PK", data.PK);
                command.Parameters.AddWithValue("@WorkOrderPK", data.WorkOrderPK);
                command.Parameters.AddWithValue("@TaskNo", data.TaskNo);
                command.Parameters.AddWithValue("@TaskAction", data.TaskAction);
                command.Parameters.AddWithValue("@IsHeader", data.IsHeader);
                command.Parameters.AddWithValue("@MeasurementInitial", data.MeasurementInitial);
                command.Parameters.AddWithValue("@Measurement", data.Measurement);
                command.Parameters.AddWithValue("@HoursEstimated", data.HoursEstimated);
                command.Parameters.AddWithValue("@HoursActual", data.HoursActual);
                command.Parameters.AddWithValue("@Rate", data.Rate);
                command.Parameters.AddWithValue("@TaskPK", data.TaskPK);
                command.Parameters.AddWithValue("@CraftPK", data.CraftPK);
                command.Parameters.AddWithValue("@AssetSpecificationPK", data.AssetSpecificationPK);
                command.Parameters.AddWithValue("@SpecificationPK", data.SpecificationPK);
                command.Parameters.AddWithValue("@AssetPK", data.AssetPK);
                command.Parameters.AddWithValue("@ClassificationPK", data.ClassificationPK);
                command.Parameters.AddWithValue("@ToolPK", data.ToolPK);
                command.Parameters.AddWithValue("@IsComplete", data.IsComplete);
                command.Parameters.AddWithValue("@IsFailed", data.IsFailed);
                command.Parameters.AddWithValue("@IsSpec", data.IsSpec);
                command.Parameters.AddWithValue("@IsMeter1", data.IsMeter1);
                command.Parameters.AddWithValue("@IsMeter2", data.IsMeter2);
                command.Parameters.AddWithValue("@NotApplicable", data.NotApplicable);
                command.Parameters.AddWithValue("@Comments", data.Comments);
                command.Parameters.AddWithValue("@Initials", data.Initials);
                command.Parameters.AddWithValue("@Photo1", data.Photo1);
                command.Parameters.AddWithValue("@DeletePhoto1", data.DeletePhoto1);
                command.Parameters.AddWithValue("@Photo2", data.Photo2);
                command.Parameters.AddWithValue("@DeletePhoto2", data.DeletePhoto2);
                command.Parameters.AddWithValue("@FollowupWorkOrderPK", data.FollowupWorkOrderPK);
                command.Parameters.AddWithValue("@LastModifiedDate", data.LastModifiedDate);
                command.Parameters.AddWithValue("@ResourceUri", data.ResourceUri);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;

        }

        #endregion

        #region Users

        public int SaveTMSUsersToLocal(TMS_UserRes data)
        {
            int newId = 0;
            const string sql = StoredProcedures.TMS_MC_InsertUsers;
            using (SqlCommand command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PK", data.PK);
                command.Parameters.AddWithValue("@ID", data.ID);
                command.Parameters.AddWithValue("@FirstName", data.FirstName);
                command.Parameters.AddWithValue("@LastName", data.LastName);
                command.Parameters.AddWithValue("@MiddleName", data.MiddleName);
                command.Parameters.AddWithValue("@Name", data.Name);
                command.Parameters.AddWithValue("@Email", data.Email);
                command.Parameters.AddWithValue("@LastModifiedDate", data.LastModifiedDate);
                command.Parameters.AddWithValue("@Active", data.Active);
                command.Parameters.AddWithValue("@LaborTypeDetailsValue", data.LaborTypeDetails.Value);
                command.Parameters.AddWithValue("@LaborTypeDetailsDescription", data.LaborTypeDetails.Description);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");

            }
            return newId;
        }

        public List<UserProfile> GetAllTMSUsers()
        {
            List<UserProfile> lists = new List<UserProfile>();
            const string sql = StoredProcedures.TMS_MC_GetUsers;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                var ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                    lists = _sqlHelper.ConvertDataTable<UserProfile>(ds.Tables[0]);
            }
            return lists;
        }
        #endregion

        #region Lookup Table & Values 

        public int SavelookupTableToLocal(TMS_LookupTable objlookuptable)
        {
            int newId;
            const string sql = StoredProcedures.TMSAkron_InsertLookupTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LookupTableID", objlookuptable.LookupTableID);
                command.Parameters.AddWithValue("@Description", objlookuptable.Description);
                command.Parameters.AddWithValue("@CodeWidth", objlookuptable.CodeWidth);
                command.Parameters.AddWithValue("@DescriptionWidth", objlookuptable.DescriptionWidth);
                command.Parameters.AddWithValue("@Internal", objlookuptable.Internal);
                command.Parameters.AddWithValue("@Enabled", objlookuptable.Enabled);
                command.Parameters.AddWithValue("@SkipValidation", objlookuptable.SkipValidation);
                command.Parameters.AddWithValue("@CanModify", objlookuptable.CanModify);
                command.Parameters.AddWithValue("@NoCascadeUpdate", objlookuptable.NoCascadeUpdate);
                command.Parameters.AddWithValue("@LastModifiedDate", objlookuptable.LastModifiedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SavelookupTableValuesToLocal(TMS_LookupTableValues objlookuptablevalues)
        {
            int newId;
            const string sql = StoredProcedures.TMSAkron_InsertLookupTableValues;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LookupTableID", objlookuptablevalues.LookupTableID);
                command.Parameters.AddWithValue("@CodeName", objlookuptablevalues.CodeName);
                command.Parameters.AddWithValue("@CodeDesc", objlookuptablevalues.CodeDesc);
                command.Parameters.AddWithValue("@CodeValue", objlookuptablevalues.CodeValue);
                command.Parameters.AddWithValue("@SystemCode", objlookuptablevalues.SystemCode);
                command.Parameters.AddWithValue("@AvailableToRequester", objlookuptablevalues.AvailableToRequester);
                command.Parameters.AddWithValue("@LastModifiedDate", objlookuptablevalues.LastModifiedDate);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion



        #region NBIH 

        public int SaveAssetsTempTable(TMSNBIAssetsResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertAssetsTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssetID", newData.AssetID);
                command.Parameters.AddWithValue("@AssetNumber", newData.AssetNumber);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@ManufacturerName", newData.ManufacturerName);
                command.Parameters.AddWithValue("@ManufacturerCode", newData.ManufacturerCode);
                command.Parameters.AddWithValue("@ModelNumber", newData.ModelNumber);
                command.Parameters.AddWithValue("@SerialNumber", newData.SerialNumber);
                command.Parameters.AddWithValue("@CategoryCode", newData.CategoryCode);
                command.Parameters.AddWithValue("@CategoryDescription", newData.CategoryDescription);
                command.Parameters.AddWithValue("@SubCategoryCode", newData.SubCategoryCode);
                command.Parameters.AddWithValue("@SubCategoryDescription", newData.SubCategoryDescription);
                command.Parameters.AddWithValue("@TypeCode", newData.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newData.TypeDescription);
                command.Parameters.AddWithValue("@AccountCode", newData.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newData.AccountDescription);
                command.Parameters.AddWithValue("@StatusCode", newData.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newData.StatusDescription);
                command.Parameters.AddWithValue("@SkillCode", newData.SkillCode);
                command.Parameters.AddWithValue("@SkillDescription", newData.SkillDescription);
                command.Parameters.AddWithValue("@PriorityCode", newData.PriorityCode);
                command.Parameters.AddWithValue("@PriorityDescription", newData.PriorityDescription);
                command.Parameters.AddWithValue("@SiteCode", newData.SiteCode);
                command.Parameters.AddWithValue("@SiteName", newData.SiteName);
                command.Parameters.AddWithValue("@BuildingCode", newData.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newData.BuildingName);
                command.Parameters.AddWithValue("@LocationCode", newData.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newData.LocationDescription);
                command.Parameters.AddWithValue("@ShopCode", newData.ShopCode);
                command.Parameters.AddWithValue("@ShopDescription", newData.ShopDescription);
                //command.Parameters.AddWithValue("@DatePurchased", newData.DatePurchased);
                //command.Parameters.AddWithValue("@DateReceived", newData.DateReceived);
                //command.Parameters.AddWithValue("@DateAccepted", newData.DateAccepted);
                command.Parameters.AddWithValue("@OriginalCost", newData.OriginalCost);
                command.Parameters.AddWithValue("@ReplacementCost", newData.ReplacementCost);
                command.Parameters.AddWithValue("@IDNumber", newData.IDNumber);
                command.Parameters.AddWithValue("@ItemIdentifier", newData.ItemIdentifier);
                command.Parameters.AddWithValue("@AreaCode", newData.AreaCode);
                command.Parameters.AddWithValue("@AreaDescription", newData.AreaDescription);
                command.Parameters.AddWithValue("@ServiceManual", newData.ServiceManual);
                command.Parameters.AddWithValue("@ManualLocation", newData.ManualLocation);
                command.Parameters.AddWithValue("@NetworkDevice", newData.NetworkDevice);
                command.Parameters.AddWithValue("@MeterTypeCode", newData.MeterTypeCode);
                command.Parameters.AddWithValue("@MeterTypeDescription", newData.MeterTypeDescription);
                command.Parameters.AddWithValue("@MeterReading", newData.MeterReading);
                command.Parameters.AddWithValue("@PhysicalConditionCode", newData.PhysicalConditionCode);
                command.Parameters.AddWithValue("@PhysicalConditionDescription", newData.PhysicalConditionDescription);
                command.Parameters.AddWithValue("@Owner", newData.Owner);
                command.Parameters.AddWithValue("@SupportStatusCode", newData.SupportStatusCode);
                command.Parameters.AddWithValue("@SupportStatusDescription", newData.SupportStatusDescription);
                //command.Parameters.AddWithValue("@DateRetired", newData.DateRetired);
                command.Parameters.AddWithValue("@WarrantyCompanyName", newData.WarrantyCompanyName);
                command.Parameters.AddWithValue("@WarrantyCompanyCode", newData.WarrantyCompanyCode);
                //command.Parameters.AddWithValue("@WarrantyStartDate", newData.WarrantyStartDate);
                //command.Parameters.AddWithValue("@WarrantyEndDate", newData.WarrantyEndDate);
                command.Parameters.AddWithValue("@OriginalPONumber", newData.OriginalPONumber);
                command.Parameters.AddWithValue("@Department", newData.Department);
                command.Parameters.AddWithValue("@BuildingDescription", newData.BuildingDescription);
                command.Parameters.AddWithValue("@Room", newData.Room);
                command.Parameters.AddWithValue("@UDF1", newData.UDF1);
                command.Parameters.AddWithValue("@UDF2", newData.UDF2);
                command.Parameters.AddWithValue("@UDF3", newData.UDF3);
                command.Parameters.AddWithValue("@UDF4", newData.UDF4);
                command.Parameters.AddWithValue("@UDF5", newData.UDF5);
                command.Parameters.AddWithValue("@UDF6", newData.UDF6);
                command.Parameters.AddWithValue("@UDF7", newData.UDF7);
                command.Parameters.AddWithValue("@UDF8", newData.UDF8);
                command.Parameters.AddWithValue("@UDF9", newData.UDF9);
                command.Parameters.AddWithValue("@UDF10", newData.UDF10);
                command.Parameters.AddWithValue("@ExtendedDescription", newData.ExtendedDescription);
                command.Parameters.AddWithValue("@Options", newData.Options);
                command.Parameters.AddWithValue("@SoftwareTest", newData.SoftwareTest);
                command.Parameters.AddWithValue("@CostBasis", newData.CostBasis);
                command.Parameters.AddWithValue("@SalvageValue", newData.SalvageValue);
                command.Parameters.AddWithValue("@LifeExpectancy", newData.LifeExpectancy);
                //command.Parameters.AddWithValue("@StartingDate", newData.StartingDate);
                command.Parameters.AddWithValue("@FirstMonth", newData.FirstMonth);
                command.Parameters.AddWithValue("@RiskNumber", newData.RiskNumber);
                command.Parameters.AddWithValue("@DateCreated", newData.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", newData.DateUpdated);
                command.Parameters.AddWithValue("@HIPAACode", newData.HIPAACode);
                command.Parameters.AddWithValue("@HIPAADescription", newData.HIPAADescription);
                command.Parameters.AddWithValue("@NextPMDate", newData.NextPMDate);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@FieldId", newData.FieldId);
                command.Parameters.AddWithValue("@ValidateForDataImport", newData.ValidateForDataImport);
                command.Parameters.AddWithValue("@KeyID", newData.KeyID);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveBuildingsTempTable(TMSNBIBuildingsResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertBuildingTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@SiteCode", newData.SiteCode);
                command.Parameters.AddWithValue("@SiteDescription", newData.SiteDescription);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInQuery", newData.ShowInQuery);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SavePriorityTempTable(TMSNBIPriorityResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertPriorityTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@ModuleCode", newData.ModuleCode);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInQuery", newData.ShowInQuery);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveAccountTempTable(TMSNBIAccountResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertAccountTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInCreate", newData.ShowInQuery);
                command.Parameters.AddWithValue("@DepartmentCode", newData.DepartmentCode);
                command.Parameters.AddWithValue("@DepartmentDescription", newData.DepartmentDescription);
                command.Parameters.AddWithValue("@TypeCode", newData.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newData.TypeDescription);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveSitesTempTable(TMSNBISitesResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertSiteTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInCreate", newData.ShowInQuery);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveTypeTempTable(TMSNBITypeResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertTypeTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@Escalate", newData.Escalate);
                command.Parameters.AddWithValue("@ShowInCreate", newData.ShowInQuery);
                command.Parameters.AddWithValue("@ModuleCode", newData.ModuleCode);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveStatusTempTable(TMSNBIStatusResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertStatusTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@ModuleCode", newData.ModuleCode);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInQuery", newData.ShowInQuery);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveResourcesTempTable(TMSNBIResourceResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertResourceTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountCode", newData.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newData.AccountDescription);
                command.Parameters.AddWithValue("@Address1", newData.Address1);
                command.Parameters.AddWithValue("@Address2", newData.Address2);
                command.Parameters.AddWithValue("@AreaCode", newData.AreaCode);
                command.Parameters.AddWithValue("@AreaDescription", newData.AreaDescription);
                command.Parameters.AddWithValue("@BuildingCode", newData.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newData.BuildingName);
                command.Parameters.AddWithValue("@ChargeRate", newData.ChargeRate);
                command.Parameters.AddWithValue("@City", newData.City);
                command.Parameters.AddWithValue("@DateCreated", newData.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", newData.DateUpdated);
                command.Parameters.AddWithValue("@Email", newData.Email);
                command.Parameters.AddWithValue("@EmergencyContact", newData.EmergencyContact);
                command.Parameters.AddWithValue("@EmergencyContactPhone", newData.EmergencyContactPhone);
                command.Parameters.AddWithValue("@FirstName", newData.FirstName);
                command.Parameters.AddWithValue("@HomePhone", newData.HomePhone);
                //command.Parameters.AddWithValue("@IsActive", newData.IsActive);
                command.Parameters.AddWithValue("@LastName", newData.LastName);
                //command.Parameters.AddWithValue("@LastReviewDate", newData.LastReviewDate);
                command.Parameters.AddWithValue("@LocationCode", newData.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newData.LocationDescription);
                command.Parameters.AddWithValue("@Name", newData.Name);
                command.Parameters.AddWithValue("@Notes", newData.Notes);
                command.Parameters.AddWithValue("@PagerEmail", newData.PagerEmail);
                command.Parameters.AddWithValue("@ResourceNumber", newData.ResourceNumber);
                command.Parameters.AddWithValue("@ResourceID", newData.ResourceID);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@ShiftCode", newData.ShiftCode);
                command.Parameters.AddWithValue("@ShiftDescription", newData.ShiftDescription);
                command.Parameters.AddWithValue("@ShopCode", newData.ShopCode);
                command.Parameters.AddWithValue("@ShopDescription", newData.ShopDescription);
                command.Parameters.AddWithValue("@SkillCode", newData.SkillCode);
                command.Parameters.AddWithValue("@SkillDescription", newData.SkillDescription);
                command.Parameters.AddWithValue("@SkillLevelCode", newData.SkillLevelCode);
                //command.Parameters.AddWithValue("@StartDate", newData.StartDate);
                command.Parameters.AddWithValue("@State", newData.State);
                command.Parameters.AddWithValue("@StatusCode", newData.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newData.StatusDescription);
                command.Parameters.AddWithValue("@TypeCode", newData.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newData.TypeDescription);
                command.Parameters.AddWithValue("@UDF1", newData.UserField1);
                command.Parameters.AddWithValue("@UDF2", newData.UserField2);
                command.Parameters.AddWithValue("@UDF3", newData.UserField3);
                command.Parameters.AddWithValue("@UDF4", newData.UserField4);
                command.Parameters.AddWithValue("@UDF5", newData.UserField5);
                command.Parameters.AddWithValue("@UDF6", newData.UserField6);
                command.Parameters.AddWithValue("@UDF7", newData.UserField7);
                command.Parameters.AddWithValue("@UDF8", newData.UserField8);
                command.Parameters.AddWithValue("@UDF9", newData.UserField9);
                command.Parameters.AddWithValue("@UDF10", newData.UserField10);
                command.Parameters.AddWithValue("@Zip", newData.Zip);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveSubStatusTempTable(TMSNBISubStatusResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertSubStatusTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@ModuleCode", newData.ModuleCode);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInCreate", newData.ShowInQuery);
                command.Parameters.AddWithValue("@StatusCode", newData.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newData.StatusDescription);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveLocationTempTable(TMSNBISLocationResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertLocationTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", newData.ID);
                command.Parameters.AddWithValue("@Code", newData.Code);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@Show", newData.Show);
                command.Parameters.AddWithValue("@ShowInCreate", newData.ShowInQuery);
                command.Parameters.AddWithValue("@SiteCode", newData.SiteCode);
                command.Parameters.AddWithValue("@SiteDescription", newData.SiteDescription);
                command.Parameters.AddWithValue("@BuildingCode", newData.BuildingCode);
                command.Parameters.AddWithValue("@BuildingDescription", newData.BuildingDescription);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public int SaveRequesterTempTable(TMSNBISRequesterResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertRequesterTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountCode", newData.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newData.AccountDescription);
                command.Parameters.AddWithValue("@BuildingCode", newData.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newData.BuildingName);
                command.Parameters.AddWithValue("@DateCreated", newData.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", newData.DateUpdated);
                command.Parameters.AddWithValue("@LocationCode", newData.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newData.LocationDescription);
                command.Parameters.AddWithValue("@PagerNumber", newData.PagerNumber);
                command.Parameters.AddWithValue("@PhoneNumber", newData.PhoneNumber);
                command.Parameters.AddWithValue("@RequesterID", newData.RequesterID);
                command.Parameters.AddWithValue("@RequesterName", newData.RequesterName);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@SiteCode", newData.SiteCode);
                command.Parameters.AddWithValue("@SiteName", newData.SiteName);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public int SaveWorkOrderTempTable(TMSNBIWorkOrderResults newData)
        {
            int newId;
            const string sql = StoredProcedures.TMSNBI_InsertWorkOrderTempTable;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AccountCode", newData.AccountCode);
                command.Parameters.AddWithValue("@AccountDescription", newData.AccountDescription);
                command.Parameters.AddWithValue("@ActionCode", newData.ActionCode);
                command.Parameters.AddWithValue("@ActionDescription", newData.ActionDescription);
                command.Parameters.AddWithValue("@AssetGroupNumber", newData.AssetGroupNumber);
                command.Parameters.AddWithValue("@AssetNumber", newData.AssetNumber);
                command.Parameters.AddWithValue("@BuildingCode", newData.BuildingCode);
                command.Parameters.AddWithValue("@BuildingName", newData.BuildingName);
                command.Parameters.AddWithValue("@CauseCode", newData.CauseCode);
                command.Parameters.AddWithValue("@CauseDescription", newData.CauseDescription);
                command.Parameters.AddWithValue("@CompletionComments", newData.CompletionComments);
                command.Parameters.AddWithValue("@DateAvailable", newData.DateAvailable);
                command.Parameters.AddWithValue("@DateCompleted", newData.DateCompleted);
                command.Parameters.AddWithValue("@DateCreated", newData.DateCreated);
                command.Parameters.AddWithValue("@DateNeeded", newData.DateNeeded);
                command.Parameters.AddWithValue("@DateUpdated", newData.DateUpdated);
                command.Parameters.AddWithValue("@Description", newData.Description);
                command.Parameters.AddWithValue("@ItemCode", newData.ItemCode);
                command.Parameters.AddWithValue("@ItemDescription", newData.ItemDescription);
                command.Parameters.AddWithValue("@LocationCode", newData.LocationCode);
                command.Parameters.AddWithValue("@LocationDescription", newData.LocationDescription);
                command.Parameters.AddWithValue("@MeterReading", newData.MeterReading);
                command.Parameters.AddWithValue("@PriorityCode", newData.PriorityCode);
                command.Parameters.AddWithValue("@PriorityDescription", newData.PriorityDescription);
                command.Parameters.AddWithValue("@ProblemCode", newData.ProblemCode);
                command.Parameters.AddWithValue("@ProblemDescription", newData.ProblemDescription);
                command.Parameters.AddWithValue("@ReferenceNumber", newData.ReferenceNumber);
                command.Parameters.AddWithValue("@RequesterComments", newData.RequesterComments);
                command.Parameters.AddWithValue("@RequesterEmail", newData.RequesterEmail);
                command.Parameters.AddWithValue("@RequesterName", newData.RequesterName);
                command.Parameters.AddWithValue("@RequesterPager", newData.RequesterPager);
                command.Parameters.AddWithValue("@RequesterPhone", newData.RequesterPhone);
                command.Parameters.AddWithValue("@SegmentID", newData.SegmentID);
                command.Parameters.AddWithValue("@ShopCode", newData.ShopCode);
                command.Parameters.AddWithValue("@ShopDescription", newData.ShopDescription);
                command.Parameters.AddWithValue("@SiteCode", newData.SiteCode);
                command.Parameters.AddWithValue("@SiteName", newData.SiteName);
                command.Parameters.AddWithValue("@SkillCode", newData.SkillCode);
                command.Parameters.AddWithValue("@SkillDescription", newData.SkillDescription);
                command.Parameters.AddWithValue("@StatusCode", newData.StatusCode);
                command.Parameters.AddWithValue("@StatusDescription", newData.StatusDescription);
                command.Parameters.AddWithValue("@SubStatusCode", newData.SubStatusCode);
                command.Parameters.AddWithValue("@SubStatusDescription", newData.SubStatusDescription);
                command.Parameters.AddWithValue("@TotalCost", newData.TotalCost);
                command.Parameters.AddWithValue("@TotalHours", newData.TotalHours);
                command.Parameters.AddWithValue("@TotalMaterialCharges", newData.TotalMaterialCharges);
                command.Parameters.AddWithValue("@TotalTimeCharges", newData.TotalTimeCharges);
                command.Parameters.AddWithValue("@TypeCode", newData.TypeCode);
                command.Parameters.AddWithValue("@TypeDescription", newData.TypeDescription);
                command.Parameters.AddWithValue("@UDF1", newData.UDF1);
                command.Parameters.AddWithValue("@UDF2", newData.UDF2);
                command.Parameters.AddWithValue("@UDF3", newData.UDF3);
                command.Parameters.AddWithValue("@UDF4", newData.UDF4);
                command.Parameters.AddWithValue("@UDF5", newData.UDF5);
                command.Parameters.AddWithValue("@UDF6", newData.UDF6);
                command.Parameters.AddWithValue("@UDF7", newData.UDF7);
                command.Parameters.AddWithValue("@UDF8", newData.UDF8);
                command.Parameters.AddWithValue("@UDF9", newData.UDF9);
                command.Parameters.AddWithValue("@UDF10", newData.UDF10);
                // command.Parameters.AddWithValue("@WeekScheduled", newData.WeekScheduled==DateTime.MinValue ?null: newData.WeekScheduled);
                command.Parameters.AddWithValue("@WorkOrderID", newData.WorkOrderID);
                command.Parameters.AddWithValue("@WorkOrderNumber", newData.WorkOrderNumber);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        #endregion
    }
}
