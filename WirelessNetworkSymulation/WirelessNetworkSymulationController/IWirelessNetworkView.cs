using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WirelessNetworkComponents;

namespace WirelessNetworkSymulationController
{
    public interface IWirelessNetworkView
    {

        void SetController(WirelessNetworkController controller);

        void PlotLambda(List<double> xValues, List<SimulationResults> yValues);

        void PlotSteadyState(List<double> times, List<double> means,string series);

        void PlotUniformGeneratorHistogram(SortedDictionary<double, int> data);

        void PlotExpGeneratorHistogram(SortedDictionary<double, int> data);

        void ClearSteadyStatePlot();

        void SavePlot(string path, object chart);

        void SetOutputText(string text);

        BackgroundWorker GetBackgroundWorker();

        void DisableControls();

        void EnableControls();
    }
}
