using HCF.BAL;
using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.Web.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class ManufactureController : BaseController
    {
        private readonly IManufactureService _manufactureService;
        public ManufactureController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }

        #region CRUD

        public ActionResult Manufactures()
        {
            UISession.AddCurrentPage("CRxSettings_manufacturer", 0, "Manufactures");
            var list = _manufactureService.GetAll();
            return View(list);
        }


        public ActionResult mngManufacture(int? mid)
        {
            Manufactures manufacture = new Manufactures();
            if (mid.HasValue)
                manufacture = _manufactureService.Get(mid.Value);

            return View(manufacture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult mngManufacture(Manufactures manufactures)
        {

            if (ModelState.IsValid)
            {

                manufactures.CreatedBy = Base.UserSession.CurrentUser.UserId;

                int manufactId = _manufactureService.Save(manufactures);
                if (manufactId > 0)
                {
                    SuccessMessage = Utility.ConstMessage.Insert_Manufacture_Success;
                    return RedirectToAction("Manufactures");
                }

            }
            return View(manufactures);
        }

        #endregion
    }
}