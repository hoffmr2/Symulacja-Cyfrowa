using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using RandomGenerators;

using log4net;
using log4net.Config;
using log4net.Core;

namespace WirelessNetworkComponents
{
    public class Supervisor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Supervisor));
        private const int SimulationsNumber = 15;
        private int TransmittersNumber; //K = 4
        private int _mainClock; //time 10*ms
        private int _simulationTime;
        private int _processesNumber;
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


        private void InitGenerators(int seedSet,double lambda)
        {
            var file = new StreamReader("seeds.txt");
            var line = file.ReadToEnd();
            var lines = line.Split('\n');
            var expectedLine = lines[seedSet];
            var seeds = expectedLine.Split(':');

            try
            {
                _simulationRandomGenerators.BackofftimerValue = new UniformRandomGenerator(int.Parse(seeds[0]));
                _simulationRandomGenerators.GenerationTime = new ExponentialRandomGenerator(lambda,int.Parse(seeds[1]));
                _simulationRandomGenerators.TransmissionTime = new UniformRandomGenerator(int.Parse(seeds[2]));

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

        public void Run(int simulationTime, int seedSet, double lambda,bool enableLogger )
        {
            SetLogging(enableLogger);
            _simulationTime = simulationTime;
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

        public void Run(int simulationTime, int seedSet, double lambda, bool enableLogger,out List<double> times, out List<double> means)
        {
            
            Run(simulationTime,seedSet,lambda,enableLogger);

            times = new List<double>();
            means = new List<double>();
       
            foreach (var d in PackageProcess.Statistics.AverageFailsInTime)
            {

                means.Add(d.Value);
                times.Add(d.Key);

            }
        }
        public void Run(int simulationTime, double lambda, out List<double> times, out List<double> means)
        {
            means = null;
            times = null;
            means = new List<double>();
            times = new List<double>();
            for (int i = 0; i < simulationTime; ++i)
            {
                times.Add(i);
                means.Add(0);
            }
            for (int i = 0; i < SimulationsNumber; ++i)
            {
                Run(simulationTime, i, lambda, false);
            
                {
                    for (var j = 0; j < means.Count; ++j)
                    {
                        try
                        {
                            means[j] += PackageProcess.Statistics.AverageFailsInTime[(int)times[j]];
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
        }

        private void SimulationLoop()
        {
           for(int i=0;i<TransmittersNumber;++i)
                CreateNewProcess(i);
           

            while (_mainClock < _simulationTime)
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
                
                if (PackageProcess.Statistics.AverageFailsInTime.ContainsKey(_mainClock) == false)
                {
                    PackageProcess.Statistics.AverageFailsInTime.Add(_mainClock,PackageProcess.Statistics.AverageFails);
                }
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
         //   System.Threading.Thread.Sleep(50);

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
            var time = _simulationRandomGenerators.GenerationTime.Rand();
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
                DrawBackoffTimer = _simulationRandomGenerators.BackofftimerValue.Rand,
                DrawTransmissionTime = _simulationRandomGenerators.TransmissionTime.Rand
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
           log.Info("number of  transmissions: " + _transmissionChannel.TotalTransmissions);
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
