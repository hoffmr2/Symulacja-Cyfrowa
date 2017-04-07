namespace WirelessNetworkComponents
{
    public struct CsmaCa
    {
        //all tuime atributes 10x larger
        public const int DifsTime = 20;
        public const int ChannelCheckFrequency = 5;
        public const int ContentionWindowMin = 15;
        public const int ContentionWindowMax = 127;
        public const int CitzTime = 10;

        public int DifsCounter { get; set; }

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