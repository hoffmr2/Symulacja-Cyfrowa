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
}