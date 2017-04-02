using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WirelessNetworkSymulationView;
using WirelessNetworkSymulationController;


namespace WirelessNetworkSymulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var symulationView = new SimulationView();
            var symulationController = new WirelessNetworkController(symulationView);
            symulationView.Visible = false;
            symulationView.ShowDialog();


        
        }
    }
}
