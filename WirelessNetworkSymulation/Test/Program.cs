using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RandomGenerators;
using WirelessNetworkComponents;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
           // GenerateSeeds();


            Supervisor supervisor = new Supervisor(150000,10,false,2);
            supervisor.SimulationLoop();
        }

        private static void GenerateSeeds()
        {
            int seedsNumber = 150;
            int seedsInLine = 3;
            UniformRandomGenerator uniformRandomGenerator = new UniformRandomGenerator(1);
            var file = new StreamWriter("seeds.txt");

            for (var i = 0; i < seedsNumber; ++i)
            {
                var line = string.Empty;
                for (var j = 0; j < seedsInLine; ++j)
                {
                    uniformRandomGenerator.Rand();
                    line += uniformRandomGenerator.GetKernel().ToString() + ":";
                }
                file.WriteLine(line);
            }
            file.Close();
        }
    }
}
