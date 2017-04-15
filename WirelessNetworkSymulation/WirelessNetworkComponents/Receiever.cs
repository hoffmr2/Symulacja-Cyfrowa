using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WirelessNetworkComponents
{
    
    public class Receiever
    {
        private int _failedTransmissions;
        private int _succesTransmissions;
        private int _totalTransmissions;
        private List<double> _times;
        private List<double> _means;

        public Receiever()
        {
            _times = new List<double>();
            _means = new List<double>();
        } 

        public int FailedTransmissions
        {
            get { return _failedTransmissions; }
            set { _failedTransmissions = value; }
        }

        public int SuccesTransmissions
        {
            get { return _succesTransmissions; }
            set { _succesTransmissions = value; }
        }

        public int TotalTransmissions
        {
            get { return _totalTransmissions; }
            set { _totalTransmissions = value; }
        }

        public double ErrorMean
        {
            get { return FailedTransmissions / (double)TotalTransmissions; }
        }

        public List<double> Times
        {
            get { return _times; }
        }

        public List<double> Means
        {
            get { return _means; }
        }

        public void OnFinalizePackageTransmission(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if(packageProcess?.GetPhase == (int)PackageProcess.Phase.SendOrNotAck)
            {
                if (packageProcess.IsDomaged)
                {
                    ++FailedTransmissions;
                    packageProcess.SetAckFlag(false);
                }
                else
                {
                    ++SuccesTransmissions;
                    packageProcess.SetAckFlag(true);
                }
                TotalTransmissions = FailedTransmissions + SuccesTransmissions;
                _times.Add(packageProcess.EventTime);
                _means.Add(ErrorMean);

            }
        }

    
    }
}
