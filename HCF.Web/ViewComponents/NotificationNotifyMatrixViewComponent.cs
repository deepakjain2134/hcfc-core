using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCF.BAL;
using Microsoft.AspNetCore.Mvc;

namespace HCF.Web.ViewComponents
{
    public class NotificationNotifyMatrixViewComponent :ViewComponent
    {
        private readonly INotificationService _notificationService;
        public NotificationNotifyMatrixViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;

        }
        public async Task<IViewComponentResult> InvokeAsync(string mode, string id)
        {
            var data = _notificationService.GetNotifications(mode, id);
            return await Task.FromResult(View(data));
        }
    }
}
