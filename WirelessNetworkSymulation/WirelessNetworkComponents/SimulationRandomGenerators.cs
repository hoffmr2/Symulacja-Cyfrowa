using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomGenerators;

namespace WirelessNetworkComponents
{
   
    public struct SimulationRandomGenerators
    {
        public UniformRandomGenerator TransmissionTime;//CTPk
        public UniformRandomGenerator BackofftimerValue; //BT
        public ExponentialRandomGenerator GenerationTime; //CGPk
    }
}
