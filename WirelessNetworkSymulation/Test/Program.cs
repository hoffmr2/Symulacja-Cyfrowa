using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using RandomGenerators;
using WirelessNetworkComponents;

using log4net;
using log4net.Config;
using log4net.Filter;
using log4net.Repository;

namespace Test
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
          

        XmlConfigurator.Configure(new FileInfo(ConfigurationSettings.AppSettings["log4net-config-file"]));           // BasicConfigurator.Configure();

            Supervisor supervisor = new Supervisor(100,null);
            supervisor.Run(2000000, 0, 1,false);
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
