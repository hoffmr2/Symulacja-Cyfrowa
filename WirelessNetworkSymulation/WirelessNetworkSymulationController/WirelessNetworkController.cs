using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WirelessNetworkSymulationModel;

namespace WirelessNetworkSymulationController
{
    public class WirelessNetworkController
    {
        private const int TransmittersNumber = 45;

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
                timeValue = Math.Abs(WirelessNetwork.TimeScalingFactor * timeValue);
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
                lambdaValue = Math.Abs(lambdaValue);
               _wirelessNetwork.Lambda = lambdaValue;
            }
            catch
            {

            }
        }

        public void SetGeneratorAnalysisLambda(string lambda)
        {
            try
            {
                var lambdaValue = double.Parse(lambda);
                lambdaValue = Math.Abs(lambdaValue);
                _wirelessNetwork.GeneratorsAnalyzer.Lambda = lambdaValue;
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
                seedSetValue = Math.Abs(seedSetValue % WirelessNetwork.MaxSeedSetIndex);
                _wirelessNetwork.SeedSet = seedSetValue;
            }
            catch
            {

            }
        }

        public void SetGeneratorAnalysisSeedSet(string seedSet)
        {
            try
            {
                var seedSetValue = int.Parse(seedSet);
                seedSetValue = Math.Abs(seedSetValue % WirelessNetwork.MaxSeedSetIndex);
                _wirelessNetwork.GeneratorsAnalyzer.SeedSet = seedSetValue;
            }
            catch
            {

            }
        }

        public void SetGeneratorAnalysisUpBound(string seedSet)
        {
            try
            {
                var value = int.Parse(seedSet);
                value = Math.Abs(value);
                _wirelessNetwork.GeneratorsAnalyzer.UnifromGeneratorUpBound = value;
            }
            catch
            {

            }
        }

        public void SetGeneratorAnalysisDownBound(string seedSet)
        {
            try
            {
                var value = int.Parse(seedSet);
                value = Math.Abs(value);
                _wirelessNetwork.GeneratorsAnalyzer.UniformGeneratorDownBound = value;
            }
            catch
            {

            }
        }

        public void SetGeneratorAnalysisSamplesNumber(string seedSet)
        {
            try
            {
                var value = int.Parse(seedSet);
                value = Math.Abs(value);
                _wirelessNetwork.GeneratorsAnalyzer.SamplesNumber = value;
            }
            catch
            {

            }
        }

        public void SetOutputText()
        {
            _wirelessNetworkView.SetOutputText(_wirelessNetwork.SimulationOutput);
        }

        public void SetEnableLogger(bool state)
        {
            _wirelessNetwork.EnableLogger = state;
        }

        public void Run()
        {
            if(_wirelessNetwork.ValidateSimulationParameters())
                _wirelessNetwork.Run();
            else
            {
                MessageBox.Show("error", "Wrong Simulation Parameters",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        public void SteadyStateAnalysis()
        {
            if (_wirelessNetwork.ValidateSimulationParameters())
                _wirelessNetwork.SteadyStateAnalysis();
            else
            {
                MessageBox.Show("error", "Wrong Simulation Parameters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void RandomGeneratorsAnalysis()
        {
            if (_wirelessNetwork.GeneratorsAnalyzer.IsInitialized())
                _wirelessNetwork.GeneratorsAnalyzer.RunAnalysis();
            else
            {
                MessageBox.Show("error", "Wrong Parameters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Plot()
        {
            _wirelessNetworkView.PlotSteadyState(_wirelessNetwork.Times, _wirelessNetwork.Means);
        }

        public void PlotGeneratorsHistograms()
        {
            _wirelessNetworkView.PlotExpGeneratorHistogram(_wirelessNetwork.GeneratorsAnalyzer.ExpGeneratorHistogram);
            _wirelessNetworkView.PlotUniformGeneratorHistogram(_wirelessNetwork.GeneratorsAnalyzer.UniformGeneratorHistogram);
        }
    }
}
