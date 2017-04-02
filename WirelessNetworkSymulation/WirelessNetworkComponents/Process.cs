using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
  
 
    public abstract class Process
    {
        protected double _eventTime;
        protected int _phase;
        protected bool _isTerminated;
        protected bool _isSleeped;

        protected Process(double globalTime )
        {
            _eventTime = globalTime;
            _isTerminated = false;
            _isSleeped = false;
        }

        public double EventTime
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
            protected set { _isSleeped = value; }
        }

        public abstract void Execute();

        public void Activate(double time)
        {
            _eventTime += time;
        }

        public void Wake(double time)
        {
            _isSleeped = false;
            _eventTime = time;
        }

       
    }
}
