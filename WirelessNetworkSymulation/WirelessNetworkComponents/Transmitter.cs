using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WirelessNetworkComponents
{
    public class Transmitter
    {
        private readonly int _index;
        private Queue<PackageProcess> _packageProcessesBuffor;
        private bool _isTransmittingPackage;

        public Transmitter(int index)
        {
            _index = index;
            _packageProcessesBuffor = new Queue<PackageProcess>();
            _isTransmittingPackage = false;
        }

        public bool IsBufforEmpty()
        {
            return _packageProcessesBuffor.Count == 0;
        }

       
        public int Index
        {
            get { return _index; }
        }

        public Queue<PackageProcess> PackageProcessesBuffor
        {
            get { return _packageProcessesBuffor; }
            set { _packageProcessesBuffor = value; }
        }

        public PackageProcess TransmittingPackageProcess { get; set; }

        public bool IsTransmittingPackage
        {
            get { return _isTransmittingPackage; }
            set { _isTransmittingPackage = value; }
        }

        public bool IsTransmitterBusy()
        {
            return IsTransmittingPackage;
            
        }

        public void Reset()
        {
            _packageProcessesBuffor.Clear();
            _isTransmittingPackage = false;
        }
        public void OnFinalizePackageTransmission(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null && packageProcess.GetPhase == (int) PackageProcess.Phase.SendOrNotAck)
                return;
           var first = _packageProcessesBuffor.Dequeue();
            if (_packageProcessesBuffor.Count != 0)
            {
                _packageProcessesBuffor.First().Wake(first.EventTime);
               // _isTransmittingPackage = false;
            }
            else
            {
                _isTransmittingPackage = false;
            }
        }

        public void OnFirstPackageInQueueReady(object sender, EventArgs e)
        {
            IsTransmittingPackage = true;
        }

        public void OnNewProcessBorn(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null)
            {
                _packageProcessesBuffor.Enqueue(packageProcess);
            
            }
        }

    }
}
