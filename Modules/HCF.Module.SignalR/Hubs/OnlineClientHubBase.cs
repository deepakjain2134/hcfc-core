using HCF.Module.Core.Extensions;
using HCF.Module.SignalR.RealTime;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.SignalR.Hubs
{
    public abstract class OnlineClientHubBase : HubBase
    {
        protected IOnlineClientManager OnlineClientManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonHub"/> class.
        /// </summary>
        protected OnlineClientHubBase(IWorkContext workContext, IOnlineClientManager onlineClientManager) : base(workContext)
        {
            OnlineClientManager = onlineClientManager;
            Logger = NullLogger<OnlineClientHubBase>.Instance;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var client = CreateClientForCurrentConnection();

            //Logger.LogDebug("A client is connected: " + client);

            OnlineClientManager.Add(client);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            //Logger.LogDebug("A client is disconnected: " + Context.ConnectionId);

            try
            {
                OnlineClientManager.Remove(Context.ConnectionId);
            }
            catch (Exception ex)
            {
                //Logger.LogWarning(ex.ToString(), ex);
            }
        }

        protected virtual IOnlineClient CreateClientForCurrentConnection()
        {
            return new OnlineClient(Context.ConnectionId, WorkContent.GetCurrentUser().Result.UserId);
        }
    }
}
