using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;

namespace WirelessNetworkComponents
{
    public class Supervisor
    {
        private const int K = 4;
        private double _mainClock;
        private double _simulationTime;
        private int _processesNumber;
        private SortedList<double,Process> _processes;
        private Transmitter[] _transmitters;
        private TransmissionChannel _transmissionChannel;
        private Random CGPk;


        public double MainClock
        {
            get { return _mainClock; }
            set { _mainClock = value; }
        }

        public double SimulationTime
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

        public Supervisor(double simulationTime)
        {
            CGPk = new Random();
            _simulationTime = simulationTime;
            _processes = new SortedList<double, Process>(Comparer<double>.Create((x, y) => (x > y) ? 1 : -1));
            _mainClock = 0.0;
            _processesNumber = 0;
            _transmissionChannel = new TransmissionChannel();
            InitTransmitters();
        }

        private void InitTransmitters()
        {
            _transmitters = new Transmitter[K];
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
                    current.Value.Activate(_processes.Last().Key-MainClock);
                    _processes.Add(current.Value.EventTime, current.Value);
                    continue;
                }
                _mainClock = current.Value.EventTime;
                current.Value.Execute();
                if (current.Value.IsTerminated == false)
                {
                    _processes.Add(current.Value.EventTime, current.Value);
                }
            
            }
        }

        public void OnNewProcessBron(object sender,EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null)
            {
                CreateNewProcess(packageProcess.ParentTransmitterIndex);                
            }
        }

        public void CreateNewProcess(int index)
        {
            ++_processesNumber;
            var tmPackageProcess = new PackageProcess(_transmitters[index], OnNewProcessBron,
                _transmissionChannel.SendFrame,  _transmissionChannel.OnFinalizePackageTransmission,
                MainClock, _transmissionChannel, _processesNumber);
            tmPackageProcess.Activate(Convert.ToDouble(CGPk.Next(0, 15)));
            _processes.Add(tmPackageProcess.EventTime, tmPackageProcess);
        } 
    }
}
