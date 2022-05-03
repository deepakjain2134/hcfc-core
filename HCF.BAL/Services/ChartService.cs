using HCF.DAL;
using System.Data;

namespace HCF.BAL
{
    public class ChartService :IChartService
    {
        private readonly IChartRepository _chartRepository;

        public ChartService(IChartRepository chartRepository)
        {
            _chartRepository = chartRepository;
        }

        public DataTable GetAssetChart(int days)
        {
            return _chartRepository.GetAssetChart(days);
        }

        public DataSet GetAllOrganizationChart()
        {
            return _chartRepository.GetAllOrganizationChart();
        }

        public DataTable GetEpStatusChart()
        {
            return _chartRepository.GetEpStatusChart();
        }

        public DataTable GetAssetInventoryReport()
        {
            return _chartRepository.GetAssetInventoryReport();
        }

    }
}
