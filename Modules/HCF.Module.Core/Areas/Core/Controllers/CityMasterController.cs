using HCF.Infrastructure.Data;
using HCF.Module.Core.Areas.Core.ViewModels.CityMaster;
using HCF.Module.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HCF.BDO;

namespace HCF.Module.Core.Areas.Core.Controllers
{
    [Area("Core")]
    [Authorize]
    public class CityMasterController : Controller
    {
        private readonly IRepository<CityMaster> _cityMasterRepository;
        private readonly IWorkContext _workContext;

        public CityMasterController(IRepository<CityMaster> cityMasterRepository, IWorkContext workContext)
        {
            _cityMasterRepository = cityMasterRepository;
            _workContext = workContext;
        }

        [Route("cities")]
        public async Task<IActionResult> Index()
        {
            var model = await _cityMasterRepository
                .Query()
                .Select(x => new CityMasterViewModel
                {
                    CityCode = x.CityCode,
                    CityName = x.CityName,
                    StateId = x.StateId,
                    StateName = x.StateMaster.StateName,
                    IsActive = x.IsActive
                }).ToListAsync();

            return View(model);
        }
    }
}
