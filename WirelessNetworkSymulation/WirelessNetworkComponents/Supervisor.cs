using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using RandomGenerators;

namespace WirelessNetworkComponents
{
    public class Supervisor
    {
        private const int TransmittersNumber = 4; //K = 4
        private int _mainClock; //time 10*ms
        private int _simulationTime;
        private int _processesNumber;
        private Logger _logger;
        private SortedList<int,Process> _processes;
        private Transmitter[] _transmitters;
        private TransmissionChannel _transmissionChannel;
        private SimulationRandomGenerators _simulationRandomGenerators;

        public Supervisor(int simulationTime,int seedSet,bool enableLogger,double lambda)
        {
            _logger = new Logger("Logger.txt",enableLogger);
            _simulationTime = simulationTime;
            _processes = new SortedList<int, Process>(Comparer<int>.Create((x, y) => (x > y) ? 1 : -1));
            _mainClock = 0;
            _processesNumber = 0;
            _transmissionChannel = new TransmissionChannel();
            InitTransmitters();
            InitGenerators(seedSet,lambda);
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

        private void InitTransmitters()
        {
            _transmitters = new Transmitter[TransmittersNumber];
            for (var i = 0; i < _transmitters.Length; ++i)
            {
                _transmitters[i] = new Transmitter(i);
            }
        }

        public void SimulationLoop()
        {
            CreateNewProcess(0);
            CreateNewProcess(1);
            CreateNewProcess(2);
            CreateNewProcess(3);
           

            while (_mainClock < _simulationTime)
            {
                Debug.Assert(_processes.Count != 0);
                var current = _processes.ElementAt(0);
                _processes.RemoveAt(0);
                
                if (current.Value.IsSleeped)
                {
                    //current.Value.Activate(_processes.Last().Key-MainClock);
                    //_processes.Add(current.Value.EventTime, current.Value);
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
            System.Threading.Thread.Sleep(500);

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
            tmPackageProcess.Activate((int)_simulationRandomGenerators.GenerationTime.Rand());
            _processes.Add(tmPackageProcess.EventTime, tmPackageProcess);
        }

        private PackageProcessInitDelegates CreatePackageDelegateInitStruct(int index)
        {
            var constructorParams = new PackageProcessInitDelegates()
            {
                IsTransmitterBusy = _transmitters[index].IsTransmitterBusy,
                IsChannelFree = _transmissionChannel.IsChannelFree,
                OnFinalizePackageTransmissionChannel = _transmissionChannel.OnFinalizePackageTransmission,
                OnFinalizePackageTransmissionTransmitter = _transmitters[index].OnFinalizePackageTransmission,
                OnFinalizePackageTransmissionSupervisor = OnFinalizePackageTransmission,
                OnFirstPackageInQueueReady = _transmitters[index].OnFirstPackageInQueueReady,
                OnNewProcessBornSupervisor = OnNewProcessBron,
                OnNewProcessBornTransmitter = _transmitters[index].OnNewProcessBorn,
                SendFrame = _transmissionChannel.SendFrame,
                DrawBackoffTimer = _simulationRandomGenerators.BackofftimerValue.Rand,
                DrawTransmissionTime = _simulationRandomGenerators.TransmissionTime.Rand,
                LoggerWrite = _logger.LoggerWrite
            };
            return constructorParams;
        }

        public void LogSimulationResults()
        {
            _logger.LoggerForceWrite("number of transmissions: " + _transmissionChannel.NumberOdTransmissions );
            _logger.LoggerForceWrite("number of failed transmissions: " + _transmissionChannel.NumberOfFailedTransmissions);
        }
    }
}
