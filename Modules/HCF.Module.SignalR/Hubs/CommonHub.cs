using HCF.Module.Core.Extensions;
using HCF.Module.SignalR.RealTime;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.SignalR.Hubs
{
    public class CommonHub : OnlineClientHubBase
    {
        public CommonHub(IWorkContext workContext, IOnlineClientManager onlineClientManager) : base(workContext, onlineClientManager)
        {
        }

        public void Register()
        {
            Logger.LogDebug("A client is registered: " + Context.ConnectionId);
        }
    }
}
