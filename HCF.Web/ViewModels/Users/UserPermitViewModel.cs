using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace HCF.Web.ViewModels.Users
{
    public class UserPermitViewModel
    {
        public int? VendorId { get; set; }

        public int? UserId { get; set; }

        public List<SelectListItem> PermitTypes { get; set; }

        public long[] SelectedPermitTypes { get; set; }
    }
}