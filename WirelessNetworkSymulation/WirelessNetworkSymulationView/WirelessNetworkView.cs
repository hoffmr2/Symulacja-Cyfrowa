using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WirelessNetworkSymulationController;

namespace WirelessNetworkSymulationView
{
    public partial class SimulationView : Form, IWirelessNetworkView
    {
        private WirelessNetworkController _wirelessNetworkController;
        public SimulationView()
        {
            InitializeComponent();
        }

        #region IWirelessNetworkView implementation

        public void SetController(WirelessNetworkController controller)
        {
            _wirelessNetworkController = controller;
        }

        #endregion
    }
}
