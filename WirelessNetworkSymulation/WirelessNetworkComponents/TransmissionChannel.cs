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



        public TransmissionChannel()
        {
            _packageProcessesinChannel = new List<PackageProcess>();
            IsFree = true;
        }

        public bool IsFree
        {
            get { return _isFree; }
            set { _isFree = value; }
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
            }
          
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


        public void Reset()
        {
            IsFree = true;
            _packageProcessesinChannel.Clear();
        }

    }
}
