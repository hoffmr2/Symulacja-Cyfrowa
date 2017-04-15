namespace WirelessNetworkComponents
{
    public struct PackageProcessInitDelegates
    {
        private PackageProcess.NewProcessBornEventHandler _onNewProcessBornTransmitter;

        private PackageProcess.NewProcessBornEventHandler _onNewProcessBornSupervisor;

        private PackageProcess.FinalizePackageTransmissionEventHandler _onFinalizePackageReceiever;

        private PackageProcess.FinalizePackageTransmissionEventHandler _onFinalizePackageTransmissionChannel;

        private PackageProcess.FinalizePackageTransmissionEventHandler _onFinalizePackageTransmissionTransmitter;

        private PackageProcess.FinalizePackageTransmissionEventHandler _onFinalizePackageTransmissionSupervisor;

        private PackageProcess.FirstPackageInQueueReadyEventHandler _onFirstPackageInQueueReady;

        private PackageProcess.IsChannelFree _isChannelFree;

        private PackageProcess.IsTransmitterBusy _isTransmitterBusy;

        private PackageProcess.SendFrame _sendFrame;

        private PackageProcess.DrawBackoffTimer _drawBackoffTimer;

        private PackageProcess.DrawTransmissionTime _drawTransmissionTime;


        public PackageProcess.NewProcessBornEventHandler OnNewProcessBornTransmitter
        {
            get { return _onNewProcessBornTransmitter; }
            set { _onNewProcessBornTransmitter = value; }
        }

        public PackageProcess.NewProcessBornEventHandler OnNewProcessBornSupervisor
        {
            get { return _onNewProcessBornSupervisor; }
            set { _onNewProcessBornSupervisor = value; }
        }

        public PackageProcess.FinalizePackageTransmissionEventHandler OnFinalizePackageTransmissionChannel
        {
            get { return _onFinalizePackageTransmissionChannel; }
            set { _onFinalizePackageTransmissionChannel = value; }
        }

        public PackageProcess.FinalizePackageTransmissionEventHandler OnFinalizePackageTransmissionTransmitter
        {
            get { return _onFinalizePackageTransmissionTransmitter; }
            set { _onFinalizePackageTransmissionTransmitter = value; }
        }

        public PackageProcess.FirstPackageInQueueReadyEventHandler OnFirstPackageInQueueReady
        {
            get { return _onFirstPackageInQueueReady; }
            set { _onFirstPackageInQueueReady = value; }
        }

        public PackageProcess.IsChannelFree IsChannelFree
        {
            get { return _isChannelFree; }
            set { _isChannelFree = value; }
        }

        public PackageProcess.IsTransmitterBusy IsTransmitterBusy
        {
            get { return _isTransmitterBusy; }
            set { _isTransmitterBusy = value; }
        }

        public PackageProcess.SendFrame SendFrame
        {
            get { return _sendFrame; }
            set { _sendFrame = value; }
        }

        public PackageProcess.FinalizePackageTransmissionEventHandler OnFinalizePackageTransmissionSupervisor
        {
            get { return _onFinalizePackageTransmissionSupervisor; }
            set { _onFinalizePackageTransmissionSupervisor = value; }
        }

        public PackageProcess.DrawBackoffTimer DrawBackoffTimer
        {
            get { return _drawBackoffTimer; }
            set { _drawBackoffTimer = value; }
        }

        public PackageProcess.DrawTransmissionTime DrawTransmissionTime
        {
            get { return _drawTransmissionTime; }
            set { _drawTransmissionTime = value; }
        }

        public PackageProcess.FinalizePackageTransmissionEventHandler OnFinalizePackageReceiever
        {
            get { return _onFinalizePackageReceiever; }
            set { _onFinalizePackageReceiever = value; }
        }
    }
}