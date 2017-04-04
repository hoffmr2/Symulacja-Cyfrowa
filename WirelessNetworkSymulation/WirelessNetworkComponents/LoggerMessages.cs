namespace WirelessNetworkComponents
{
    public struct LoggerMessages
    {
        public const string BronPhase = " Phase: Born";
        public const string WaitingForIdleChannelPhase = " Phase: Waiting for idle channel";
        public const string WaitingForRandomDelayPhase = " Phase: Waiting for random delay time";
        public const string TransmissionInChannelPhase = " Phase: Transmission in channel";
        public const string SendOrNotAckPhase = " Phase: Send or not ack";
        public const string CorrectTransmissionPhase = " Phase: received with ack";
        public const string AbortTransmissionPhase = " Phase: received without ack -transmission aborted";
        public const string RetransmissionPhase = " Phase: received without ack -retransmission";
        public const string PackageId = " Package id: ";
        public const string PackageEvenTime = " Package event time: ";
    }
}