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
        public List< UniformRandomGenerator> TransmissionTime;//CTPk
        public List<UniformRandomGenerator> BackofftimerValue; //BT
        public List<ExponentialRandomGenerator> GenerationTime; //CGPk
    }
}
