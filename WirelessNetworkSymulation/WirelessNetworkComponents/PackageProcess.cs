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
        private double _sendTime;
        private CsmaCa _csmaCa;
        

        public PackageProcess(PackageProcessInitDelegates initDelegates, int parenTransmitterIndex ,double globalTime, int id) : base(globalTime)
        {
            InitEventsAndDelegates(initDelegates);
            _parentTransmitterIndex = parenTransmitterIndex;
            _phase = (int) Phase.Born;
            _csmaCa = new CsmaCa(0,false,CsmaCa.ContentionWindowMin,15);
            _id = id;
        }

        private void InitEventsAndDelegates(PackageProcessInitDelegates initDelegates)
        {
            NewProcessBorn = initDelegates.OnNewProcessBornSupervisor;
            NewProcessBorn += initDelegates.OnNewProcessBornTransmitter;
            FinalizePackageTransmission = initDelegates.OnFinalizePackageTransmissionChannel;
            FinalizePackageTransmission += initDelegates.OnFinalizePackageTransmissionTransmitter;
            _isTransmitterBusy = initDelegates.IsTransmitterBusy;
            FirstPackageInQueueReady = initDelegates.OnFirstPackageInQueueReady;
            _isChannelFree = initDelegates.IsChannelFree;
            _sendFrame = initDelegates.SendFrame;
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
                active = false;
                _isTerminated = true;
               OnFinalizePackageTransmission();
            }
            else
            {
                if (_csmaCa.ContentionWindow < CsmaCa.ContentionWindowMax)
                {
                    active = true;
                    PrepareForRetransmission();
                }
                else
                {
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
            UpdateContentionWindow();
        }


        private void SendOrNotAckOperations(out bool active)
        {
            active = false;
            OnFinalizePackageTransmission();
            Activate(CsmaCa.CitzTime);
            _phase = (int) Phase.SucsessOrRetransmission;
        }

        private void TransmissionInChannel(out bool active)
        {
            _sendFrame?.Invoke(this);
            active = false;
            SendTime1 = EventTime;
            Activate((new Random().Next(0,10)));
            _phase = (int) Phase.SendOrNotAck;
        }

        private void WaitingForRandomDelayTime(out bool active)
        {
            if (_csmaCa.BackoffTimer == 0)
            {
                _phase = (int) Phase.TransmissionInChannel;
                active = true;  
            }
            else 
            {
                if (_isChannelFree?.Invoke(this) == true)
                {
                    --_csmaCa.BackoffTimer;
                    active = false;
                    Activate(CsmaCa.ChannelCheckFrequency);
                }
                else
                {
                    active = false;
                    Activate(CsmaCa.ChannelCheckFrequency);
                }
            }
        }

        private void WaitingForIdleChannelPhaseOperations(out bool active)
        {
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
                    _csmaCa.BackoffTimer = (new Random()).Next(0, _csmaCa.ContentionWindow);
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
            }
            
        }

        private void UpdateContentionWindow()
        {
            var newContentionWindow = 2 * (_csmaCa.ContentionWindow + 1) - 1;
            _csmaCa.ContentionWindow = (newContentionWindow < CsmaCa.ContentionWindowMax)
                ? newContentionWindow
                : CsmaCa.ContentionWindowMax;
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

   

        public double SendTime1
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
    }
}
