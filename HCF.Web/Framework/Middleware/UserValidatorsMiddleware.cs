using HCF.BAL;
using HCF.BDO.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace HCF.Web.Framework.Middleware
{
    public class UserValidatorsMiddleware
    {
        private readonly RequestDelegate _next;
        public UserValidatorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, IUserService userRepo)
        {
            if (!IsAjaxRequest(context.Request))
            {
                if ((!context.Request.Path.StartsWithSegments("/change-password")
                    && !context.Request.Path.StartsWithSegments("/logout")
                    && !context.Request.Path.StartsWithSegments("/login")) && Base.UserSession.CurrentUser.UserId > 0)
                {

                    var userId = Base.UserSession.CurrentUser.UserId;
                    var currentUser = userRepo.GetUser(Convert.ToInt32(userId));
                    DateTime? dateLastUpdatedPassword = currentUser.LastUpdatedPasswordDate;
                    if (currentUser.UserId > 0)
                    {
                        if (!currentUser.IsActive)
                        {
                            context.Response.Redirect("/logout?message=" + AccountMessageId.InActiveUserMsg);
                            return;
                        }

                        if (currentUser.IsVendorUser && currentUser.Vendor != null && !currentUser.Vendor.IsActive)
                        {
                            context.Response.Redirect("/logout?message=" + AccountMessageId.InActiveVendorMsg);
                            return;
                        }

                        if (currentUser.UserStatusCode == 202)
                        {
                            context.Response.Redirect("/change-password/lastlogin");
                            return;
                        }
                        if (dateLastUpdatedPassword.HasValue)
                        {
                            int days = (DateTime.UtcNow - dateLastUpdatedPassword.Value).Days;
                            if (days > 180)
                            {
                                context.Response.Redirect("/change-password/expired");
                                return;
                            }
                        }
                    }
                }
            }
            await _next.Invoke(context);
        }


        private static bool IsAjaxRequest(HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }
    }
}
