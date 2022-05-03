using HCF.Module.Core.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.SignalR.Hubs
{
    public abstract class HubBase : Hub
    {
        public ILogger<HubBase> Logger { get; set; }

        protected IWorkContext WorkContent { get; }

        protected HubBase(IWorkContext workContent)
        {
            Logger = NullLogger<HubBase>.Instance;
            WorkContent = workContent;
        }
    }
}
