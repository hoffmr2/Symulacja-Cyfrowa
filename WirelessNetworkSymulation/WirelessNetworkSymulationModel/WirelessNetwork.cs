using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WirelessNetworkComponents;

namespace WirelessNetworkSymulationModel
{
    public class WirelessNetwork
    {
        public const int TimeScalingFactor = 10;
        public const int MaxSeedSetIndex = 30;
        private Supervisor _supervisor;
        private RandomGeneratorsAnalyzer _randomGeneratorsAnalyzer;
        private int _simulationTime;
        private int _seedSet;
        private double _lambda;
        private bool _enableLogger;
        private List<double> _times;
        private List<double> _means;


        public WirelessNetwork(int transmittersNumber, BackgroundWorker worker)
        {
            _supervisor = new Supervisor(transmittersNumber, worker);
            _randomGeneratorsAnalyzer = new RandomGeneratorsAnalyzer();
            _simulationTime = 0;
            _seedSet = 0;
            _lambda = 0.0;
            _enableLogger = false;
        }

        public double Lambda
        {
            get { return _lambda; }
            set { _lambda = value; }
        }

        public int SeedSet
        {
            get { return _seedSet; }
            set { _seedSet = value; }
        }

        public int SimulationTime
        {
            get { return _simulationTime; }
            set { _simulationTime = value; }
        }

        public List<double> Times
        {
            get { return _times; }
            set { _times = value; }
        }

        public List<double> Means
        {
            get { return _means; }
            set { _means = value; }
        }

        public bool EnableLogger
        {
            get { return _enableLogger; }
            set { _enableLogger = value; }
        }

        public RandomGeneratorsAnalyzer GeneratorsAnalyzer
        {
            get { return _randomGeneratorsAnalyzer; }
            set { _randomGeneratorsAnalyzer = value; }
        }

        public void Run()
        {
            _supervisor.Run(SimulationTime,SeedSet,Lambda,_enableLogger,out _times,out _means);
        }

        public void SteadyStateAnalysis()
        {
            _supervisor.Run(SimulationTime, Lambda, out _times, out _means);
        }

        public bool ValidateSimulationParameters()
        {
            if(_simulationTime <= 0)
                return false;
            if(Lambda <= 0)
                return false;
            if(_seedSet <= 0 && _seedSet > MaxSeedSetIndex)
                return false;
            return true;
        }
    }
}
