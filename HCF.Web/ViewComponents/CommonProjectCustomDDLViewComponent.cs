using HCF.BAL;
using HCF.BDO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCF.Web.ViewComponents
{
    public class CommonProjectCustomDDLViewComponent : ViewComponent
    {
        #region ctor

        private readonly ITIcraProjectService _IcraProjectService;
        public CommonProjectCustomDDLViewComponent(ITIcraProjectService IcraProjectService)
        {
            _IcraProjectService = IcraProjectService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(string type, string ProjectId)
        {
            int parentProjectId = 0;
            var projectList = new List<TIcraProject>();
            if (!string.IsNullOrEmpty(ProjectId))
            {
                if (ProjectId.Contains(","))
                {
                    projectList = _IcraProjectService.GetAllActiveTIcraProject().Where(x => x.ParentProjectId == null && x.Status).ToList();
                }
                else
                {
                    var projectparent = _IcraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ProjectId == Convert.ToInt32(ProjectId) && x.ParentProjectId != null).Select(x => x.ParentProjectId).FirstOrDefault();

                    if (projectparent != null)                    
                        parentProjectId = projectparent.Value;                    
                    projectList = _IcraProjectService.GetAllActiveTIcraProject(false).Where(x => x.ParentProjectId == null && (x.Status || x.ProjectId == Convert.ToInt32(ProjectId) || x.ProjectId == parentProjectId)).ToList();
                }

            }
            ViewBag.type = type;
            ViewBag.ProjectId = ProjectId;
            return await Task.FromResult(View(projectList));
        }
    }
}