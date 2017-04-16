using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WirelessNetworkSymulationController;

namespace WirelessNetworkSymulationView
{
    public partial class SimulationView : Form, IWirelessNetworkView
    {
        private WirelessNetworkController _wirelessNetworkController;
        private Thread t;
        public SimulationView()
        {
            InitializeComponent();
        
        }

        enum BackgroundWorkerMode
        {
            SingleLoop,
            SteadyStateAnalysis
        }

        #region IWirelessNetworkView implementation

        public void SetController(WirelessNetworkController controller)
        {
            _wirelessNetworkController = controller;
        }

        public void PlotSteadyState(List<double> times, List<double> means)
        {
            if (times == null || means == null)
                return;
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

        public void DisableControls()
        {

            textBoxSimulationTime.Enabled = false;
            textBoxLambda.Enabled = false;
            textBoxSeedSet.Enabled = false;
            buttonRun.Enabled = false;
            buttonSteadyStateAnalysis.Enabled = false;
            checkBoxEnableLogger.Enabled = false;
        }

        public void EnableControls()
        {
            textBoxSimulationTime.Enabled = true;
            textBoxLambda.Enabled = true;
            textBoxSeedSet.Enabled = true;
            buttonRun.Enabled = true;
            buttonSteadyStateAnalysis.Enabled = true;
            checkBoxEnableLogger.Enabled = true;
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
            DisableControls();  
            backgroundWorkerSimulationLoop.RunWorkerAsync(BackgroundWorkerMode.SingleLoop);
        }

        private void buttonSteadyStateAnalysis_Click(object sender, EventArgs e)
        {
            DisableControls();
           backgroundWorkerSimulationLoop.RunWorkerAsync(BackgroundWorkerMode.SteadyStateAnalysis);
        }

        private void backgroundWorkerSimulationLoop_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorkerMode mode = (BackgroundWorkerMode) e.Argument;
            switch (mode)
            {
                case BackgroundWorkerMode.SingleLoop:
                    _wirelessNetworkController.Run();
                    break;
                case BackgroundWorkerMode.SteadyStateAnalysis:
                    _wirelessNetworkController.SteadyStateAnalysis();
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

   

        private void backgroundWorkerSimulationLoop_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarSimulationLoop.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerSimulationLoop_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _wirelessNetworkController.Plot();
            EnableControls();

        }

     

        private void checkBoxEnableLogger_CheckedChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetEnableLogger(checkBoxEnableLogger.Checked);
        }

    }
}
