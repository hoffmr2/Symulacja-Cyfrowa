using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
  
 
    public abstract class Process
    {
        protected int _eventTime;
        protected int _phase;
        protected bool _isTerminated;
        protected bool _isSleeped;

        protected Process(int globalTime )
        {
            _eventTime = globalTime;
            _isTerminated = false;
            _isSleeped = false;
        }

        public int EventTime
        {
            get { return _eventTime; }
        }

        public bool IsTerminated
        {
            get { return _isTerminated; }
        }

        public bool IsSleeped
        {
            get { return _isSleeped; }
            set { _isSleeped = value; }
        }

        public int GetPhase
        {
            get { return _phase; }
            protected set { _phase = value; }
        }

        public void Activate(int time)
        {
            _eventTime += time;
        }

        public void Wake(int time)
        {
            _isSleeped = false;
            _eventTime = time;
        }

        public abstract void Execute();

    }
}
