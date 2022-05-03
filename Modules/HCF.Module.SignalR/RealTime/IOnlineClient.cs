using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.SignalR.RealTime
{
    public interface IOnlineClient
    {
        /// <summary>
        /// Unique connection Id for this client.
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// Connection establishment time for this client.
        /// </summary>
        DateTime ConnectTime { get; }

        /// <summary>
        /// Shortcut to set/get <see cref="Properties"/>.
        /// </summary>
        object this[string key] { get; set; }

        /// <summary>
        /// Can be used to add custom properties for this client.
        /// </summary>
        Dictionary<string, object> Properties { get; }
    }
}
