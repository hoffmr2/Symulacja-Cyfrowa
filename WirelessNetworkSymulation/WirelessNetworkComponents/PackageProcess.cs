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
        public delegate void AddToTransmitterBufferDelegate(PackageProcess packageProcess);
        public delegate PackageProcess GetFromTransmitterBufferDelegate();
        public delegate bool IsTransmittingPackageDelegate();
        public delegate void PreparePackageForTransmissionDelegate();
        public delegate void EndofTransmissionTransmitter();

        public delegate void CreateNewProcessDelegate(int index);

        public delegate bool IsChannelFreeDelegate(PackageProcess packageProcess);
        public delegate void SendFrame(PackageProcess packageProcess  );
        public delegate void RemoveFromChannel(int id);
        public delegate void EndOfTransmission(PackageProcess packageProcess);

        public event AddToTransmitterBufferDelegate addToTransmitterBufferEvent;
        public event GetFromTransmitterBufferDelegate getFromTransmitterBufferEvent;
        public event IsTransmittingPackageDelegate isTransmittingPackageEvent;
        public event PreparePackageForTransmissionDelegate preparePackageForTransmissionEvent;
        public event EndofTransmissionTransmitter endOfTransmissionTransmitter;

        public event IsChannelFreeDelegate isChannelFreeEvent;
        public event SendFrame sendFrame;
        public event EndOfTransmission endOfTransmission;
        public event RemoveFromChannel removeFromChannel;

        public event CreateNewProcessDelegate createNewProcessEvent;

        private readonly int _parentTransmitterIndex;
        private readonly int _id;
        private double _sendTime;

    

        private CsmaCa _csmaCa;
        

        public PackageProcess(Transmitter parenTransmitter  , CreateNewProcessDelegate createNewProcessEventDelegate,SendFrame sendFrame,EndOfTransmission endOfTransmission,RemoveFromChannel removeFromChannel,double globalTime, TransmissionChannel channel, int id) : base(globalTime)
        {
     

            addToTransmitterBufferEvent = parenTransmitter.PackageProcessesBuffor.Enqueue;
            getFromTransmitterBufferEvent = parenTransmitter.PackageProcessesBuffor.Dequeue;
            isTransmittingPackageEvent = parenTransmitter.IsTransmitterBusy;
            preparePackageForTransmissionEvent = parenTransmitter.PreparePackageForTransmission;
            endOfTransmissionTransmitter = parenTransmitter.EndOfTransmission;
            this.removeFromChannel = removeFromChannel;

            _parentTransmitterIndex = parenTransmitter.Index;
            createNewProcessEvent = createNewProcessEventDelegate;
            isChannelFreeEvent = channel.IsChannelFree;
            this.sendFrame = sendFrame;
            this.endOfTransmission = endOfTransmission;

            _phase = (int) Phase.Born;
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
                endOfTransmissionTransmitter?.Invoke();
                removeFromChannel?.Invoke(_id);
            }
            else
            {
                if (_csmaCa.ContentionWindow < CsmaCa.ContentionWindowMax)
                {
                    active = true;
                    IsDomaged = false;
                    _phase = (int) Phase.WaitingForIdleChannel;
                    var newContentionWindow = 2 * (_csmaCa.ContentionWindow + 1) - 1;
                    _csmaCa.ContentionWindow = (newContentionWindow < CsmaCa.ContentionWindowMax)? newContentionWindow : CsmaCa.ContentionWindowMax;
                }
                else
                {
                    active = false;
                    _isTerminated = true;
                    endOfTransmissionTransmitter?.Invoke();
                }
            }
        }

        private void SendOrNotAckOperations(out bool active)
        {
            active = false;
            endOfTransmission?.Invoke(this);
            Activate(CsmaCa.CitzTime);
            _phase = (int) Phase.SucsessOrRetransmission;
        }

        private void TransmissionInChannel(out bool active)
        {
            sendFrame?.Invoke(this);
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
                if (isChannelFreeEvent?.Invoke(this) == true)
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
            if (isChannelFreeEvent?.Invoke(this) == true)
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
            createNewProcessEvent?.Invoke(ParentTransmitterIndex);
            addToTransmitterBufferEvent?.Invoke(this);
            if (isTransmittingPackageEvent?.Invoke() == false)
            {
                active = true;
                _phase = (int) Phase.WaitingForIdleChannel;
                preparePackageForTransmissionEvent?.Invoke();
            }
            else
            {
                active = false;
                IsSleeped = true;
            }
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

        public double SendTime
        {
            get
            {
                return SendTime1;
            }

            set
            {
                SendTime1 = value;
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
