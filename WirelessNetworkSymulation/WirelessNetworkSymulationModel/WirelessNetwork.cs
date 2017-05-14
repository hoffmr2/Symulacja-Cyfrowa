using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private int _maxTransmissions;
        private int _seedSet;
        private double _lambda;
        private double _startLambda;
        private double _endLambda;
        private bool _enableLogger;
        private List<double> _times;
        private List<double> _means;
        private List<double> _xValuesLambdaAnalysis;
        private List<SimulationResults> _yValuesLambdaAnalysis;
        private DataTable _mainSimulationResults;
        private string _simulationOutput;
        private string _excelOutPath;


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

        public string SimulationOutput
        {
            get { return _simulationOutput; }
            set { _simulationOutput = value; }
        }

        public int MaxTransmissions
        {
            get { return _maxTransmissions; }
            set { _maxTransmissions = value; }
        }

        public double StartLambda
        {
            get { return _startLambda; }
            set { _startLambda = value; }
        }

        public double EndLambda
        {
            get { return _endLambda; }
            set { _endLambda = value; }
        }

        public List<double> XValuesLambdaAnalysis
        {
            get { return _xValuesLambdaAnalysis; }
            set { _xValuesLambdaAnalysis = value; }
        }

        public List<SimulationResults> YValuesLambdaAnalysis
        {
            get { return _yValuesLambdaAnalysis; }
            set { _yValuesLambdaAnalysis = value; }
        }

        public DataTable MainSimulationResults
        {
            get { return _mainSimulationResults; }
            set { _mainSimulationResults = value; }
        }

        public string ExcelOutPath
        {
            get { return _excelOutPath; }
            set { _excelOutPath = value; }
        }

        public void Run()
        {
            _supervisor.Run(SimulationTime,SeedSet,Lambda,MaxTransmissions, _enableLogger,out _times,out _means,out _simulationOutput);
        }

        public void SteadyStateAnalysis()
        {
            _supervisor.Run(SimulationTime, Lambda,MaxTransmissions, out _times, out _means,out _simulationOutput);
        }

        public void LambdaAnalysis()
        {
            _supervisor.Run(StartLambda, EndLambda, out _xValuesLambdaAnalysis, out _yValuesLambdaAnalysis);
        }

        public void MainSimulation()
        {
            _supervisor.Run(Lambda, out _mainSimulationResults);
        }

        public bool ValidateSimulationParameters()
        {
            if(_simulationTime <= 0)
                return false;
            if(Lambda <= 0)
                return false;
            if (MaxTransmissions <= 0)
                return false;
            if (_seedSet <= 0 && _seedSet > MaxSeedSetIndex)
                return false;
            return true;
        }

        public bool ValidateLambdaAnalysisParameters()
        {
            if (StartLambda <= 0)
                return false;
            if (EndLambda <= 0)
                return false;
            if (StartLambda >= EndLambda)
                return false;
            return true;
        }

    }
}
