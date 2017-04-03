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
        public delegate void EndofTransmissionTransmitter();


        public delegate bool IsChannelFree(PackageProcess packageProcess);
        public delegate void SendFrame(PackageProcess packageProcess  );
        public delegate void EndOfTransmission(PackageProcess packageProcess);

        public event NewProcessBornEventHandler NewProcessBorn;     
        public event FirstPackageInQueueReadyEventHandler FirstPackageInQueueReady;
        public event FinalizePackageTransmissionEventHandler FinalizePackageTransmission;

      
        public event SendFrame sendFrame;
        public event EndOfTransmission endOfTransmission;

        private IsTransmitterBusy _isTransmitterBusy;
        private IsChannelFree _isChannelFree;

        private readonly int _parentTransmitterIndex;
        private readonly int _id;
        private double _sendTime;

    

        private CsmaCa _csmaCa;
        

        public PackageProcess(Transmitter parenTransmitter  , NewProcessBornEventHandler onNewProcessBorn,SendFrame sendFrame,EndOfTransmission endOfTransmission,FinalizePackageTransmissionEventHandler onFinalizePackageTransmission,double globalTime, TransmissionChannel channel, int id) : base(globalTime)
        {
     

            NewProcessBorn = parenTransmitter.OnNewProcessBorn;
            NewProcessBorn += onNewProcessBorn;
            FinalizePackageTransmission = parenTransmitter.OnFinalizePackageTransmission;
            FinalizePackageTransmission += onFinalizePackageTransmission;
            _isTransmitterBusy  = parenTransmitter.IsTransmitterBusy;
            FirstPackageInQueueReady = parenTransmitter.OnFirstPackageInQueueReady;

            _parentTransmitterIndex = parenTransmitter.Index;
          //  createNewProcessEvent = createNewProcessEventDelegate;
            _isChannelFree = channel.IsChannelFree;
            this.sendFrame = sendFrame;
            this.endOfTransmission = endOfTransmission;

            base.Phase = (int) Phase.Born;
            _csmaCa = new CsmaCa(0,false,CsmaCa.ContentionWindowMin,0);
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

        public override void Execute()
        {
            bool active = true;
            while (active)
            {
                switch ((Phase) base.Phase)
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
                   // endOfTransmissionTransmitter?.Invoke();
                }
            }
        }

        private void PrepareForRetransmission()
        {
            IsDomaged = false;
            base.Phase = (int) Phase.WaitingForIdleChannel;
            UpdateContentionWindow();
        }


        private void SendOrNotAckOperations(out bool active)
        {
            active = false;
            endOfTransmission?.Invoke(this);
            Activate(CsmaCa.CitzTime);
            base.Phase = (int) Phase.SucsessOrRetransmission;
        }

        private void TransmissionInChannel(out bool active)
        {
            sendFrame?.Invoke(this);
            active = false;
            SendTime1 = EventTime;
            Activate((new Random().Next(0,10)));
            base.Phase = (int) Phase.SendOrNotAck;
        }

        private void WaitingForRandomDelayTime(out bool active)
        {
            if (_csmaCa.BackoffTimer == 0)
            {
                base.Phase = (int) Phase.TransmissionInChannel;
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
                    base.Phase = (int) Phase.WaitingForRandomDelayTime;
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
                base.Phase = (int) Phase.WaitingForIdleChannel;
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
