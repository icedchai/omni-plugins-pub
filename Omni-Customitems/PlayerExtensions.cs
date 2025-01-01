using Exiled.API.Features;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni_Customitems
{
    public static class PlayerExtensions
    {
        public static void ODestroyNetworkIdentity(this Player player, NetworkIdentity networkIdentity)
        {
            player.Connection.Send(new ObjectDestroyMessage
            {
                netId = networkIdentity.netId
            });
        }
    }
}
