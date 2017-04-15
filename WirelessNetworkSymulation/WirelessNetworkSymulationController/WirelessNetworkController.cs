using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WirelessNetworkSymulationModel;

namespace WirelessNetworkSymulationController
{
    public class WirelessNetworkController
    {
        private const int TransmittersNumber = 4;
        private IWirelessNetworkView _wirelessNetworkView;
        private WirelessNetwork _wirelessNetwork;

        public WirelessNetworkController(IWirelessNetworkView wirelessNetworkView)
        {
            _wirelessNetworkView = wirelessNetworkView;
            _wirelessNetwork = new WirelessNetwork(TransmittersNumber,_wirelessNetworkView.GetBackgroundWorker());
            wirelessNetworkView.SetController(this);
        }

        public void SetSimulationTime(string time)
        {
            try
            {
                var timeValue = int.Parse(time);
                _wirelessNetwork.SimulationTime = timeValue;
            }
            catch
            {
                
            }
        }

        public void SetLambda(string lambda)
        {
            try
            {
                var lambdaValue = double.Parse(lambda);
               _wirelessNetwork.Lambda = lambdaValue;
            }
            catch
            {

            }
        }

        public void SetSeedSet(string seedSet)
        {
            try
            {
                var seedSetValue = int.Parse(seedSet);
                _wirelessNetwork.SeedSet = seedSetValue;
            }
            catch
            {

            }
        }

        public void SetEnableLogger(bool state)
        {
            _wirelessNetwork.EnableLogger = state;
        }

        public void Run()
        {
            _wirelessNetwork.Run();
        }

        public void Plot()
        {
            _wirelessNetworkView.PlotSteadyState(_wirelessNetwork.Times, _wirelessNetwork.Means);
        }


    }
}
