using HCF.BAL.Interfaces.Services;
using HCF.BAL.Services;
using HCF.BDO.ModuleTraining;
using HCF.Module.Core.Areas.Core.Controllers;
using HCF.Module.Core.Extensions;
using HCF.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Module.ModuleTraining.Areas.ModuleTraining.Controllers
{
    [Authorize]
    [Area("ModuleTraining")]
    public class ModuleTrainingController : BaseController
    {
        #region  ctor
        private readonly INavigationSession _uISession;
        private readonly IWorkContext _workContext;
        private readonly ITrainingSessionService _trainingSessionService;

        public ModuleTrainingController(INavigationSession uISession, ITrainingSessionService trainingSessionService, IWorkContext workContext)
        {
            _uISession = uISession;
            _trainingSessionService = trainingSessionService;
            _workContext = workContext;

        }

        #endregion

        [HttpGet("module/training")]
        public async Task<IActionResult> ModuleTrainingLists()
        {
            var currentUser = await _workContext.GetCurrentUser();
            _uISession.AddCurrentPage("ModuleTraining_Index", 0, "Implementation Status");
            var items = _trainingSessionService.GetTrainingSessions(Convert.ToInt32(currentUser.ClientNo)).Where(x => x.IsActive);
            return View(items);
        }

        public IActionResult UpdateTrainingStatus(int modeuleSessionId)
        {
            var currentUser = _workContext.GetCurrentUser();

            var orgTrainingSession = new OrgTrainingSessions();
           
            // var orgKey = currentUser.Result.Orgkey;
            var ClientNo = currentUser.Result.ClientNo;
            var sessionMaster = _trainingSessionService.GetTrainingSessions((Convert.ToInt32(ClientNo)), modeuleSessionId).FirstOrDefault(x => x.ModuleSessionId == modeuleSessionId);


            if (sessionMaster.TrainingSessions != null)
            {
                orgTrainingSession = sessionMaster.TrainingSessions.FirstOrDefault();
            }

           

            if (orgTrainingSession == null)
            {
                orgTrainingSession = new OrgTrainingSessions();                
            }

            if (sessionMaster != null)
                orgTrainingSession.TrainingSessionMaster = sessionMaster;

            orgTrainingSession.ModuleSessionId = modeuleSessionId;

            return PartialView("UpdateTrainingStatus", orgTrainingSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTrainingStatus(OrgTrainingSessions model)
        {
            var currentUser = _workContext.GetCurrentUser();
           
            model.CreatedBy = currentUser.Result.UserId;
            model.ClientNo = currentUser.Result.ClientNo.Value;

            var status = _trainingSessionService.SaveOrgTrainingSession(model);
            if (status > 0)
            {
                SuccessMessage = Utility.ConstMessage.Saved_Successfully;
            }
            else
            {
                SuccessMessage = Utility.ConstMessage.Internal_Server_Error;
            }
            var items = _trainingSessionService.GetTrainingSessions(Convert.ToInt32(model.ClientNo)).Where(x => x.IsActive);
            return RedirectToAction("ModuleTrainingLists",items);
        }

        [HttpGet("moduletraining")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _workContext.GetCurrentUser();
            _uISession.AddCurrentPage("ModuleTraining_Index", 0, "Manage Implementation Status");
            var items = _trainingSessionService.GetTrainingSessions(Convert.ToInt32(currentUser.ClientNo));
            return View(items);
        }

        [HttpGet("create/ModuleTraining")]
        public async Task<IActionResult> AddModuleTraining(int? id)
        {
            var currentUser = await _workContext.GetCurrentUser();
            _uISession.AddCurrentPage("ModuleTraining_Index", 0, (id > 0) ? "Update Implementation Status" : "Add Implementation Status");
            TrainingSessionMaster model = new TrainingSessionMaster();
            if (id.HasValue)
                model = _trainingSessionService.GetTrainingSessions(Convert.ToInt32(currentUser.ClientNo)).FirstOrDefault(x => x.ModuleSessionId == id);
            return View(model);
        }

        [HttpPost("create/ModuleTraining")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModuleTraining(TrainingSessionMaster model)
        {
            var currentUser = await _workContext.GetCurrentUser();
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Description))
                {
                    model.CreatedBy = currentUser.UserId;
                    if (model.ModuleSessionId > 0)
                    {
                        _trainingSessionService.UpdateData(model);
                        SuccessMessage = Utility.ConstMessage.Module_Training;
                    }
                    else
                    {
                        _trainingSessionService.AddModuleTraining(model);
                        SuccessMessage = Utility.ConstMessage.Saved_Successfully;
                    }
                    return RedirectToAction("Index");
                }
                else
                    ErrorMessage = "Please enter description";
            }
            return View(model);
        }
    }
}


