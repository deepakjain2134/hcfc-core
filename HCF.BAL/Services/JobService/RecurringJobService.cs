using HCF.BAL.Interfaces.Services;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCF.BAL.Services.JobService
{

    public partial class RecurringJobService
    {
        private readonly ILoggingService _loggingService;
        private readonly IOrganizationRepository _organizationRepository;
        public RecurringJobService(ILoggingService loggingService, IOrganizationRepository organizationRepository)
        {
            _loggingService = loggingService;
            _organizationRepository = organizationRepository;
        }

        public void SendMarkAsSolutionReminders()
        {
            _loggingService.Error($"App Started");
            GeneratelogSheet();
        }

        private void GeneratelogSheet()
        {
            var organizations = _organizationRepository.GetMasterOrganization().Where(x => !string.IsNullOrEmpty(x.DbName) && x.IsActive).ToList();
            foreach (var item in organizations)
            {
                _loggingService.Error(item.Name + " " + DateTime.Now);
            }
        }
    }
}
