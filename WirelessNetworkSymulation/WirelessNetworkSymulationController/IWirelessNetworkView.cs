using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkSymulationController
{
    public interface IWirelessNetworkView
    {
        void SetController(WirelessNetworkController controller);

        void PlotSteadyState(List<double> times, List<double> means);

        BackgroundWorker GetBackgroundWorker();
    }
}
