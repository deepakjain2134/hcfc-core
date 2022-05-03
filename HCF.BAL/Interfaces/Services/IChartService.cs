using System.Data;

namespace HCF.BAL
{
    public interface IChartService
    {
        DataSet GetAllOrganizationChart();
        DataTable GetAssetChart(int days);
        DataTable GetAssetInventoryReport();
        DataTable GetEpStatusChart();
    }
}