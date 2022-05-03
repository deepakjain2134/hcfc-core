using HCF.BAL.Interfaces.Services;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HCF.BAL.Services.JobService
{

    public partial class RoundRecuringJobService: IRoundRecuringJobService
    {
        private readonly ILoggingService _loggingService;
        private readonly IRoundsService _roundService;
        private readonly IOrganizationRepository _organizationRepository;
        public RoundRecuringJobService(ILoggingService loggingService, IRoundsService roundService, IOrganizationRepository organizationRepository)
        {
            _loggingService = loggingService;
            _roundService = roundService;
            _organizationRepository = organizationRepository;
        }

        public void SetArchiveRound()
        {
            _loggingService.Error($"App Started");
            ArchivRounds();
        }

        private void ArchivRounds()
        {
            //var IsArchived = _roundService.SetArchiveRound();
            var organizations = _organizationRepository.GetMasterOrganization().Where(x => !string.IsNullOrEmpty(x.DbName) && x.IsActive).ToList();
            foreach (var item in organizations)
            {
                _loggingService.Error(item.DbName + " " + DateTime.Now);

                var IsArchived = _roundService.SetArchiveRound(item.DbName);
            }
        }
    }
}

