using System.Collections.Generic;

namespace HCF.BDO
{
    public class HighChart
    {
        public ChartX ChartX { get; set; }
        public List<AssetSeries> AssetSeries { get; set; }

        public List<Series> Series { get; set; }

        public EPStatusSeries EPStatusSeries { get; set; }

        public AssetInventorySeries AssetInventorySeries { get; set; }      

        public string title { get; set; }

    }


    public class AssetSeries
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public string color { get; set; }
    } 


    public class Series
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public string color { get; set; }
    }

    public class AssetInventorySeries
    {
        public List<InventoryData> data { get; set; }
    }

    public class EPStatusSeries
    {
        public List<InventoryData> data { get; set; }
    }

    public struct InventoryData
    {
        public string name { get; set; }
        public int y { get; set; }
    }


    public class ChartX
    {
        public List<string> categories { get; set; }
    }

}