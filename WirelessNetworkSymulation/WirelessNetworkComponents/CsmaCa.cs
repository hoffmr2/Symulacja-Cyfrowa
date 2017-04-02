namespace WirelessNetworkComponents
{
    public struct CsmaCa
    {
        public const double DifsTime = 2;
        public const double ChannelCheckFrequency = 0.5;
        public const int ContentionWindowMin = 15;
        public const int ContentionWindowMax = 127;
        public const double CitzTime = 1;

        public double DifsCounter { get; set; }

        public bool Ack { get; set; }

        public int BackoffTimer { get; set; }

        public int ContentionWindow { get; set; }

        public CsmaCa(int backoffTimer = 0, bool ack = false, int contentionWindow = 0, int difsCounter = 0 )
        {
            Ack = ack;
            BackoffTimer = backoffTimer;
            ContentionWindow = ContentionWindowMin;
            DifsCounter = difsCounter;

        }
    }
}