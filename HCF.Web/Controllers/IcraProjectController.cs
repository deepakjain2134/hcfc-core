using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Utility;
using HCF.Web.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Controllers  
{
    [Authorize]
    public class IcraProjectController : BaseController
    {       
        private readonly ITIcraProjectService _icraProjectService;
        private readonly ICommonModelFactory _commonModelFactory;

        public IcraProjectController(ICommonModelFactory commonModelFactory,
            ITIcraProjectService icraProjectService) 
        {
            _commonModelFactory = commonModelFactory;
            _icraProjectService = icraProjectService;
        }

        public async Task<ActionResult> Index()
        {
            UISession.AddCurrentPage("IcraProject_Index", 0, "Projects");
            var model = await _icraProjectService.GetAllTIcraProject();
            model = model.Where(x => x.ParentProjectId == null).ToList();
            return View(model);
        }

        public async Task<ActionResult> ChildIcraProject(int projectId, int treeLevel = 0)
        {
            ViewBag.ParentId = projectId;
            ViewBag.TreeLevel = treeLevel;
            List<TIcraProject> model = await _icraProjectService.GetAllTIcraProject();
            model = model.Where(x => x.ParentProjectId == projectId).ToList();
            return View("_childIcraProject", model);
        }

        public async Task<ActionResult> AddOrEdit(int? parentProjectId, int id = 0)
        {
            UISession.AddCurrentPage("IcraProject_AddOrEdit", 0, "Manage Project");

            var model = new TIcraProject();

            if (parentProjectId.HasValue)
            {
                model = await _icraProjectService.GetTIcraProject(parentProjectId.Value);
                if (model != null)
                {
                    model.Status = true;
                    model.ProjectNumber = model.ProjectNumber + "_" + _commonModelFactory.GetAlphabeticFromIndex(model.SubProject.Count + 1);
                    model.ParentProjectId = parentProjectId;
                    model.ProjectId = 0;
                    model.ProjectName = "";
                    //model = new TIcraProject
                    //{
                    //    Status = true,
                    //    ProjectNumber = model.ProjectNumber + "_" + Base.Common.GetAlphabeticFromIndex(model.SubProject.Count + 1),
                    //    ParentProjectId = parentProjectId
                    //};
                }
                else
                {
                    model = new TIcraProject
                    {
                        Status = true,
                        ParentProjectId = parentProjectId
                    };
                }
            }

            if (id > 0)
            {
                model = await _icraProjectService.GetTIcraProject(id);
            }
            //ViewBag.ConstProjects = await _icraProjectService.GetAllTIcraProject().ConfigureAwait(false);            
            model.icraProjectList =await _icraProjectService.GetAllTIcraProject();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrEdit(TIcraProject model)
        {
            model.CreatedBy = UserSession.CurrentUser.UserId;
            var data = await _icraProjectService.GetAllTIcraProject();
            if (model.ProjectId == 0 && data.Any(x => x.ProjectNumber == model.ProjectNumber))
            {
                ErrorMessage = ConstMessage.IcraProjectAlreadyExists;
                return View(model);
            }

            var mode = model.ProjectId > 0 ? 2 : 1;
            var tFileIds = new List<Int32>();
            if (!string.IsNullOrEmpty(model.AttachFiles))
            {
                foreach (var item in model.AttachFiles.Split(','))
                {
                    int fileId = Convert.ToInt32(item.Split('_')[0]);
                    tFileIds.Add(fileId);
                }

                if (!string.IsNullOrEmpty(model.TFileIds))
                {
                    var file = string.Join(",", tFileIds.Select(n => n.ToString()).ToArray());
                    model.TFileIds = model.TFileIds + ',' + file;
                }
                else
                {
                    model.TFileIds = string.Join(",", tFileIds.Select(n => n.ToString()).ToArray());
                }

            }


            var returnId = _icraProjectService.CrudTIcraProject(model, mode);
            if (returnId > 0)
                SuccessMessage = model.ProjectId > 0 ? ConstMessage.Project_Updated_Successfully : ConstMessage.Project_Created_Successfully;

            //ViewBag.ConstProjects = await _icraProjectService.GetAllTIcraProject().ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var model = await _icraProjectService.GetTIcraProject(id);
            var returnId = _icraProjectService.CrudTIcraProject(model, 3);
            if (returnId > 0)
            {
                SuccessMessage = ConstMessage.Project_Deleted_Successfully;
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteProjectDrawing(int projectId, string fileIds)
        {
            var result = _icraProjectService.DeleteProjectFiles(projectId, fileIds, 2);
            return Json(result);
        }
        public JsonResult DeleteProjectFiles(int projectId, string fileIds)
        {
            var result = _icraProjectService.DeleteProjectFiles(projectId, fileIds, 1);
            return Json(result);
        }
    }
}