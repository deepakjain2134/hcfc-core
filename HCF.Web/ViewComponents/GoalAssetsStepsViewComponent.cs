using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class GoalAssetsStepsViewComponent :ViewComponent
    {
        private readonly IEpService _epService;
        private readonly ITransactionService _transactionService;
        private readonly IInsActivityService _inspectionActivityService;       
        private readonly IBuildingsService _buildingsService;

        public GoalAssetsStepsViewComponent(IEpService epService,
            ITransactionService transactionService,
            IBuildingsService buildingsService,
            IInsActivityService inspectionActivityService)
        {
            _buildingsService = buildingsService;
            _inspectionActivityService = inspectionActivityService;
            _epService = epService;
            _transactionService = transactionService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int epId, int? inspectionId, int? floorAssetId, bool? showEp, int? frequencyId, string pagemode, int? inspectionGroupId, bool isPopUp = false)
        {
            List<EPSteps> epsteps;
            int _frequencyId = frequencyId ?? 0;
            ViewBag.frequencyId = _frequencyId;
            int _inspectionId = inspectionId ?? 0;
            ViewBag.inspectionId = _inspectionId;
            var userid = UserSession.CurrentUser.UserId;
            if (floorAssetId.HasValue)
            {
                int mode = Convert.ToInt32(BDO.Enums.Mode.ASSET);
                epsteps = _transactionService.GetGoalStepsByActivity(floorAssetId, 0, epId, inspectionId ?? 0, mode, _frequencyId, inspectionGroupId);
                foreach (var item in epsteps)
                {
                    if (item.Inspection != null)
                        item.IsCurrentCycle = Convert.ToInt32(item.Inspection.IsCurrent);
                    if (item.TInspectionActivity == null)
                        item.TInspectionActivity = new TInspectionActivity();
                    if (item.CurrentStatus != "NA")
                        item.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(epsteps.FirstOrDefault().InspectionId).Where(x => (x.DocTypeId != 108)).OrderByDescending(x => x.CreatedDate).FirstOrDefault(x => x.IsCurrent);
                    item.IsInspReady = 1;
                    item.ActivityType = mode;
                    if (_frequencyId > 0)
                        item.FrequencyId = _frequencyId;
                    if (item.InspectionFiles == null)
                        item.InspectionFiles = CreateInspectionFiles();
                    if (item.Comment != null)
                        item.Comment = epsteps.FirstOrDefault().Comment;
                    item.Campus = _buildingsService.GetCampus(userid,epId).ToList();
                }
            }
            else
            {
                int mode = Convert.ToInt32(BDO.Enums.Mode.EP);
                epsteps = _transactionService.GetGoalStepsByActivity(null, null, epId, inspectionId ?? 0, mode, _frequencyId, inspectionGroupId);
                foreach (var item in epsteps)
                {
                    if (item.Inspection != null)
                        item.IsCurrentCycle = Convert.ToInt32(item.Inspection.IsCurrent);
                    if (item.TInspectionActivity == null)
                        item.TInspectionActivity = new TInspectionActivity();
                    if (item.EPDetails.Inspection != null && item.EPDetails.Inspection.SubStatus != "NA")
                        item.TInspectionActivity = _inspectionActivityService.GetAllInspectionActivity(epsteps.FirstOrDefault().InspectionId).Where(x => (x.DocTypeId != 108)).OrderByDescending(x => x.CreatedDate).FirstOrDefault(x => x.IsCurrent);
                    item.ActivityType = mode;
                    if (_frequencyId > 0)
                        item.FrequencyId = _frequencyId;
                    else
                        ViewBag.frequencyId = item.FrequencyId;
                    if (item.InspectionFiles == null)
                        item.InspectionFiles = CreateInspectionFiles();
                    if (item.Comment != null)
                        item.TInspectionActivity.Comment = item.Comment;
                    item.Campus = _buildingsService.GetCampus(userid,epId).ToList();

                }
            }
            ViewBag.showEp = (showEp != null);
            ViewBag.isPopUp = isPopUp;

            if (epsteps != null && epsteps.Count > 0 && epsteps.FirstOrDefault().FloorAssetId.HasValue && epsteps.SingleOrDefault().MainGoal.Any(x => x.Steps.Any(y => y.StepType == 2)))
            {
                epsteps.FirstOrDefault().InspectionActivities = _transactionService.GetInspections(epsteps.FirstOrDefault().FloorAssetId.Value, epsteps.SingleOrDefault().EPDetailId).OrderByDescending(x => x.ActivityInspectionDate).Take(4).ToList();
            }
            epsteps.FirstOrDefault().epdocument = _transactionService.GetInspectionDocs(epsteps.SingleOrDefault().InspectionId).ToList();
            epsteps.FirstOrDefault().epdetials = _epService.GetEpShortDescription(epId);
            return await Task.FromResult(View(epsteps));
        }

        public static List<TInspectionFiles> CreateInspectionFiles()
        {
            var inspectionfiles = new List<TInspectionFiles>();
            var inspection = new TInspectionFiles
            {
                FileType = "C",
            };
            var inspection1 = new TInspectionFiles
            {
                FileType = "D",
            };
            inspectionfiles.Add(inspection);
            inspectionfiles.Add(inspection1);
            return inspectionfiles;
        }
    }
}