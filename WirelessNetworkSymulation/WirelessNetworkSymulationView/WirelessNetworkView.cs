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

        public void PlotSteadyState(List<double> times, List<double> means)
        {
           chartSteadyState.Series["error mean"].Points.Clear();
            for(int i=0;i< times.Count;++i)
            {
                chartSteadyState.Series["error mean"].Points.AddXY(times[i], means[i]);
            }
         //   chartSteadyState.DataBind();
            
        }

        public BackgroundWorker GetBackgroundWorker()
        {
            return backgroundWorkerSimulationLoop;
        }
        #endregion

        private void textBoxSimulationTime_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetSimulationTime(textBoxSimulationTime.Text);
        }

        private void textBoxLambda_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetLambda(textBoxLambda.Text);
        }

        private void textBoxSeedSet_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetSeedSet(textBoxSeedSet.Text);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            progressBarSimulationLoop.Show();
            backgroundWorkerSimulationLoop.RunWorkerAsync();
        }

        private void buttonPlot_Click(object sender, EventArgs e)
        {
            _wirelessNetworkController.Plot();
        }

        private void backgroundWorkerSimulationLoop_DoWork(object sender, DoWorkEventArgs e)
        {
            DisableControls();

            _wirelessNetworkController.Run();
        }

        private void DisableControls()
        {
          
                textBoxSimulationTime.Enabled = false;
                textBoxLambda.Enabled = false;
                textBoxSeedSet.Enabled = false;
                buttonRun.Enabled = false;        
        }

        private void backgroundWorkerSimulationLoop_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarSimulationLoop.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerSimulationLoop_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _wirelessNetworkController.Plot();
            progressBarSimulationLoop.Hide();
            EnableControls();
        
        }

        private void EnableControls()
        {
            textBoxSimulationTime.Enabled = true;
            textBoxLambda.Enabled = true;
            textBoxSeedSet.Enabled = true;
            buttonRun.Enabled = true;
        }

        private void checkBoxEnableLogger_CheckedChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetEnableLogger(checkBoxEnableLogger.Checked);
        }
    }
}
