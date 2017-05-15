using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace WirelessNetworkComponents
{
    public struct Pair
    {

        private int _transmissions;
        private int _fails;

        public int Transmissions
        {
            get { return _transmissions; }
            set { _transmissions = value; }
        }

        public int Fails
        {
            get { return _fails; }
            set { _fails = value; }
        }
    }
    public class TransmissionStatistics
    {
        public static int EndOfTransientPhase = 500;
        public const int TiemScalingFactor = 10;
        private int _failedTransmissions;
        private int _succesfulTransmissions;
        private int _transientPhaseTransmissions;
        private int _retransmissions;
        private List<double> _delayTimes;
        private List<double> _waitingTimes;
        private SortedDictionary<int, double> _averageFailsInTime;
        private SortedDictionary<int, double> _averageRetransmissions;
        private List<Pair> _maxFails;


        public void Clear()
        {
            _delayTimes.Clear();
            _waitingTimes.Clear();
            _averageFailsInTime.Clear();
            _averageRetransmissions.Clear();
            FailedTransmissions = 0;
            SuccesfulTransmissions = 0;
            Retransmissions = 0;
            _transientPhaseTransmissions = 0;
            _maxFails.Clear();
            InitMaxFails();



        }

        public void InitMaxFails()
        {
            Pair pair = new Pair()
            {
                Fails = 0,
                Transmissions = 0
            };
            for (int i = 0; i < MaxFails.Capacity; i++)
            {
                MaxFails.Add(pair);
            }
        } 

        public double MaxFailsRatio()
        {
            var maxFails = 0.0;
            return _maxFails.Max(x => x.Fails / (double) x.Transmissions);

        }

    
        public double AverageFails
        {
            get {
                try
                {
                    return (double)_failedTransmissions / (double)(_succesfulTransmissions+_failedTransmissions);
                }
                catch
                {
                    return 0;
                }
            }
           
        }
        public double AvarageDelayTime
        {
            get { return (_delayTimes.Count != 0)? (_delayTimes.Average())/TiemScalingFactor : 0; }
        }

        public double AverageWaitingTimes
        {
            get { return (_waitingTimes.Count() != 0)? _waitingTimes.Average()/TiemScalingFactor : 0; }
        }


        public int FailedTransmissions
        {
            get { return _failedTransmissions; }
            set { _failedTransmissions = value; }
        }


        public int SuccesfulTransmissions
        {
            get { return _succesfulTransmissions; }
            set { _succesfulTransmissions = value; }
        }

        public int Retransmissions
        {
            get { return _retransmissions; }
            set { _retransmissions = value; }
        }

        public List<double> DelayTimes
        {
            get { return _delayTimes; }
            set { _delayTimes = value; }
        }

        public List<double> WaitingTimes
        {
            get { return _waitingTimes; }
            set { _waitingTimes = value; }
        }

        public SortedDictionary<int, double> AverageFailsInTime
        {
            get { return _averageFailsInTime; }
            set { _averageFailsInTime = value; }
        }

        public SortedDictionary<int, double> AverageRetransmissions
        {
            get { return _averageRetransmissions; }
            set { _averageRetransmissions = value; }
        }

        public int TransientPhaseTransmissions
        {
            get { return _transientPhaseTransmissions; }
            set { _transientPhaseTransmissions = value; }
        }

        public List<Pair> MaxFails
        {
            get { return _maxFails; }
            set { _maxFails = value; }
        }
    }
}