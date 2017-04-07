using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
    public class PackageProcess : Process
    {
        
        public delegate void NewProcessBornEventHandler(object sender, EventArgs e);
        public delegate void FirstPackageInQueueReadyEventHandler(object sender, EventArgs e);
        public delegate void FinalizePackageTransmissionEventHandler(object sender, EventArgs e);

        public delegate bool IsTransmitterBusy();
        public delegate bool IsChannelFree(PackageProcess packageProcess);
        public delegate void SendFrame(PackageProcess packageProcess  );


        public event NewProcessBornEventHandler NewProcessBorn;     
        public event FirstPackageInQueueReadyEventHandler FirstPackageInQueueReady;
        public event FinalizePackageTransmissionEventHandler FinalizePackageTransmission;


        private SendFrame _sendFrame;
        private IsTransmitterBusy _isTransmitterBusy;
        private IsChannelFree _isChannelFree;

        private readonly int _parentTransmitterIndex;
        private readonly int _id;
        private int _sendTime;
        private CsmaCa _csmaCa;
        private static Logger _logger;
        private bool _enableLogger;
        private static Random _random;

        static PackageProcess()
        {
            _logger = new Logger("Logger.txt");
            _random = new Random();
        }
        public PackageProcess(PackageProcessInitDelegates initDelegates, int parenTransmitterIndex ,int globalTime, int id, bool enableLogger = false) : base(globalTime)
        {
            InitEventsAndDelegates(initDelegates);
            _parentTransmitterIndex = parenTransmitterIndex;
            _phase = (int) Phase.Born;
            _csmaCa = new CsmaCa(0,false,CsmaCa.ContentionWindowMin);
            _id = id;
            _enableLogger = enableLogger;
           

        }

               public enum  Phase 
        {
            Born,
            WaitingForIdleChannel,
            WaitingForRandomDelayTime,
            TransmissionInChannel,
            SendOrNotAck,
            SucsessOrRetransmission

        }

        public int ParentTransmitterIndex
        {
            get { return _parentTransmitterIndex; }
        }

        public bool IsDomaged { get; set; }

        public int Id
        {
            get
            {
                return _id;
            }
        }



        public int SendTime1
        {
            get
            {
                return _sendTime;
            }

            set
            {
                _sendTime = value;
            }
        }

        public void SetAckFlag()
        {
            _csmaCa.Ack = true;
        }

        private void InitEventsAndDelegates(PackageProcessInitDelegates initDelegates)
        {
            NewProcessBorn = initDelegates.OnNewProcessBornSupervisor;
            NewProcessBorn += initDelegates.OnNewProcessBornTransmitter;
            FinalizePackageTransmission = initDelegates.OnFinalizePackageTransmissionChannel;
            FinalizePackageTransmission += initDelegates.OnFinalizePackageTransmissionTransmitter;
            FinalizePackageTransmission += initDelegates.OnFinalizePackageTransmissionSupervisor;
            _isTransmitterBusy = initDelegates.IsTransmitterBusy;
            FirstPackageInQueueReady = initDelegates.OnFirstPackageInQueueReady;
            _isChannelFree = initDelegates.IsChannelFree;
            _sendFrame = initDelegates.SendFrame;
        }

 

        public override void Execute()
        {
            bool active = true;
            while (active)
            {
                switch ((Phase) _phase)
                {
                    case Phase.Born:
                        BornPhaseOperations(out active);
                        break;
                    case Phase.WaitingForIdleChannel:
                        WaitingForIdleChannelPhaseOperations(out active);
                        break;
                    case Phase.WaitingForRandomDelayTime:
                        WaitingForRandomDelayTime(out active);
                        break;
                    case Phase.TransmissionInChannel:
                        TransmissionInChannel(out active);
                        break;
                    case Phase.SendOrNotAck:
                        SendOrNotAckOperations(out active);
                        break;
                    case Phase.SucsessOrRetransmission:
                        SucsessOrRetransmissionOperations(out active);
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        }

        private void SucsessOrRetransmissionOperations(out bool active)
        {
            if (_csmaCa.Ack)
            {
                LogWrite(LoggerMessages.CorrectTransmissionPhase);
                active = false;
                _isTerminated = true;
               OnFinalizePackageTransmission();
            }
            else
            {
                if (_csmaCa.ContentionWindow < CsmaCa.ContentionWindowMax)
                {
                    LogWrite(LoggerMessages.RetransmissionPhase);
                    active = true;
                    PrepareForRetransmission();
                }
                else
                {
                    LogWrite(LoggerMessages.AbortTransmissionPhase);
                    active = false;
                    _isTerminated = true;
                    OnFinalizePackageTransmission();
                }
            }
        }

        private void PrepareForRetransmission()
        {
            IsDomaged = false;
            _phase = (int) Phase.WaitingForIdleChannel;
            _csmaCa.DifsCounter = 0;
            UpdateContentionWindow();
        }


        private void SendOrNotAckOperations(out bool active)
        {
            LogWrite(LoggerMessages.SendOrNotAckPhase);
            active = false;
            OnFinalizePackageTransmission();
            Activate(CsmaCa.CitzTime);
            _phase = (int) Phase.SucsessOrRetransmission;
        }

        private void TransmissionInChannel(out bool active)
        {
            SendTime1 = EventTime;
            LogWrite(LoggerMessages.TransmissionInChannelPhase + " Send time: " + SendTime1);
            
            _sendFrame?.Invoke(this);
            active = false;           
            Activate(_random.Next(0,100));
            _phase = (int) Phase.SendOrNotAck;
        }

        private void WaitingForRandomDelayTime(out bool active)
        {
            LogWrite(LoggerMessages.WaitingForRandomDelayPhase);
            if(_isChannelFree?.Invoke(this) == true)
            {
                if (_csmaCa.BackoffTimer == 0)
                {
                    _phase = (int)Phase.TransmissionInChannel;
                    active = true;
                }
                else
                {
                    --_csmaCa.BackoffTimer;
                    active = false;
                    Activate(CsmaCa.ChannelCheckFrequency);
                }
            }
            else
            {
                active = false;
                Activate(CsmaCa.ChannelCheckFrequency);
               // return;
            }

         

        }

        private void WaitingForIdleChannelPhaseOperations(out bool active)
        {
            LogWrite(LoggerMessages.WaitingForIdleChannelPhase);
            if (_isChannelFree?.Invoke(this) == true)
            {
                _csmaCa.DifsCounter += CsmaCa.ChannelCheckFrequency;
                if (_csmaCa.DifsCounter < CsmaCa.DifsTime)
                {
                    active = false;
                    Activate(CsmaCa.ChannelCheckFrequency);
                }
                else
                {
                    _phase = (int) Phase.WaitingForRandomDelayTime;
                    _csmaCa.BackoffTimer = _random.Next(0, _csmaCa.ContentionWindow);
                    //debug help
                    if(_csmaCa.BackoffTimer ==0)
                        LogWrite("backoff timer wylosowany 0");

                    Activate(CsmaCa.ChannelCheckFrequency);
                    active = false;
                }
            }
            else
            {
                _csmaCa.DifsCounter = 0;
                active = false;
                Activate(CsmaCa.ChannelCheckFrequency);
            }
        }

        private void BornPhaseOperations(out bool active)
        {
            OnNewProcessBorn();
           LogWrite(LoggerMessages.BronPhase);
            
            if (_isTransmitterBusy?.Invoke() == false)
            {
                active = true;
                _phase = (int) Phase.WaitingForIdleChannel;
                OnFirstPackageInQueueReady();
            }
            else
            {
                active = false;
                IsSleeped = true;
                _phase = (int)Phase.WaitingForIdleChannel;
            }
            
        }

        private void UpdateContentionWindow()
        {
            var newContentionWindow = 2 * (_csmaCa.ContentionWindow + 1) - 1;
            _csmaCa.ContentionWindow = (newContentionWindow < CsmaCa.ContentionWindowMax)
                ? newContentionWindow
                : CsmaCa.ContentionWindowMax;
        }

        public void LogWrite(string text)
        {
            if (_enableLogger)
                _logger.LoggerWrite(LoggerMessages.PackageEvenTime + EventTime/10.0 + LoggerMessages.PackageId + _id + text);
        }

        public bool GetAck()
        {
            return _csmaCa.Ack;
        }

        protected virtual void OnNewProcessBorn()
        {
            NewProcessBorn?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFirstPackageInQueueReady()
        {
           FirstPackageInQueueReady?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFinalizePackageTransmission()
        {
            FinalizePackageTransmission?.Invoke(this, EventArgs.Empty);
        }

       
    }
}
