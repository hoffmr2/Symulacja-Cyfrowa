using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using RandomGenerators;
using System.Web;

using log4net;
using log4net.Config;
using log4net.Core;

namespace WirelessNetworkComponents
{
    public struct SimulationResults
    {
        private double _lostPackagesMean;
        private double _errorUpBound;
        private double _errorLowBound;
        private double _flow;
        private double _maxLostPackagesRatio;

        public double LostPackagesMean
        {
            get { return _lostPackagesMean; }
            set { _lostPackagesMean = value; }
        }

        public double ErrorUpBound
        {
            get { return _errorUpBound; }
            set { _errorUpBound = value; }
        }

        public double ErrorLowBound
        {
            get { return _errorLowBound; }
            set { _errorLowBound = value; }
        }

        public double Flow
        {
            get { return _flow; }
            set { _flow = value; }
        }

        public double MaxLostPackagesRatio
        {
            get { return _maxLostPackagesRatio; }
            set { _maxLostPackagesRatio = value; }
        }
    };

public class Supervisor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Supervisor));
        private const int SimulationsNumber = 15;

        private int TransmittersNumber; //K = 4
        private int _mainClock; //time 10*ms
        private int _simulationTime;
        private int _processesNumber;
        private int _maxTransmissions;
        private SortedList<int,Process> _processes;
        private Transmitter[] _transmitters;
        private Receiever[] _receievers;
        private TransmissionChannel _transmissionChannel;
        private SimulationRandomGenerators _simulationRandomGenerators;
        private BackgroundWorker _worker;

        public Supervisor(int transmitterNumber, BackgroundWorker worker)
        {
           
            _worker = worker;
            TransmittersNumber = transmitterNumber;
            _processes = new SortedList<int, Process>(Comparer<int>.Create((x, y) => (x > y) ? 1 : -1));
            _mainClock = 0;
            _processesNumber = 0;
            _transmissionChannel = new TransmissionChannel();
          // GenerateSeeds(transmitterNumber);
            InitTransmittersAndReceievers();
        }

        public int MainClock
        {
            get { return _mainClock; }
            set { _mainClock = value; }
        }

        public int SimulationTime
        {
            get
            {
                return _simulationTime;
            }

            set
            {
                _simulationTime = value;
            }
        }

        public int MaxTransmissions
        {
            get { return _maxTransmissions; }
            set { _maxTransmissions = value; }
        }


        private void InitGenerators(int seedSet,double lambda)
        {
            _simulationRandomGenerators.BackofftimerValue = new List<UniformRandomGenerator>();
            _simulationRandomGenerators.GenerationTime = new List<ExponentialRandomGenerator>();
            _simulationRandomGenerators.TransmissionTime = new List<UniformRandomGenerator>();

            var file = new StreamReader("seeds.txt");
            var line = file.ReadToEnd();
            var lines = line.Split('\n');
            var expectedLine = lines[seedSet];
            var seeds = expectedLine.Split(':');

            try
            {
                for (int i = 0; i < TransmittersNumber; i++)
                {

                        _simulationRandomGenerators.BackofftimerValue.Add( new UniformRandomGenerator(int.Parse(seeds[3*i + 0])));
                        _simulationRandomGenerators.GenerationTime.Add( new ExponentialRandomGenerator(lambda,int.Parse(seeds[3*i+1])));
                        _simulationRandomGenerators.TransmissionTime.Add( new UniformRandomGenerator(int.Parse(seeds[3*i+2])));
                    
                }
            }
            catch 
            {
                throw new Exception("Error initialising random Generators");
            }

        }

        private void InitTransmittersAndReceievers()
        {
            _transmitters = new Transmitter[TransmittersNumber];
            _receievers = new Receiever[TransmittersNumber];
            for (var i = 0; i < _transmitters.Length; ++i)
            {
                _transmitters[i] = new Transmitter(i);
                _receievers[i] = new Receiever();
            }
        }

        public void Run(int simulationTime, int seedSet, double lambda,int maxTransmissions,bool enableLogger )
        {
            SetLogging(enableLogger);
            _simulationTime = simulationTime;
            _maxTransmissions = maxTransmissions;
            _mainClock = 0;
            _processesNumber = 0;
            InitGenerators(seedSet, lambda);
            ResetSimulationComponents();
            SimulationLoop();

        }

        private static void SetLogging(bool enableLogger)
        {
            if (!enableLogger)
            {
                log.Logger.Repository.Threshold = Level.Off;
            }
            else
            {
                log.Logger.Repository.Threshold = Level.All;
            }
        }

        private void ResetSimulationComponents()
        {
            _processes.Clear();
            _transmissionChannel.Reset();
            ResetTransmitters();
            PackageProcess.Statistics.Clear();
        }

        private void ResetTransmitters()
        {
            foreach (var transmitter in _transmitters)
            {
                transmitter.Reset();
            }
        }

        public void Run(int simulationTime, int seedSet, double lambda, int maxTransmissions, bool enableLogger,out List<double> times, out List<double> means, out string outputData)
        {
            TransmissionStatistics.EndOfTransientPhase = 0;
            Run(simulationTime,seedSet,lambda,maxTransmissions,enableLogger);
            times = null;
            means = null;
            times = new List<double>();
            means = new List<double>();
       
            foreach (var d in PackageProcess.Statistics.AverageFailsInTime)
            {

                means.Add(d.Value);
                times.Add(d.Key);

            }
            outputData = SimulationResult();
        }
        public void Run(int simulationTime, double lambda, int maxTransmissions, out List<double> times, out List<double> means,out string outputData)
        {
            TransmissionStatistics.EndOfTransientPhase = 0;
            means = null;
            times = null;
            means = new List<double>();
            times = new List<double>();
            for (int i = 0; i < maxTransmissions; ++i)
            {
                times.Add(i);
                means.Add(0);
            }
            for (int i = 0; i < SimulationsNumber; ++i)
            {
                Run(simulationTime, i, lambda,maxTransmissions, false);
            
                {
                    for (var j = 0; j <MaxTransmissions; ++j)
                    {
                        try
                        {
                            means[j] += PackageProcess.Statistics.AverageRetransmissions[j];
                        }
                        catch
                        {
                        }                   
                    }
                }
                 
            }
            for (var j = 0; j < means.Count; ++j)
            {
                means[j] /= (double)SimulationsNumber;
            }
            outputData = SimulationResult();
        }


        private void Run(double lambda,out SimulationResults simulationResults )
        {
            TransmissionStatistics.EndOfTransientPhase = 250;
            var maxTransmissions = 3000;
            var simulationTime = 1000000;
            var lostPackagesList = new List<double>();

            simulationResults = new SimulationResults()
            {
                LostPackagesMean = 0,
                ErrorLowBound = 0,
                ErrorUpBound = 0,
                Flow = 0,
                MaxLostPackagesRatio = 0
               
            };

            for (int i = 0; i < SimulationsNumber; ++i)
            {
                Run(simulationTime, i, lambda, maxTransmissions, false);
                lostPackagesList.Add(PackageProcess.Statistics.AverageFails);
                simulationResults.MaxLostPackagesRatio += PackageProcess.Statistics.MaxFailsRatio();
                var allTransmissions = (PackageProcess.Statistics.SuccesfulTransmissions +
                                        PackageProcess.Statistics.FailedTransmissions);
                simulationResults.Flow += (double) allTransmissions / MainClock *
                                          TransmissionStatistics.TiemScalingFactor;
            }
            simulationResults.MaxLostPackagesRatio /= SimulationsNumber;
            simulationResults.Flow /= SimulationsNumber;
            CalculateDeviations(ref simulationResults, lostPackagesList);
        }

        public void Run(double lambda,out DataTable dataTable)
        {
            TransmissionStatistics.EndOfTransientPhase = 250;
            var maxTransmissions = 3000;
            var simulationTime = 100000;
            var MeanFailRatio = new List<double>();
            var MaxFailRatio = new List<double>();
            var AverageRetransmissions = new List<double>();
            var AverageWaitingTime = new List<double>();
            var AverageDelayTime = new List<double>();
            var Flow = new List<double>();
        
            for (int i = 0; i < SimulationsNumber; ++i)
            {
                Run(simulationTime, i, lambda, maxTransmissions, false);
                MeanFailRatio.Add(PackageProcess.Statistics.AverageFails);
                MaxFailRatio.Add(PackageProcess.Statistics.MaxFailsRatio());
                AverageRetransmissions.Add(PackageProcess.Statistics.Retransmissions/(double)TransmittersNumber);
                AverageWaitingTime.Add(PackageProcess.Statistics.AverageWaitingTimes);
                AverageDelayTime.Add(PackageProcess.Statistics.AvarageDelayTime);
                var allTransmissions = (PackageProcess.Statistics.SuccesfulTransmissions +
                         PackageProcess.Statistics.FailedTransmissions);
                Flow.Add( (double)allTransmissions *
                                      TransmissionStatistics.TiemScalingFactor / MainClock);

            }

            string[] columnNames = {"Nr Symulacji",
                                    "Średnia Pakietowa Stopa Błedów",
                                    "Maksymalna Pakietowa Stopa Błedów",
                                    "Średnia Liczba Retransmisji [ms]",
                                    "Średni Czas Oczekiwania [ms]",
                                    "Średnie Opóźnienie",
                                    "Przepływność [Pakiet/ms]"};
           dataTable = new DataTable("Wyniki Symulacji");
            DataColumn column;
            foreach (var columnName in columnNames)
            {
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.Double");
                column.ColumnName = columnName;
                column.AutoIncrement = false;
                column.Caption = columnName;
                column.ReadOnly = false;
                column.Unique = false;
                dataTable.Columns.Add(column);
            }
            DataRow row;
            for (int i = 0; i < SimulationsNumber; ++i)
            {
                row = dataTable.NewRow();
                row[columnNames[0]] = i;
                row[columnNames[1]] = MeanFailRatio[i];
                row[columnNames[2]] = MaxFailRatio[i];
                row[columnNames[3]] = AverageRetransmissions[i];
                row[columnNames[4]] = AverageWaitingTime[i];
                row[columnNames[5]] = AverageDelayTime[i];
                row[columnNames[6]] = Flow[i];
                dataTable.Rows.Add(row);

            }

            row = dataTable.NewRow();
            row[columnNames[0]] = 0.0;
            row[columnNames[1]] = MeanFailRatio.Average();
            row[columnNames[2]] = MaxFailRatio.Average();
            row[columnNames[3]] = AverageRetransmissions.Average();
            row[columnNames[4]] = AverageWaitingTime.Average();
            row[columnNames[5]] = AverageDelayTime.Average();
            row[columnNames[6]] = Flow.Average();
            dataTable.Rows.Add(row);


            row = dataTable.NewRow();
            row[columnNames[0]] = 0.0;
            row[columnNames[1]] = TotalDeviation(MeanFailRatio);
            row[columnNames[2]] = TotalDeviation(MaxFailRatio);
            row[columnNames[3]] = TotalDeviation(AverageRetransmissions);
            row[columnNames[4]] = TotalDeviation(AverageWaitingTime);
            row[columnNames[5]] = TotalDeviation(AverageDelayTime);
            row[columnNames[6]] = TotalDeviation(Flow);
            dataTable.Rows.Add(row);

        }


        public void Run(double startLambda,double endLambda, out List<double> xValues,out List<SimulationResults> yValues  )
        {
            xValues = new List<double>();
            yValues = new List<SimulationResults>();
            var pointsNumber = 10;
            double step = (endLambda - startLambda) / pointsNumber;
            for (var lambda = startLambda; lambda < endLambda; lambda += step)
            {
                SimulationResults simulationResults;
                xValues.Add(lambda);
                Run(lambda,out simulationResults);
                yValues.Add(simulationResults);
            }
        }

        private static void CalculateDeviations(ref SimulationResults simulationResults, List<double> lostPackagesList)
        {
            simulationResults.LostPackagesMean = lostPackagesList.Average();
            var totalDeviation = TotalDeviation(lostPackagesList);
            simulationResults.ErrorUpBound = simulationResults.LostPackagesMean + totalDeviation;
            simulationResults.ErrorLowBound = simulationResults.LostPackagesMean - totalDeviation;
        }

        private static double TotalDeviation(List<double> data)
        {
            var average = data.Average();
            double standardDeviation = 0.0;
            foreach (var d in data)
            {
                standardDeviation += Math.Pow(d - average, 2);
            }
            standardDeviation /= SimulationsNumber;
            standardDeviation = Math.Sqrt(standardDeviation);
            const double studentsFactor = 2.1315;
            var totalDeviation = standardDeviation * studentsFactor / Math.Sqrt(SimulationsNumber);
            return totalDeviation;
        }


        private void SimulationLoop()
        {
           for(int i=0;i<TransmittersNumber;++i)
                CreateNewProcess(i);
           

            while (CheckTimeCondition() && CheckTransmissionsNumberCondition())
            {
                
                if (_mainClock % 10 == 0)
                {
                    if (_worker != null)
                    {
                        _worker.ReportProgress((int)((float)_mainClock / (float)SimulationTime * 100));
                    }
                }
                Debug.Assert(_processes.Count != 0);
                var current = _processes.ElementAt(0);
                _processes.RemoveAt(0);
                
                if (current.Value.IsSleeped)
                {
                    continue;
                }
                _mainClock = current.Value.EventTime;
                current.Value.Execute();
                if (current.Value.IsTerminated == false)
                {
                    _processes.Add(current.Value.EventTime, current.Value);
                }           
            }
            LogSimulationResults();

        }

        private bool CheckTransmissionsNumberCondition()
        {
            return PackageProcess.Statistics.SuccesfulTransmissions < MaxTransmissions;
        }

        private bool CheckTimeCondition()
        {
            return _mainClock < _simulationTime;
        }

        private static void GenerateSeeds(int transmittersNumber)
        {
            int seedsNumber = 150;
            int seedsInLine = 3*transmittersNumber;
            int seedsDistance = 100000;
            UniformRandomGenerator uniformRandomGenerator = new UniformRandomGenerator(1);
            var file = new StreamWriter("seeds.txt");

            for (var i = 0; i < seedsNumber; ++i)
            {
                var line = string.Empty;
                for (var j = 0; j < seedsInLine; ++j)
                {
                    for (int k = 0; k < seedsDistance; ++k)
                        uniformRandomGenerator.Rand();
                    line += uniformRandomGenerator.GetKernel().ToString() + ":";
                }
                file.WriteLine(line);
            }
            file.Close();
        }

        public void OnNewProcessBron(object sender,EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null)
            {
                CreateNewProcess(packageProcess.ParentTransmitterIndex);                
            }
        }

        void OnFinalizePackageTransmission(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null && packageProcess.GetPhase == (int)PackageProcess.Phase.SendOrNotAck)
                return;

            var transmitterIndex = packageProcess.ParentTransmitterIndex;
                if (_transmitters[transmitterIndex].PackageProcessesBuffor.Count != 0)
                {
                    var firstInTransmitterBuffer = _transmitters[transmitterIndex].PackageProcessesBuffor.First();
                    _processes.Add(firstInTransmitterBuffer.EventTime, firstInTransmitterBuffer);
                }
            
        }

        public void CreateNewProcess(int index)
        {
            ++_processesNumber;
            var constructorParams = CreatePackageDelegateInitStruct(index);
            var tmPackageProcess = new PackageProcess(constructorParams,index,MainClock,_processesNumber);
            var time = _simulationRandomGenerators.GenerationTime[index].Rand();
            tmPackageProcess.Activate((int)(time*10));
            _processes.Add(tmPackageProcess.EventTime, tmPackageProcess);
        }

        private PackageProcessInitDelegates CreatePackageDelegateInitStruct(int index)
        {
            var constructorParams = new PackageProcessInitDelegates()
            {
                IsTransmitterBusy = _transmitters[index].IsTransmitterBusy,
                IsChannelFree = _transmissionChannel.IsChannelFree,
                OnFinalizePackageReceiever = _receievers[index].OnFinalizePackageTransmission,
                OnFinalizePackageTransmissionChannel = _transmissionChannel.OnFinalizePackageTransmission,
                OnFinalizePackageTransmissionTransmitter = _transmitters[index].OnFinalizePackageTransmission,
                OnFinalizePackageTransmissionSupervisor = OnFinalizePackageTransmission,
                OnFirstPackageInQueueReady = _transmitters[index].OnFirstPackageInQueueReady,
                OnNewProcessBornSupervisor = OnNewProcessBron,
                OnNewProcessBornTransmitter = _transmitters[index].OnNewProcessBorn,
                SendFrame = _transmissionChannel.SendFrame,
                DrawBackoffTimer = _simulationRandomGenerators.BackofftimerValue[index].Rand,
                DrawTransmissionTime = _simulationRandomGenerators.TransmissionTime[index].Rand
            };
            return constructorParams;
        }

        public void LogSimulationResults()
        {
           // _logger.LoggerForceWrite("number of transmissions: " + _transmissionChannel.NumberOdTransmissions );
           // _logger.LoggerForceWrite("number of failed transmissions: " + _transmissionChannel.NumberOfFailedTransmissions);
            double mean = 0;
            foreach (Receiever receiever in _receievers)
            {
                mean += receiever.ErrorMean;
            }
            mean /= _receievers.Length;
            log.Logger.Repository.Threshold = Level.All;
           log.Info("number of transmissions: " + mean);
            log.Info("number of  processes: " + _processes.Count);
        }

        private string SimulationResult()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("Number of lost packeges: " + PackageProcess.Statistics.FailedTransmissions);
            text.AppendLine("Number of succes transmissions: " + PackageProcess.Statistics.SuccesfulTransmissions);
            text.AppendLine("Number of retransmissions: " + PackageProcess.Statistics.Retransmissions);
            text.AppendLine("Average delay time: " + PackageProcess.Statistics.AvarageDelayTime);
            text.AppendLine("Average waiting time: " + PackageProcess.Statistics.AverageWaitingTimes);
            text.AppendLine("Average lost packages: " + PackageProcess.Statistics.AverageFails);
            return text.ToString();

        }


    }
}
