using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.ViewModels.Round;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCF.Web.ViewComponents
{
   
    public class RoundViewAssignmentViewComponent : ViewComponent
    {
        private readonly IRoundInspectionsService _roundInspectionService;

        private readonly IUserService _userService;
        public RoundViewAssignmentViewComponent(IRoundInspectionsService roundInspectionService, IUserService userService)
        {
            _roundInspectionService = roundInspectionService;
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new RoundViewModel
            {
                userList = _userService.GetUsers().Where(x => x.IsActive && x.IsUserRole(BDO.Enums.UserRole.RoundVolunteer)).ToList(),
                Roundgrouplist = _roundInspectionService.GetRoundGroupList().Where(x => x.IsActive == 1).ToList(),
            };
            model.UserSelectPicker = new ViewModels.Common.CustomSelectPicker("ddlusers", "All Volunteers", "0", Convert.ToString(Base.UserSession.CurrentUser.UserId))
            {
                Items = model.userList.Select(x =>
                                        new SelectListItem()
                                        {
                                            Text = x.FullName,
                                            Value =Convert.ToString(x.UserId)
                                        }).ToList()
            };

            return await Task.FromResult(View(model));
        }
    }
}
