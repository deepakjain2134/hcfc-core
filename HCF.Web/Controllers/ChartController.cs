using System;
using System.Collections.Generic;
using System.Linq;
using HCF.Utility;
using HCF.BDO;
using System.Data;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.Web.Base;

namespace HCF.Web.Controllers
{
    [Authorize]
    public class ChartController : BaseController
    {
        private readonly IChartService _chartService;       
        private readonly ICommonModelFactory _commonModelFactory;

        public ChartController(IChartService chartService, ICommonModelFactory commonModelFactory)
        {         
            _chartService = chartService;
            _commonModelFactory = commonModelFactory;

        }

        // GET: Chart
        public ActionResult AssetChart()
        {
            var objhigchart = new HighChart();
            var objChartx = new ChartX();
            var objSeries = new List<AssetSeries>();
            objChartx.categories = new List<string>();
            DataTable dt = new DataTable();
            int days = 7;
            dt = _chartService.GetAssetChart(days);
            for (int i = days - 1; i >= 0; i--)
            {
                objChartx.categories.Add(DateTime.Now.AddDays(-i).ToFormatDate());
            }
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                var objs = new AssetSeries { name = dt.Columns[j].ColumnName, data = new List<int>() };
                for (int k = 0; k < objChartx.categories.Count; k++)
                {
                    var rows = from row in dt.AsEnumerable() where Convert.ToDateTime(row.Field<string>("ActivityInspectionDate")).ToFormatDate() == objChartx.categories[k].ToString() select row;
                    if (rows != null && rows.Any())
                    {
                        if (!string.IsNullOrEmpty(rows.FirstOrDefault()[j].ToString()))
                        {
                            objs.data.Add(Convert.ToInt32(rows.FirstOrDefault()[j]));
                        }
                    }
                    else { objs.data.Add(0); }
                }
                objSeries.Add(objs);
            }
            objhigchart.ChartX = objChartx;
            objhigchart.AssetSeries = objSeries;
            var res = _commonModelFactory.JsonSerialize<HighChart>(objhigchart);
            ViewBag.AssetChart = res;
            return PartialView();
        }

        public ActionResult AssetInventoryReport()
        {           
            HighChart objhigchart = new();
            AssetInventorySeries objAssetInventorySeries = new AssetInventorySeries { data = new List<InventoryData>() };
            DataTable dt = new();
            dt = _chartService.GetAssetInventoryReport();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var obj = new InventoryData
                {
                    name = dt.Columns[i].ColumnName,
                    y = Convert.ToInt32(dt.Rows[0][i].ToString())
                };
                objAssetInventorySeries.data.Add(obj);
            }
            objhigchart.AssetInventorySeries = objAssetInventorySeries;
            var res = _commonModelFactory.JsonSerialize<HighChart>(objhigchart);
            ViewBag.AssetInventoryChart = res;
            return PartialView();
        }

        public ActionResult AllOrganizationChart()
        {
            var objhigchart = new HighChart();
           
            ChartX objChartx = new ChartX();
            List<Series> objSeries = new List<Series>();
            objChartx.categories = new List<string>();
            DataSet ds = new DataSet();
            DataSet allds = _chartService.GetAllOrganizationChart();
            if (Base.UserSession.CurrentOrg != null && allds != null)
            {
                var parentOrgKey = Base.UserSession.CurrentOrg.ParentOrgKey.ToString();
                for (int i = 0; i < allds.Tables.Count; i++)
                {
                    if (allds.Tables[i].Columns.Contains("OrgKey"))
                    {
                        string strExpr;
                        if (!string.IsNullOrEmpty(parentOrgKey))
                        {
                            strExpr = "OrgKey = '" + Base.UserSession.CurrentOrg.Orgkey + "'" + "AND ParentOrgKey = '" + parentOrgKey + "'";
                        }
                        else
                        {
                            strExpr = "OrgKey = '" + Base.UserSession.CurrentOrg.Orgkey + "'";
                        }
                        var dv = allds.Tables[i].DefaultView;
                        dv.RowFilter = strExpr;
                        var newDT = dv.ToTable();
                        if (newDT.Rows.Count > 0)
                            ds.Tables.Add(newDT);
                    }
                }
            }
            objSeries.Add(new Series { name = "Work Orders" });
            objSeries.Add(new Series { name = "Deficiencies" });
            objSeries.Add(new Series { name = "Risk Assessment" });
            objSeries.Add(new Series { name = "ILSM" });


            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].Rows.Count > 0)
                    objChartx.categories.Add(ds.Tables[i].Rows[0][0].ToString());
            }


            int jj = 1;
            foreach (var item in objSeries)
            {
                item.data = new List<int>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    if (ds.Tables[i].Rows.Count > 0)
                        item.data.Add(Convert.ToInt32(ds.Tables[i].Rows[0][jj]));
                }
                jj++;
            }

            objhigchart.ChartX = objChartx;
            objhigchart.Series = objSeries;
            var res = _commonModelFactory.JsonSerialize<HighChart>(objhigchart);
            ViewBag.AllOrgChart = res;
            return PartialView();
        }

        public ActionResult EpStatusChart()
        {
            var objhigchart = new HighChart();
            
            EPStatusSeries objEPStatusSeries = new EPStatusSeries { data = new List<InventoryData>() };
            DataTable dt = _chartService.GetEpStatusChart();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var obj = new InventoryData
                {
                    name = dt.Columns[i].ColumnName,
                    y = Convert.ToInt32(dt.Rows[0][i].ToString())
                };
                objEPStatusSeries.data.Add(obj);
            }           
            objhigchart.EPStatusSeries = objEPStatusSeries;
            var res = _commonModelFactory.JsonSerialize<HighChart>(objhigchart);
            ViewBag.EPStatusChart = res;
            return PartialView();
        }

        public ActionResult WorkOrders()
        {
            var objhigchart = new HighChart();
            ChartX objChartx = new ChartX();
            List<Series> objSeries = new List<Series>();
            objChartx.categories = new List<string>();
            DataTable dt = new DataTable();
            int days = 10;
            for (int i = days - 1; i >= 0; i--)
            {
                objChartx.categories.Add(DateTime.Now.AddDays(-i).ToString("yyyy/MM/dd"));
            }

            for (int j = 1; j < dt.Columns.Count; j++)
            {
                var objs = new Series { name = dt.Columns[j].ColumnName, data = new List<int>() };
                for (int k = 0; k < objChartx.categories.Count; k++)
                {
                    var rows = from row in dt.AsEnumerable() where row.Field<string>("ActivityInspectionDate") == objChartx.categories[k].ToString() select row;
                    if (rows != null && rows.Any())
                    {
                        if (!string.IsNullOrEmpty(rows.FirstOrDefault()[j].ToString()))
                        {
                            objs.data.Add(Convert.ToInt32(rows.FirstOrDefault()[j]));
                        }
                    }
                    else { objs.data.Add(0); }
                }
                objSeries.Add(objs);
            }
            objhigchart.ChartX = objChartx;
            objhigchart.Series = objSeries;
            var res = _commonModelFactory.JsonSerialize<HighChart>(objhigchart);
            ViewBag.WorkOrderChart = res;
            return PartialView("_WorkOrdersChart");
        }

    }
}