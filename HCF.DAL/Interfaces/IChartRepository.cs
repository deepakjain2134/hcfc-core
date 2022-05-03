using System.Data;

namespace HCF.DAL
{
    public interface IChartRepository
    {
        DataSet GetAllOrganizationChart();
        DataTable GetAssetChart(int days);
        DataTable GetAssetInventoryReport();
        DataTable GetEpStatusChart();
        DataTable GetWorkOrdersChart(int days, int userId);
    }
}