using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using System.Text;
using HCF.Utility;
using System.Globalization;
using System.Web;
using Newtonsoft.Json;
using HCF.Web.Filters;
using HCF.BAL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HCF.Web.Controllers
{
    //[Authorize]
    //public class ImportController : BaseController
    //{
    //    private readonly IAssetsService _assetService;
    //    private readonly IAssetTypeService _assetTypeService;
    //    private readonly IFloorService _floorService;
    //    private readonly ITransactionService _transactionService;
    //    private readonly ICSVService _csvService;
    //    private readonly IEpService _epService;
    //    private readonly ILoggingService _loggingService;
    //    private readonly IHttpPostFactory _httpService;
    //    public ImportController(IAssetsService assetService, IAssetTypeService assetTypeService, IFloorService floorService,
    //        ITransactionService transactionService, ICSVService csvService, IEpService epService, ILoggingService logging, IHttpPostFactory httpService) : base(logging)
    //    {
    //        _loggingService = logging;
    //        _assetTypeService = assetTypeService;
    //        _assetService = assetService;
    //        _floorService = floorService;
    //        _transactionService = transactionService;
    //        _csvService = csvService;
    //        _epService = epService;
    //        _httpService = httpService;
    //    }

    //    #region Import Excel
    //    public ActionResult ImportExcel()
    //    {
    //        return View();
    //    }

    //    [ActionName("ImportExcel")]
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult ImportExcel1()
    //    {
    //        var dtrecords = new DataTable();
    //        string connString = "";
    //        if (Request.Files["FileUpload1"].ContentLength > 0)
    //        {
    //            string extension = Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
    //            //string query = null;
    //            string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
    //            string path = $"{Server.MapPath("~/Content/Uploads")}/{Request.Files["FileUpload1"].FileName}";
    //            if (!Directory.Exists(path))
    //            {
    //                Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
    //            }
    //            if (validFileTypes.Contains(extension))
    //            {
    //                if (System.IO.File.Exists(path))
    //                { System.IO.File.Delete(path); }
    //                Request.Files["FileUpload1"].SaveAs(path);
    //                if (extension == ".csv")
    //                {
    //                    dtrecords = Utility.CommonUtility.ConvertCsVtoDataTable(path);
    //                }
    //                //Connection String to Excel Workbook  
    //                else switch (extension.Trim())
    //                    {
    //                        case ".xls":
    //                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
    //                            dtrecords = CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                            break;
    //                        case ".xlsx":
    //                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
    //                            dtrecords = CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                            break;
    //                    }
    //                ViewBag.Data = dtrecords;
    //                Session["tableData"] = ViewBag.Data;
    //                if (dtrecords != null)
    //                {
    //                    if (dtrecords.Rows.Count > 0)
    //                        ConvertToFloorAssets(dtrecords);


    //                }
    //            }
    //            else
    //            {
    //                ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format only";
    //            }
    //        }
    //        else
    //        {
    //            if (Session["tableData"] != null)
    //            {
    //                dtrecords = (DataTable)Session["tableData"];
    //            }
    //            else
    //            {
    //                ViewBag.Error = "Please select a file";
    //            }
    //        }
    //        return View();
    //    }
    //    #endregion

    //    private void ConvertToFloorAssets(DataTable dtrecords)
    //    {
    //        var floorAssets = new List<TFloorAssets>();
    //        foreach (DataRow item in dtrecords.Rows)
    //        {
    //            var floorAsset = new TFloorAssets
    //            {
    //                AssetId = Convert.ToInt32(item["AssetId"].ToString()),
    //                AscId = Convert.ToInt32(item["AscId"].ToString()),
    //                SerialNo = item["TagNo"].ToString(),
    //                DeviceNo = item["TagNo"].ToString(),
    //                NearBy = item["LocationName"].ToString(),
    //                Name = item["Model"].ToString(),
    //                Stop = new StopMaster
    //                {
    //                    StopCode = item["StopCode"].ToString(),
    //                    StopName = item["StopName"].ToString()
    //                },
    //                FloorId = Convert.ToInt32(item["FloorId"].ToString()),
    //                Model = item["Model"].ToString(),
    //                Source = (int)Enums.Source.Internal,
    //                CreatedBy = 4
    //            };
    //            floorAssets.Add(floorAsset);
    //        }

    //        //JavaScriptSerializer js = new JavaScriptSerializer();
    //        //js.MaxJsonLength = Int32.MaxValue;

    //        string Uri = string.Empty;
    //        Uri = APIUrls.Assets_tfloorAssetEdit_Add;// "assets/addfloorassets";
    //        foreach (var floorAsset in floorAssets)
    //        {
    //            string postData = Base.Common.JsonSerialize<List<TFloorAssets>>(floorAsset);
    //            int statusCode = Convert.ToInt32(Enums.HttpReponseStatusCode.Success);
    //            _httpService.CallPostMethod(postData, Uri, ref statusCode);
    //        }
    //    }

    //    private void Insertfloor(DataTable dtrecords)
    //    {
    //        var view = new DataView(dtrecords);
    //        var distinctValues = view.ToTable(true, "FloorName", "BuildingId");
    //        foreach (DataRow rows in distinctValues.Rows)
    //        {
    //            var floor = new Floor
    //            {
    //                FloorName = rows["FloorName"].ToString()
    //            };
    //            //floor.IsActive = true;
    //            int floorId = 0;
    //            floor.BuildingId = Convert.ToInt32(rows["BuildingId"].ToString());
    //            floor.CreatedBy = UserSession.CurrentUser.UserId;
    //            int _floorId = _floorService.Save(floor);
    //            var drs = dtrecords.AsEnumerable().Where(r => ((string)r["FloorName"]).Equals(floor.FloorName)).ToList();
    //            floorId = _floorId > 0 ? _floorId : _floorService.GetFloors().FirstOrDefault(m => m.FloorName == floor.FloorName).FloorId;
    //            foreach (var dr in drs)
    //            {
    //                dr["FloorId"] = floorId;
    //            }
    //        }
    //    }

    //    private void InsertAssetType(DataTable dtrecords)
    //    {
    //        DataView view = new DataView(dtrecords);
    //        DataTable distinctValues = view.ToTable(true, "AssetType");
    //        foreach (DataRow rows in distinctValues.Rows)
    //        {
    //            AssetType type = new AssetType
    //            {
    //                Name = rows["AssetType"].ToString(),
    //                IsActive = true
    //            };
    //            int TypeId = 0;
    //            type.CreatedBy = UserSession.CurrentUser.UserId;
    //            int _TypeId = _assetTypeService.Save(type);
    //            List<DataRow> drs = dtrecords.AsEnumerable().Where(r => ((string)r["AssetType"]).Equals(type.Name)).ToList();
    //            TypeId = _TypeId > 0 ? _TypeId : _assetTypeService.GetAssetsType().Where(x => x.Name == type.Name).ToList().FirstOrDefault().TypeId;
    //            foreach (DataRow dr in drs)
    //            {
    //                dr["TypeId"] = TypeId;
    //            }
    //        }
    //        InsertAssets(dtrecords);
    //    }

    //    private void InsertAssets(DataTable dtrecords)
    //    {
    //        DataView view = new DataView(dtrecords);
    //        DataTable distinctValues = view.ToTable(true, "Assets", "TypeId");
    //        foreach (DataRow rows in distinctValues.Rows)
    //        {
    //            Assets assets = new Assets
    //            {
    //                Name = rows["Assets"].ToString(),
    //                IsActive = true
    //            };
    //            int assetId = 0;
    //            assets.AssetTypeId = Convert.ToInt32(rows["TypeId"].ToString());
    //            assets.CreatedBy = UserSession.CurrentUser.UserId;
    //            int _assetId = _assetService.AddAssets(assets);
    //            List<DataRow> drs = dtrecords.AsEnumerable().Where(r => ((string)r["Assets"]).Equals(assets.Name)).ToList();
    //            assetId = _assetId > 0 ? _assetId : _assetService.GetAssets(assets.CreatedBy).Where(x => x.Name == assets.Name).ToList().FirstOrDefault().AssetId;
    //            foreach (DataRow dr in drs)
    //            {
    //                dr["AssetId"] = assetId;
    //            }
    //        }
    //    }

    //    public ActionResult DownladSapmle()
    //    {
    //        return View();
    //    }


    //    #region Import Non Assets Eps

    //    public ActionResult ImportNonAssetsEPs()
    //    {
    //        HCF.Web.Base.UISession.AddCurrentPage("import_importnonassetseps", 0, "Compliance Dates ");
    //        List<EPDetails> epdetails = _epService.GetNonAssetsEp();
    //        return View(epdetails);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult NonAssetsEPsInspection(IEnumerable<EPDetails> model)
    //    {
    //        foreach (EPDetails ep in model)
    //        {
    //            if (!string.IsNullOrEmpty(ep.InspectionDate.ToString()))
    //            {
    //                if (ep.EPDetailId > 0)
    //                {
    //                    DateTime inspectiondate = ep.InspectionDate.Value; //DateTime.ParseExact(ep.InspectionDate, "M/d/yyyy", CultureInfo.InvariantCulture);
    //                    var modeType = Convert.ToInt32(Enums.Mode.EP);
    //                    var epdocs = _transactionService.GetGoalStepsByActivity(null, null, ep.EPDetailId, 0, modeType, 0);
    //                    var createdBy = Convert.ToInt32(UserSession.CurrentUser.UserId);
    //                    var inspection = new Inspection
    //                    {
    //                        InspectionId = 0,
    //                        EPDetailId = ep.EPDetailId,
    //                        DocStatus = -3,
    //                        InspectionGroupId = 0,
    //                        Status = 1,
    //                        StartDate = inspectiondate
    //                    };
    //                    var newInspectionId = _transactionService.AddInspection(inspection);
    //                    if (newInspectionId > 0)
    //                    {
    //                        var inspectionActicity = new TInspectionActivity
    //                        {
    //                            ActivityType = Utility.CommonUtility.SetActivityType(Enums.Mode.EP.ToString()),
    //                            ActivityId = Guid.NewGuid(),
    //                            CreatedBy = createdBy,
    //                            InspectionId = newInspectionId,
    //                            Status = 1,
    //                            ActivityInspectionDate = inspectiondate,
    //                            IsDeficiency = false,
    //                            //FloorAssetId = assets.floorAssetId,
    //                            SubStatus = Enums.InspectionSubStatus.NA.ToString(),
    //                            Comment = "",
    //                            DtEffectiveDate = DateTime.UtcNow
    //                        };
    //                        var activityId = _transactionService.AddTInspectionActivity(inspectionActicity);
    //                        if (activityId > 0)
    //                        {
    //                            foreach (var goal in epdocs[0].MainGoal)
    //                            {
    //                                var detail = new TInspectionDetail
    //                                {
    //                                    MainGoalId = goal.MainGoalId,
    //                                    ActivityId = inspectionActicity.ActivityId
    //                                };
    //                                var inspectionDetailId = _transactionService.AddInspectionDetails(detail);
    //                                if (inspectionDetailId > 0)
    //                                    foreach (var steps in goal.Steps)
    //                                    {
    //                                        var tsteps = new InspectionSteps
    //                                        {
    //                                            InspectionDetailId = inspectionDetailId,
    //                                            StepsId = steps.StepsId,
    //                                            Status = 1,
    //                                            Comments = ""
    //                                        };
    //                                        _transactionService.AddInspectionSteps(tsteps);
    //                                    }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return RedirectToAction("ImportNonAssetsEPs");
    //    }

    //    //[HttpPost]
    //    ////[ValidateAntiForgeryToken]
    //    //public JsonResult ComplianceEPsInspection(int epdetailId, string inspectiondate)
    //    //{
    //    //    if (!string.IsNullOrEmpty(inspectiondate.ToString()))
    //    //    {
    //    //        if (epdetailId > 0)
    //    //        {
    //    //            DateTime InspectionDate = DateTime.ParseExact(inspectiondate, "M/d/yyyy", CultureInfo.InvariantCulture);
    //    //            var modeType = Convert.ToInt32(Enums.Mode.EP);
    //    //            var epdocs = _transactionService.GetGoalStepsByActivity(null, null, epdetailId, 0, modeType, 0);
    //    //            var createdBy = Convert.ToInt32(UserSession.CurrentUser.UserId);
    //    //            var inspection = new Inspection
    //    //            {
    //    //                InspectionId = 0,
    //    //                EPDetailId = epdetailId,
    //    //                DocStatus = -3,
    //    //                InspectionGroupId = 0,
    //    //                Status = 1,
    //    //                StartDate = InspectionDate
    //    //            };
    //    //            var newInspectionId = _transactionService.AddInspection(inspection);
    //    //            if (newInspectionId > 0)
    //    //            {
    //    //                var inspectionActicity = new TInspectionActivity
    //    //                {
    //    //                    ActivityType = Utility.Common.SetActivityType(Enums.Mode.EP.ToString()),
    //    //                    ActivityId = Guid.NewGuid(),
    //    //                    CreatedBy = createdBy,
    //    //                    InspectionId = newInspectionId,
    //    //                    Status = 1,
    //    //                    ActivityInspectionDate = InspectionDate,
    //    //                    IsDeficiency = false,
    //    //                    //FloorAssetId = assets.floorAssetId,
    //    //                    SubStatus = Enums.InspectionSubStatus.NA.ToString(),
    //    //                    Comment = "",
    //    //                    DtEffectiveDate = DateTime.UtcNow
    //    //                };
    //    //                var activityId = _transactionService.AddTInspectionActivity(inspectionActicity);
    //    //                if (activityId > 0)
    //    //                {
    //    //                    foreach (var goal in epdocs[0].MainGoal)
    //    //                    {
    //    //                        var detail = new TInspectionDetail
    //    //                        {
    //    //                            MainGoalId = goal.MainGoalId,
    //    //                            ActivityId = inspectionActicity.ActivityId
    //    //                        };
    //    //                        var inspectionDetailId = _transactionService.AddInspectionDetails(detail);
    //    //                        if (inspectionDetailId > 0)
    //    //                            foreach (var steps in goal.Steps)
    //    //                            {
    //    //                                var tsteps = new InspectionSteps
    //    //                                {
    //    //                                    InspectionDetailId = inspectionDetailId,
    //    //                                    StepsId = steps.StepsId,
    //    //                                    Status = 1,
    //    //                                    Comments = ""
    //    //                                };
    //    //                                _transactionService.AddInspectionSteps(tsteps);
    //    //                            }
    //    //                    }
    //    //                }
    //    //            }

    //    //            var EpConfig = new EpConfig()
    //    //            {
    //    //                EPDetailId = epdetailId,
    //    //                InspectionDate = InspectionDate,
    //    //                ClientNo = Base.UserSession.CurrentOrg.ClientNo,
    //    //                IsActiveEp = true,
    //    //                IsApplicable = true,
    //    //                InspectionGroupId = -1
    //    //            };
    //    //            var EpConfigId = _transactionService.InsertUpdateEpConfig(EpConfig);
    //    //        }
    //    //    }
    //    //    var result = new { Result = true, EPDetailId = epdetailId };
    //    //    return Json(result, JsonRequestBehavior.AllowGet);
    //    //}

    //    //[HttpPost]
    //    ////[ValidateAntiForgeryToken]
    //    //public JsonResult SheduleDateEpInspection(int epdetailId, string inspectiondate)
    //    //{
    //    //    if (!string.IsNullOrEmpty(inspectiondate.ToString()))
    //    //    {
    //    //        if (epdetailId > 0)
    //    //        {
    //    //            DateTime InspectionDate = DateTime.ParseExact(inspectiondate, "M/d/yyyy", CultureInfo.InvariantCulture);
    //    //           var createdBy = Convert.ToInt32(UserSession.CurrentUser.UserId);


    //    //            var EpConfig = new EpConfig()
    //    //            {
    //    //                EPDetailId = epdetailId,
    //    //                InspectionDate = InspectionDate,
    //    //                ClientNo = Base.UserSession.CurrentOrg.ClientNo,
    //    //                IsActiveEp = true,
    //    //                IsApplicable = true,
    //    //                InspectionGroupId = -1
    //    //            };
    //    //            var EpConfigId = _transactionService.InsertUpdateEpConfig(EpConfig);
    //    //        }
    //    //    }
    //    //    var result = new { Result = true, EPDetailId = epdetailId };
    //    //    return Json(result, JsonRequestBehavior.AllowGet);
    //    //}

    //    public FileContentResult GetNonAssetsEps()
    //    {
    //        DataTable dt = _epService.GetNonAssetsEps();

    //        StringBuilder sb = new StringBuilder();
    //        IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
    //                                          Select(column => column.ColumnName);
    //        sb.AppendLine(string.Join(",", columnNames));

    //        foreach (DataRow row in dt.Rows)
    //        {
    //            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
    //            sb.AppendLine(string.Join(",", fields));
    //        }
    //        return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "GetNonAssetsEps.csv");
    //    }

    //    [ActionName("ImportNonAssetsEps")]
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult GetNonAssetsEps1()
    //    {
    //        DataTable dtrecords = new DataTable();
    //        if (Request.Files["FileUpload1"].ContentLength > 0)
    //        {
    //            string extension = Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
    //            //string query = null;
    //            string connString = "";
    //            string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
    //            string path = $"{Server.MapPath("~/Content/Uploads")}/{Request.Files["FileUpload1"].FileName}";
    //            if (!Directory.Exists(path))
    //            {
    //                Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
    //            }
    //            if (validFileTypes.Contains(extension))
    //            {



    //                if (System.IO.File.Exists(path))
    //                { System.IO.File.Delete(path); }



    //                Request.Files["FileUpload1"].SaveAs(path);
    //                if (extension == ".csv")
    //                {
    //                    dtrecords = Utility.CommonUtility.ConvertCsVtoDataTable(path);
    //                }
    //                //Connection String to Excel Workbook  
    //                else if (extension.Trim() == ".xls")
    //                {
    //                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
    //                    dtrecords = Utility.CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                }
    //                else if (extension.Trim() == ".xlsx")
    //                {
    //                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
    //                    dtrecords = Utility.CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                }
    //                ViewBag.Data = dtrecords;
    //                Session["tableData"] = ViewBag.Data;
    //                if (dtrecords != null)
    //                {
    //                    List<EPDetails> epdetails = _epService.GetEpDetails();
    //                    foreach (DataRow datarow in dtrecords.Rows)
    //                    {
    //                        if (!string.IsNullOrEmpty(datarow["InspectionDate"].ToString()))
    //                        {
    //                            string epNumber = datarow["EPNumber"].ToString();
    //                            string TJCStandard = datarow["TJCStandard"].ToString();
    //                            string inspectionDate = datarow["InspectionDate"].ToString();
    //                            EPDetails epdeail = epdetails.FirstOrDefault(x => x.EPNumber == epNumber && x.Standard.TJCStandard == TJCStandard);
    //                            if (epdeail != null)
    //                            {
    //                                DateTime inspectiondate = DateTime.ParseExact(inspectionDate, "M/d/yyyy", CultureInfo.InvariantCulture);
    //                                var modeType = Convert.ToInt32(Enums.Mode.EP);
    //                                var epdocs = _transactionService.GetGoalStepsByActivity(null, null, epdeail.EPDetailId, 0, modeType, 0);
    //                                var createdBy = Convert.ToInt32(UserSession.CurrentUser.UserId);
    //                                var inspection = new Inspection
    //                                {
    //                                    InspectionId = 0,
    //                                    EPDetailId = epdeail.EPDetailId,
    //                                    DocStatus = -3,
    //                                    InspectionGroupId = 0,
    //                                    Status = 1,
    //                                    StartDate = inspectiondate
    //                                };
    //                                var newInspectionId = _transactionService.AddInspection(inspection);
    //                                if (newInspectionId > 0)
    //                                {
    //                                    var inspectionActicity = new TInspectionActivity
    //                                    {
    //                                        ActivityType = Utility.CommonUtility.SetActivityType(Enums.Mode.EP.ToString()),
    //                                        ActivityId = Guid.NewGuid(),
    //                                        CreatedBy = createdBy,
    //                                        InspectionId = newInspectionId,
    //                                        Status = 1,
    //                                        ActivityInspectionDate = inspectiondate,
    //                                        IsDeficiency = false,
    //                                        //FloorAssetId = assets.floorAssetId,
    //                                        SubStatus = Enums.InspectionSubStatus.NA.ToString(),
    //                                        Comment = "",
    //                                        DtEffectiveDate = DateTime.UtcNow
    //                                    };
    //                                    var activityId = _transactionService.AddTInspectionActivity(inspectionActicity);
    //                                    if (activityId > 0)
    //                                    {
    //                                        foreach (var goal in epdocs[0].MainGoal)
    //                                        {
    //                                            var detail = new TInspectionDetail
    //                                            {
    //                                                MainGoalId = goal.MainGoalId,
    //                                                ActivityId = inspectionActicity.ActivityId
    //                                            };
    //                                            var inspectionDetailId = _transactionService.AddInspectionDetails(detail);
    //                                            if (inspectionDetailId > 0)
    //                                                foreach (var steps in goal.Steps)
    //                                                {
    //                                                    var tsteps = new InspectionSteps
    //                                                    {
    //                                                        InspectionDetailId = inspectionDetailId,
    //                                                        StepsId = steps.StepsId,
    //                                                        Status = 1,
    //                                                        Comments = ""
    //                                                    };
    //                                                    _transactionService.AddInspectionSteps(tsteps);
    //                                                }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format only";
    //            }
    //        }
    //        else
    //        {
    //            if (Session["tableData"] != null)
    //            {
    //                dtrecords = (DataTable)Session["tableData"];
    //            }
    //            else
    //            {
    //                ViewBag.Error = "Please select a file";
    //            }
    //        }
    //        return RedirectToRoute("importnonassetseps");
    //    }

    //    #endregion

    //    #region Import CSV file for Update Bulk Data
    //    public ActionResult ImportCsv()
    //    {
    //        return View();
    //    }

    //    /// <summary>
    //    /// Import CSV file data.
    //    /// </summary>
    //    /// <param name="form"></param>
    //    /// <returns></returns>
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult ImportCsv(FormCollection form)
    //    {
    //        DataSet ds = new DataSet();
    //        HttpPostedFileBase postedfile = Request.Files[0];
    //        string importfile = Convert.ToString(form["ddlFiletype"]) != "0" ? Convert.ToString(form["ddlFiletype"]) : string.Empty;
    //        if (!string.IsNullOrEmpty(importfile))
    //        {
    //            if (ModelState.IsValid)
    //            {
    //                if ((postedfile != null && postedfile.ContentLength > 0) && postedfile.FileName.EndsWith(".csv"))
    //                {
    //                    DataTable dtCSV = new DataTable();
    //                    dtCSV = _csvService.ReadCSVFile(postedfile);
    //                    if (dtCSV != null && dtCSV.Rows.Count > 0)
    //                    {
    //                        DataTable dtExistData = null;
    //                        if (importfile == "Assest") dtExistData = _csvService.AssestsCSVMatch();
    //                        string notExistcolumn = string.Empty;
    //                        bool notcolumn = ColumValidate(dtExistData, dtCSV.Columns, ref notExistcolumn);
    //                        if (notcolumn)
    //                        {
    //                            if (Session["CSVData"] != null) Session.Remove("CSVData");
    //                            Session["CSVData"] = dtCSV;
    //                            return View(dtCSV);
    //                        }
    //                        else ModelState.AddModelError("File", HCF.Utility.ConstMessage.ImportCSV_ColumnsError.Replace("[columns]", notExistcolumn));
    //                    }
    //                    else ModelState.AddModelError("File", HCF.Utility.ConstMessage.ImportCSV_NoRecord);
    //                }
    //                else ModelState.AddModelError("File", HCF.Utility.ConstMessage.ImportCSV_fileNotMatch);
    //            }
    //            else ModelState.AddModelError("File", HCF.Utility.ConstMessage.ImportCSV_fileRequired);
    //        }
    //        else ModelState.AddModelError("File", HCF.Utility.ConstMessage.ImportCSV_fileTypeRequired);
    //        return View();
    //    }

    //    #region Validation column Name
    //    /// <summary>
    //    /// Validate column name (match and does not match) from imported CSV file. 
    //    /// </summary>
    //    /// <param name="dtExistData"></param>
    //    /// <param name="CSVcolumns"></param>
    //    /// <param name="notExistcolumn"></param>
    //    /// <returns></returns>
    //    public bool ColumValidate(DataTable dtExistData, DataColumnCollection CSVcolumns, ref string notExistcolumn)
    //    {
    //        bool result = false;
    //        if (dtExistData != null && dtExistData.Rows.Count > 0)
    //        {
    //            DataColumnCollection ExistsColumn = dtExistData.Columns;
    //            List<string> notMatchedcol = new List<string>();
    //            foreach (DataColumn col in CSVcolumns)
    //            {
    //                string columnName = col.ColumnName.IndexOf("\r") > -1 ? col.ColumnName.Replace("\r", "").Trim() : col.ColumnName.Trim();
    //                if (!ExistsColumn.Contains(columnName)) notMatchedcol.Add(columnName);
    //            }
    //            if (notMatchedcol.Count == 0)
    //            {
    //                if (Session["ExistData"] != null) Session.Remove("ExistData");
    //                Session["ExistData"] = dtExistData;
    //                result = true;
    //            }
    //            else
    //            {
    //                notExistcolumn = string.Join(", ", notMatchedcol);
    //                result = false;
    //            }
    //        }
    //        return result;
    //    }
    //    #endregion

    //    #region Upload CSV file for filter Record (Invalid Data, Existed data, New Data)
    //    /// <summary>
    //    /// Upload data After Import from CSV.
    //    /// </summary>
    //    /// <param name="selectjsondata"></param>
    //    /// <param name="filetype"></param>
    //    /// <returns></returns>
    //    [HttpPost]
    //    public ActionResult UploadData(string selectjsondata, string filetype)
    //    {
    //        DataSet ds = new DataSet();
    //        DataTable dtSelectCSVData = JsonConvert.DeserializeObject<DataTable>(selectjsondata);
    //        dtSelectCSVData.TableName = "dtSelectCSVData";

    //        foreach (DataColumn col in dtSelectCSVData.Columns)
    //        {
    //            string colname = col.ColumnName;
    //            col.ColumnName = colname.IndexOf("\n", StringComparison.Ordinal) > -1 ? colname.Trim().Replace("\n", string.Empty) : colname;
    //        }
    //        foreach (DataRow dr in dtSelectCSVData.Rows)
    //        {
    //            foreach (DataColumn col in dtSelectCSVData.Columns)
    //            {
    //                string colval = Convert.ToString(dr[col.ColumnName]);
    //                dr[col.ColumnName] = colval.IndexOf("\n") > -1 ? colval.Trim().Replace("\n", string.Empty) : colval;
    //            }
    //        }

    //        if (dtSelectCSVData != null && dtSelectCSVData.Rows.Count > 0)
    //        {
    //            if (filetype == "Assest")
    //            {
    //                ds = SaveCSVFilterNewAssets(dtSelectCSVData);
    //                if (ds != null)
    //                {
    //                    ds.Tables[0].TableName = "NotMatchData";
    //                    ds.Tables[1].TableName = "MatchData";
    //                    ds.Tables[2].TableName = "NewData";

    //                    #region Not Match data.
    //                    DataTable dtNotMatchData = _csvService.GetDistinctRecord(ds.Tables["NotMatchData"]);
    //                    if (Session["NotMatchData"] != null) Session.Remove("NotMatchData");
    //                    Session["NotMatchData"] = dtNotMatchData;
    //                    #endregion

    //                    #region Match Record data.
    //                    DataTable dtMatchData = _csvService.GetDistinctRecord(ds.Tables["MatchData"]);
    //                    if (Session["MatchData"] != null) Session.Remove("MatchData");
    //                    Session["MatchData"] = dtMatchData;
    //                    #endregion

    //                    #region New Record data.
    //                    DataTable dtNewData = _csvService.GetDistinctRecord(ds.Tables["NewData"]);
    //                    if (Session["NewData"] != null) Session.Remove("NewData");
    //                    Session["NewData"] = dtNewData;
    //                    #endregion
    //                }
    //            }
    //        }
    //        return PartialView("_partialFilterRecordAfterImport", ds);
    //    }
    //    #endregion

    //    #region Download CSV Section
    //    /// <summary>
    //    /// Download Record in CSV.
    //    /// </summary>
    //    public void DownloadRecordInCSV()
    //    {
    //        if (Session["NotMatchData"] != null)
    //        {
    //            DataTable dt = Session["NotMatchData"] as DataTable;
    //            string header = string.Empty, rows = string.Empty;
    //            string[] cols = (from dc in dt.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();
    //            header = string.Join(",", cols);
    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                string[] vals = dr.ItemArray.Select(x => x.ToString()).ToArray();
    //                rows += string.Join(",", vals);
    //            }
    //            string csvdata = header + rows;
    //            _csvService.DownloadCSV(csvdata, string.Empty, "NotMatchData");
    //        }
    //    }

    //    /// <summary>
    //    /// Download Sample Template fiel for Upload Data By CSV.
    //    /// </summary>
    //    /// <param name="filetype"></param>
    //    public void DownloadSampleInCSV(string filetype)
    //    {
    //        if (!string.IsNullOrEmpty(filetype))
    //        {
    //            DataTable dt = null;
    //            if (filetype == "Assest") dt = _csvService.AssestsCSVMatch();
    //            string sampleColumn = string.Empty;
    //            if (dt != null)
    //            {
    //                string[] cols = (from dc in dt.Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();
    //                sampleColumn = string.Join(",", cols);
    //            }
    //            _csvService.DownloadCSV(sampleColumn, filetype, "Sample");
    //        }
    //    }
    //    #endregion

    //    #region Save New Record
    //    //public DataTable SaveCSVFilterNewAssets(DataTable dtMatchData,ref DataTable dtMatchFilter)
    //    //{
    //    //    DataTable dtNew = null;
    //    //    if(dtMatchData != null)
    //    //    {
    //    //        string xmlrecord = HCF.Utility.Common.ConvertDataTableToXML(dtMatchData, "NewRecord", "Record", true);
    //    //        dtNew = BAL.CSVRepository.SaveCSVFilterNewAssets(xmlrecord, Base.UserSession.CurrentUser.UserId);
    //    //        if(dtNew!=null && dtNew.Rows.Count>0)
    //    //        {
    //    //            foreach(DataRow dr in dtNew.Rows)
    //    //            {
    //    //                DataRow[] result = dtMatchData.Select("DeviceNo='"+ Convert.ToString(dr["DeviceNo"]) + "'");
    //    //                foreach(DataRow r in result)
    //    //                {
    //    //                    r.Delete();
    //    //                    dtMatchData.AcceptChanges();
    //    //                }
    //    //            }
    //    //        }
    //    //        dtMatchFilter = dtMatchData;
    //    //    }
    //    //    return dtNew;
    //    //}
    //    public DataSet SaveCSVFilterNewAssets(DataTable dtSelectCSVData)
    //    {
    //        DataSet ds = new DataSet();
    //        if (dtSelectCSVData != null)
    //        {
    //            string xmlrecord = HCF.Utility.CommonUtility.ConvertDataTableToXML(dtSelectCSVData, "NewRecord", "Record", true);
    //            ds = _csvService.SaveCSVFilterNewAssets(xmlrecord, Base.UserSession.CurrentUser.UserId);
    //        }
    //        return ds;
    //    }
    //    #endregion

    //    #region Update Exist Record
    //    [HttpPost]
    //    public string UpdateExistedAssets()
    //    {
    //        string result = string.Empty;
    //        if (Session["MatchData"] != null)
    //        {
    //            var dt = Session["MatchData"] as DataTable;
    //            string xmlrecord = Utility.CommonUtility.ConvertDataTableToXML(dt, "UpdateRecord", "Record", true);
    //            bool SaveResult = _csvService.UpdateExistedAssets(xmlrecord, Base.UserSession.CurrentUser.UserId);
    //            result = SaveResult ? ConstMessage.ImportCSV_UpdateSuccess : ConstMessage.ImportCSV_UpdateFail;
    //        }
    //        return result;
    //    }
    //    #endregion

    //    #endregion

    //    #region Import Asset From CSV File 

    //    [HttpGet]
    //    //[CrxAuthorize(Roles = "Import_ImportAsset")]
    //    public ActionResult ImportAsset()
    //    {
    //        return View();
    //    }

    //    [ActionName("ImportAsset")]
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult ImportAssetFromCSVFile()
    //    {
    //        var dtrecords = new DataTable();
    //        string connString = "";
    //        try
    //        {
    //            if (Request.Files["FileUpload1"].ContentLength > 0)
    //            {
    //                string extension = Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
    //                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
    //                string path = $"{Server.MapPath("~/Content/Uploads")}/{Request.Files["FileUpload1"].FileName}";
    //                if (!Directory.Exists(path))
    //                {
    //                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
    //                }
    //                if (validFileTypes.Contains(extension))
    //                {
    //                    if (System.IO.File.Exists(path))
    //                    { System.IO.File.Delete(path); }
    //                    Request.Files["FileUpload1"].SaveAs(path);
    //                    if (extension == ".csv")
    //                    {
    //                        dtrecords = Utility.CommonUtility.ConvertCsVtoDataTable(path);
    //                        for (int i = 0; i < dtrecords.Columns.Count; i++)
    //                        {
    //                            if (dtrecords.Columns[i].ColumnName.Contains("\""))
    //                                dtrecords.Columns[i].ColumnName = dtrecords.Columns[i].ColumnName.Substring(1, dtrecords.Columns[i].ColumnName.Count() - 2);
    //                        }
    //                        for (int i = 0; i < dtrecords.Rows.Count; i++)
    //                        {
    //                            for (int j = 0; j < dtrecords.Columns.Count; j++)
    //                            {
    //                                if (dtrecords.Rows[i][j].ToString().Contains("\""))
    //                                    dtrecords.Rows[i][j] = dtrecords.Rows[i][j].ToString().Substring(1, dtrecords.Rows[i][j].ToString().Count() - 2);
    //                            }
    //                        }
    //                    }
    //                    //Connection String to Excel Workbook  
    //                    else switch (extension.Trim())
    //                        {
    //                            case ".xls":
    //                                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
    //                                dtrecords = Utility.CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                                break;
    //                            case ".xlsx":
    //                                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
    //                                dtrecords = Utility.CommonUtility.ConvertXSLXtoDataTable(path, connString);
    //                                break;
    //                        }
    //                    ViewBag.Data = dtrecords;
    //                    Session["tableData"] = ViewBag.Data;
    //                }
    //                else
    //                {
    //                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format only";
    //                }
    //            }
    //            else
    //            {
    //                if (Session["tableData"] != null)
    //                {
    //                    dtrecords = (DataTable)Session["tableData"];
    //                }
    //                else
    //                {
    //                    ViewBag.Error = "Please select a file";
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            var routeData = ControllerContext.RouteData.Values;
    //            ErrorLog.LogError(ex, routeData["controller"].ToString(), routeData["action"].ToString());
    //        }
    //        return View(dtrecords);
    //    }


    //    [HttpGet]
    //    public ActionResult ConvertToFloorAssetsData()
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            DataTable dtrecords = (DataTable)Session["tableData"];
    //            var floorAssets = new List<TFloorAssets>();

    //            foreach (DataRow item in dtrecords.Rows)
    //            {
    //                var column = dtrecords.Columns;
    //                // Checking for the mandatory fields
    //                if (!column.Contains("AssetName") || String.IsNullOrEmpty(item["AssetName"].ToString()))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "AssetName is missing or has no text";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }
    //                if (!column.Contains("AssetsCategory") || String.IsNullOrEmpty(item["AssetsCategory"].ToString()))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "AssetName is missing or has no text";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }
    //                if (!column.Contains("AssetsSubCategory") || String.IsNullOrEmpty(item["AssetsSubCategory"].ToString()))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "AssetsSubCategory is missing or has no text";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }
    //                if (!column.Contains("AssetNo") || String.IsNullOrEmpty(item["AssetNo"].ToString()))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "AssetNo is missing or has no text";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }
    //                if (!column.Contains("Status") || String.IsNullOrEmpty(item["Status"].ToString()))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "Status is missing or has no text";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }

    //                string assetName = item["AssetName"].ToString();
    //                string buildingName = GetColumnValue(dtrecords, item, "Building", "0");
    //                string floorName = GetColumnValue(dtrecords, item, "Floor", "0");
    //                string stopName = GetColumnValue(dtrecords, item, "Stops", "0");
    //                string assetsCategory = GetColumnValue(dtrecords, item, "assetsCategory");
    //                string assetsSubCategory = GetColumnValue(dtrecords, item, "AssetsSubCategory");
    //                // get building code
    //                var assetsIds = _floorService.GetAssetsIds(assetName, buildingName, floorName, stopName, assetsCategory, assetsSubCategory);
    //                if (assetsIds.Item6 == "0" || !int.TryParse(assetsIds.Item6, out int resCatId))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "assetsCategory is invalid";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }
    //                if (assetsIds.Item7 == "0" || !int.TryParse(assetsIds.Item7, out int resSubCatId))
    //                {
    //                    ViewBag.MandatoryFieldMissing = "importFailed";
    //                    ViewBag.Message = "assetsSubCategory is invalid";
    //                    return PartialView("_partialFilterRecordAfterImport", ds);
    //                }

    //                var floorAsset = new TFloorAssets
    //                {
    //                    AssetId = Convert.ToInt32(assetsIds.Item1),
    //                    Name = assetName,
    //                    AscId = int.Parse(assetsIds.Item6),
    //                    AssetTypeId = int.Parse(assetsIds.Item7),   // Asset Sub Category
    //                    DeviceNo = GetColumnValue(dtrecords, item, "AssetNo", "0"),
    //                    SerialNo = GetColumnValue(dtrecords, item, "BarcodeNo", "0"),
    //                    Rating = GetColumnValue(dtrecords, item, "Rating"),
    //                    Model = GetColumnValue(dtrecords, item, "Model"),
    //                    NearBy = GetColumnValue(dtrecords, item, "NearBy"),
    //                    Description = GetColumnValue(dtrecords, item, "Description"),
    //                    BuildingCode = assetsIds.Item5,   //GetColumnValue(dtrecords, item, "Building"),
    //                    FloorId = Convert.ToInt32(assetsIds.Item3),
    //                    Stop = new StopMaster
    //                    {
    //                        StopCode = assetsIds.Item4,
    //                        StopName = GetColumnValue(dtrecords, item, "Stops")
    //                    },
    //                    StatusCode = GetAssetStatus(GetColumnValue(dtrecords, item, "Status")),
    //                    Source = (int)Enums.Source.Internal,
    //                    CreatedBy = UserSession.CurrentUser.UserId,
    //                    DateCreated = DateTime.Today,
    //                    BuildingID = Convert.ToInt32(assetsIds.Item2),
    //                    BuildingName = GetColumnValue(dtrecords, item, "Building"),
    //                    FloorName = GetColumnValue(dtrecords, item, "Floor"),
    //                    BuildingTypeId = GetColumnValue(dtrecords, item, "BuildingTypeId"),
    //                    SiteCode = GetColumnValue(dtrecords, item, "SiteCode"),
    //                    WallRating = GetColumnValue(dtrecords, item, "WallRating"),
    //                    DoorRating = GetColumnValue(dtrecords, item, "DoorRating"),
    //                    FrameRating = GetColumnValue(dtrecords, item, "FrameRating"),
    //                    AssetCategoryName = GetColumnValue(dtrecords, item, "assetsCategory"),
    //                    AssetSubCategory = new AssetsSubCategory { AscName = GetColumnValue(dtrecords, item, "AssetsSubCategory") },
    //                    LocationName = GetColumnValue(dtrecords, item, "LocationName"),
    //                    Make = new Manufactures { ManufactureName = GetColumnValue(dtrecords, item, "Make") }

    //                };
    //                floorAssets.Add(floorAsset);
    //            }

    //            string Uri = string.Empty;
    //            Uri = APIUrls.Assets_AddImportedFloorAssets; // Assets/ddImportedFloorAssets
    //            foreach (var floorAsset in floorAssets)
    //            {
    //                string postData = Base.Common.JsonSerialize<List<TFloorAssets>>(floorAsset);
    //                int statusCode = Convert.ToInt32(Enums.HttpReponseStatusCode.Success);
    //                var items = _httpService.CallPostMethod(postData, Uri, ref statusCode);

    //                var httpResponse = Base.Common.JsonDeserialize<HttpResponseMessage>(items);
    //                ViewBag.Message = httpResponse.Message;
    //                var files = httpResponse.Result != null ? httpResponse.Result.TFloorAssetItem : null;

    //                var ds1 = JsonConvert.DeserializeObject<DataSet>(files.AssetDataset);

    //                if (ds.Tables.Count == 0)
    //                {
    //                    ds = ds1;
    //                    ds.Tables[1].TableName = "NotMatchData";
    //                    ds.Tables[2].TableName = "MatchData";
    //                    ds.Tables[3].TableName = "NewData";
    //                    // adding only those rows which have value so restting empty rows
    //                    if (CheckDataTableEmptyValue(ds.Tables["NotMatchData"])) { ds.Tables["NotMatchData"].Clear(); }
    //                    // if (CheckDataTableEmptyValue(ds.Tables["MatchData"])) { ds.Tables["NotMatchData"].Clear(); }    // This tbl will always have asset category and sub category
    //                    if (CheckDataTableEmptyValue(ds.Tables["NewData"])) { ds.Tables["NewData"].Clear(); }

    //                    var dr = ds.Tables["MatchData"].Rows[0];
    //                    dr[2] = floorAsset.AssetCategoryName;
    //                    dr[3] = floorAsset.AssetSubCategory.AscName;
    //                }
    //                else
    //                {
    //                    var dr = ds1.Tables["MatchData"].Rows[0];
    //                    dr[2] = floorAsset.AssetCategoryName;
    //                    dr[3] = floorAsset.AssetSubCategory.AscName;

    //                    // adding only those rows which have value 
    //                    if (!CheckDataTableEmptyValue(ds1.Tables["NotMatchData"]))
    //                    { ds.Tables["NotMatchData"].ImportRow(ds1.Tables["NotMatchData"].Rows[0]); }
    //                    ds.Tables["MatchData"].ImportRow(ds1.Tables["MatchData"].Rows[0]);
    //                    if (!CheckDataTableEmptyValue(ds1.Tables["NewData"]))
    //                    { ds.Tables["NewData"].ImportRow(ds1.Tables["NewData"].Rows[0]); }
    //                }
    //                ds1 = new DataSet();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            var routeData = ControllerContext.RouteData.Values;
    //            ErrorLog.LogError(ex, routeData["controller"].ToString(), routeData["action"].ToString());
    //            ViewBag.Message = (ex.GetType().Name == "NullReferenceException") ? "There is something wrong in the import data" : ex.Message;
    //        }

    //        ViewBag.MandatoryFieldMissing = "importSuccess";
    //        return PartialView("_partialFilterRecordAfterImport", ds);
    //    }
    //    /// <summary>
    //    /// gets the value of a column in a particular record of datable
    //    /// </summary>
    //    private string GetColumnValue(DataTable dt, DataRow dr, string columnName, string DefaultReturnValue = "")
    //    {
    //        string value = "";
    //        if (dt.Columns.Contains(columnName) && dr[columnName] != null)
    //        {
    //            value = String.IsNullOrWhiteSpace(dr[columnName].ToString()) ? "" : dr[columnName].ToString();
    //            value = value.Trim();

    //        }
    //        if (String.IsNullOrWhiteSpace(value))
    //        {
    //            value = DefaultReturnValue;
    //        }
    //        return value;
    //    }

    //    private String GetAssetStatus(string status)
    //    {
    //        string statusCode = "";
    //        switch (status)
    //        {
    //            case "ACTIVE -In Use":
    //                statusCode = "ACTIV";
    //                break;
    //            case "INACT -In Inventory":
    //                statusCode = "INACT";
    //                break;
    //            case "RETIR -Obsolete":
    //                statusCode = "RETIR";
    //                break;
    //            default:
    //                statusCode = "ACTIV";
    //                break;
    //        }
    //        return statusCode;
    //    }

    //    private bool CheckDataTableEmptyValue(DataTable dt)
    //    {
    //        bool isEmpty = true;
    //        var row = dt.Rows[0];
    //        foreach (var cell in row.ItemArray)
    //        {
    //            if (cell != null)
    //            {
    //                var cellVal = cell.ToString();
    //                if (!String.IsNullOrWhiteSpace(cellVal)) { isEmpty = false; break; }
    //            }
    //        }
    //        return isEmpty;
    //    }
    //    #endregion

    //    //public ActionResult ImportSiemensFile()
    //    //{
    //    //    return View();
    //    //}

    //    //[HttpPost]
    //    //public ActionResult ImportSiemensFile(string url = "")
    //    //{
    //    //    try
    //    //    {
    //    //        if (String.IsNullOrEmpty(url))
    //    //        {
    //    //            url = "https://s3.amazonaws.com/111347/files/tfilespath/637257257356349016/0b042100-7b2f-4908-8ef0-700b969edf60.csv";
    //    //        }
    //    //        string csvPath = Server.MapPath("~/Content/Uploads");
    //    //        SaveData saveData = new SaveData();
    //    //        var fileUploadRes = saveData.SaveRecords(url, csvPath);
    //    //    }
    //    //    catch (Exception ex)
    //    //    { }

    //    //    return View();
    //    //}
    //}
}