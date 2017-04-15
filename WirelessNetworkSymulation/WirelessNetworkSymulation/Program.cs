using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WirelessNetworkSymulationView;
using WirelessNetworkSymulationController;
using log4net;
using log4net.Config;

namespace WirelessNetworkSymulation
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure(new FileInfo(ConfigurationSettings.AppSettings["log4net-config-file"]));          // BasicConfigurator.Configure();

            var symulationView = new SimulationView();
            var symulationController = new WirelessNetworkController(symulationView);
            symulationView.Visible = false;
            symulationView.ShowDialog();


        
        }
    }
}
