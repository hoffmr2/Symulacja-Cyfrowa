using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkSymulationController
{
    public class WirelessNetworkController
    {
        private IWirelessNetworkView _wirelessNetworkView;

        public WirelessNetworkController(IWirelessNetworkView wirelessNetworkView)
        {
            _wirelessNetworkView = wirelessNetworkView;
            wirelessNetworkView.SetController(this);
        }
    }
}
