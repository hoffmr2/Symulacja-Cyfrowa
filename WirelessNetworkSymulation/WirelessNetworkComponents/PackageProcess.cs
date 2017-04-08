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
        private const int MinTransmissionTime = 10;
        private const int MaxTransmissionTime = 100;
           
        public delegate void NewProcessBornEventHandler(object sender, EventArgs e);
        public delegate void FirstPackageInQueueReadyEventHandler(object sender, EventArgs e);
        public delegate void FinalizePackageTransmissionEventHandler(object sender, EventArgs e);

        public delegate bool IsTransmitterBusy();
        public delegate bool IsChannelFree(PackageProcess packageProcess);
        public delegate void SendFrame(PackageProcess packageProcess  );
        public delegate double DrawBackoffTimer(int min, int max);
        public delegate double DrawTransmissionTime(int min, int max);
        public delegate void LoggerWrite(string text);

        public event NewProcessBornEventHandler NewProcessBorn;     
        public event FirstPackageInQueueReadyEventHandler FirstPackageInQueueReady;
        public event FinalizePackageTransmissionEventHandler FinalizePackageTransmission;


        private SendFrame _sendFrame;
        private IsTransmitterBusy _isTransmitterBusy;
        private IsChannelFree _isChannelFree;
        private DrawBackoffTimer _drawBackoffTimer;
        private DrawTransmissionTime _drawTransmissionTime;
        private LoggerWrite _loggerWrite;


        private readonly int _parentTransmitterIndex;
        private readonly int _id;
        private int _sendTime;
        private CsmaCa _csmaCa;


        public PackageProcess(PackageProcessInitDelegates initDelegates, int parenTransmitterIndex ,int globalTime, int id) : base(globalTime)
        {
            InitEventsAndDelegates(initDelegates);
            _parentTransmitterIndex = parenTransmitterIndex;
            _phase = (int) Phase.Born;
            _csmaCa = new CsmaCa(0,false,CsmaCa.ContentionWindowMin);
            _id = id;
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
            _drawBackoffTimer = initDelegates.DrawBackoffTimer;
            _drawTransmissionTime = initDelegates.DrawTransmissionTime;
            _loggerWrite = initDelegates.LoggerWrite;
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
            SetTransmissionTime();
            _phase = (int) Phase.SendOrNotAck;
        }

        private void SetTransmissionTime()
        {
            var randValue = _drawTransmissionTime?.Invoke(MinTransmissionTime, MaxTransmissionTime);
            if (randValue != null)
            {
                Activate((int) randValue);
            }
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
                    SetBackoffTimerValue();

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

        private void SetBackoffTimerValue()
        {
            var randValue = _drawBackoffTimer?.Invoke(0, _csmaCa.ContentionWindow);
            if (randValue != null)
                _csmaCa.BackoffTimer = (int) randValue;
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

                _loggerWrite(LoggerMessages.PackageEvenTime + EventTime/10.0 + LoggerMessages.PackageId + _id + text);
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
