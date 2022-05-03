using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.SignalR.RealTime
{
    public class OnlineUserEventArgs : OnlineClientEventArgs
    {
        public long UserId { get; }

        public OnlineUserEventArgs(long userId, IOnlineClient client)
            : base(client)
        {
            UserId = userId;
        }
    }
}
