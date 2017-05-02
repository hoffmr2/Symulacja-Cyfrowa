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
using System.Windows.Forms.DataVisualization.Charting;
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

        enum BackgroundWorkerMode
        {
            SingleLoop,
            SteadyStateAnalysis,
            RandomGeneratorsAnalysis
        }

        #region IWirelessNetworkView implementation

        public void SetOutputText(string text)
        {
            richTextBoxOutput.Text = text;
        }

        public void PlotUniformGeneratorHistogram(SortedDictionary<double, int> data)
        {
            if (data == null)
                return;
            chartUniformGeneratorAnalysis.Series["Uniform Generator"].Points.Clear();
            foreach(var pair in data)
            {
                chartUniformGeneratorAnalysis.Series["Uniform Generator"].Points.AddXY(pair.Key, pair.Value);
            }
        }

        public void PlotExpGeneratorHistogram(SortedDictionary<double, int> data)
        {
            if (data == null)
                return;
            chartExponentialGeneratorAnalysis.Series["Exp generator"].Points.Clear();
            foreach (var pair in data)
            {
                chartExponentialGeneratorAnalysis.Series["Exp generator"].Points.AddXY(pair.Key, pair.Value);
            }
        }

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
                chartSteadyState.Series["error mean"].Points.AddXY(times[i]/10, means[i]);
            }
            chartSteadyState.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "7","error mean","error mean");
         
         //   chartSteadyState.DataBind();
            
        }

        public BackgroundWorker GetBackgroundWorker()
        {
            return backgroundWorkerSimulationLoop;
        }

        public void DisableControls()
        {
            DisableSingleRunTabControls();
            DisableGeneratorAnalysisTabControls();
        }

        private void DisableGeneratorAnalysisTabControls()
        {
            textBoxGeneratorAnalysisLambda.Enabled = false;
            textBoxGeneratorAnalysisSeedSet.Enabled = false;
            textBoxSamplesNumber.Enabled = false;
            textBoxUniformGeneratorLowBound.Enabled = false;
            textBoxUniformGeneratorUpBound.Enabled = false;
            buttonAnalyzeGenerators.Enabled = false;
        }

        private void DisableSingleRunTabControls()
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
            EnableSingleRunTabControls();
            EnableGeneratorAnalysisTabControls();
        }

        private void EnableGeneratorAnalysisTabControls()
        {
            textBoxGeneratorAnalysisLambda.Enabled = true;
            textBoxGeneratorAnalysisSeedSet.Enabled = true;
            textBoxSamplesNumber.Enabled = true;
            textBoxUniformGeneratorLowBound.Enabled = true;
            textBoxUniformGeneratorUpBound.Enabled = true;
            buttonAnalyzeGenerators.Enabled = true;
        }

        private void EnableSingleRunTabControls()
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
                case BackgroundWorkerMode.RandomGeneratorsAnalysis:
                    _wirelessNetworkController.RandomGeneratorsAnalysis();
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
            _wirelessNetworkController.PlotGeneratorsHistograms();
            _wirelessNetworkController.SetOutputText();
            EnableControls();

        }

     

        private void checkBoxEnableLogger_CheckedChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetEnableLogger(checkBoxEnableLogger.Checked);
        }

        private void textBoxUniformGeneratorLowBound_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetGeneratorAnalysisDownBound(textBoxUniformGeneratorLowBound.Text);
        }

        private void textBoxUniformGeneratorUpBound_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetGeneratorAnalysisUpBound(textBoxUniformGeneratorUpBound.Text);
        }

        private void textBoxGeneratorAnalysisLambda_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetGeneratorAnalysisLambda(textBoxGeneratorAnalysisLambda.Text);
        }

        private void textBoxGeneratorAnalysisSeedSet_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetGeneratorAnalysisSeedSet(textBoxGeneratorAnalysisSeedSet.Text);
        }

        private void textBoxSamplesNumber_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetGeneratorAnalysisSamplesNumber(textBoxSamplesNumber.Text);
        }

        private void buttonAnalyzeGenerators_Click(object sender, EventArgs e)
        {
            DisableControls();
            backgroundWorkerSimulationLoop.RunWorkerAsync(BackgroundWorkerMode.RandomGeneratorsAnalysis);
        }
    }
}
