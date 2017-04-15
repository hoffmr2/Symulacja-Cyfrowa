using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
    public class TransmissionChannel
    {
        private List<PackageProcess> _packageProcessesinChannel;

        private bool _isFree;
        private int _failedTransmissions;
        private int _succesTransmissions;
        private int _totalTransmissions;
        private List<double> _times;
        private List<double> _means;


        public TransmissionChannel()
        {
            _packageProcessesinChannel = new List<PackageProcess>();
            IsFree = true;
            _times = new List<double>();
            _means = new List<double>();
        }

        public bool IsFree
        {
            get { return _isFree; }
            set { _isFree = value; }
        }

        public List<double> Means
        {
            get { return _means; }
            set { _means = value; }
        }

        public List<double> Times
        {
            get { return _times; }
            set { _times = value; }
        }

        public double ErrorMean
        {
            get { return _failedTransmissions / (double)_totalTransmissions; }
        }
        public void Collision()
        {
            foreach (var packageProcess in _packageProcessesinChannel)
            {
                packageProcess.IsDomaged = true;
            }
        }
        public void Add(PackageProcess packageProcess)
        {
            _packageProcessesinChannel.Add(packageProcess);
            if (IsFree == false)
            {
                Collision();
            }
        }
        public void Remove(int id)
        {
            var packageProcess = _packageProcessesinChannel.Find(s => s.Id == id);
            _packageProcessesinChannel.Remove(packageProcess);
            if (_packageProcessesinChannel.Count == 0)
                IsFree = true;
        }


        

        public bool IsChannelFree(PackageProcess packageProcess)
        {

            if (IsFree)
                return true;
          
                 foreach (var process in _packageProcessesinChannel)
                 {
                    
                     if (packageProcess.EventTime != process.SendTime1)
                         return false;
                 }
                 return true;
        }

        public void SendFrame(PackageProcess packageProcess)
        {
            _packageProcessesinChannel.Add(packageProcess);
            if (IsFree)
            {
                
                IsFree = false;
            }
            else
            {
                Collision();
            }
        }

        public void EndOfTransmission(PackageProcess packageProcess)
        {
            if (packageProcess == null)
                return;
            if (packageProcess.IsDomaged)
            {
                Remove(packageProcess.Id);
                ++_failedTransmissions;
            }
            else
            {
                ++_succesTransmissions;
            }
            _totalTransmissions = _failedTransmissions + _succesTransmissions;
            _times.Add(packageProcess.EventTime);
            _means.Add(ErrorMean);
        }

        public void OnFinalizePackageTransmission(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if (packageProcess != null)
                switch (packageProcess.GetPhase)
                {
                    case (int) PackageProcess.Phase.SucsessOrRetransmission:  
                        if (packageProcess.GetAck())
                        {
                            Remove(packageProcess.Id);
                        }
                        break;
                    case (int) PackageProcess.Phase.SendOrNotAck:
                        EndOfTransmission(packageProcess);
                        break;
                    default:
                        Debug.Assert(false);
                        break;

                }
        }

        private void ResetStats()
        {
            _means.Clear();
            _times.Clear();
            _failedTransmissions = 0;
            _succesTransmissions = 0;
        }

        public void Reset()
        {
            IsFree = true;
            _packageProcessesinChannel.Clear();
            ResetStats();
        }

    }
}
