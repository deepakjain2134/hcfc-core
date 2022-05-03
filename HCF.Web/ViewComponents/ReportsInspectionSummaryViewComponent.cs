using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using HCF.Web.Base;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class ReportsInspectionSummaryViewComponent : ViewComponent
    {
        private readonly IReportsService _reportService;
        public ReportsInspectionSummaryViewComponent(IReportsService reportService)
        {
            _reportService = reportService;

        }
        public async Task<IViewComponentResult> InvokeAsync(int year, int userId, string sortOrder = "", string OrderbySort = "", int? categoryId = null)
        {
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "StandardEP" : sortOrder;
            OrderbySort = string.IsNullOrEmpty(OrderbySort) ? "ASC" : OrderbySort;
            var currentUserId = UserSession.CurrentUser.UserId;
            var InspectionReport = _reportService.GetInspectionDetailReports(sortOrder, OrderbySort, year, userId, currentUserId, categoryId);

            return await Task.FromResult(View(InspectionReport));
        }
    }
}
