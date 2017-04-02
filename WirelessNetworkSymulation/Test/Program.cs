using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WirelessNetworkComponents;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Supervisor supervisor = new Supervisor(1000);
            supervisor.SimulationLoop();
        }
    }
}
