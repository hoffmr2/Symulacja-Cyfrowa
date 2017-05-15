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
using WirelessNetworkComponents;
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
            RandomGeneratorsAnalysis,
            LambdaAnalysis,
            MainSimulation
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

        public void PlotSteadyState(List<double> times, List<double> means, string series)
        {
            if (times == null || means == null)
                return;
            if(chartSteadyState.Series.FindByName(series) == null)
                chartSteadyState.Series.Add(new Series( series));
            chartSteadyState.Series[series].ChartType = SeriesChartType.FastLine;
            chartSteadyState.Series[series].XValueType = ChartValueType.Int32;
            
           chartSteadyState.Series[series].Points.Clear();
            for(int i=0;i< times.Count;++i)
            {
                chartSteadyState.Series[series].Points.AddXY(times[i], means[i]);
            }
            chartSteadyState.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage,"3",series,series);
            //  chartSteadyState.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "lambda0,001");
            //   chartSteadyState.DataBind();

        }

        public void PlotLambda(List<double> xValues, List<SimulationResults> yValues)
        {
            if (xValues == null || yValues == null)
                return;
            ClearCharts();
            string name;
            string name2;
            string name3;
            string nameFlowChart;
            string nameMaxFailsChart;
            InitChartsParameters(out name, out name2, out name3, out nameFlowChart, out nameMaxFailsChart);

            SetChartsPoints(xValues, yValues, name, name2, name3, nameFlowChart, nameMaxFailsChart);
        }

        private void SetChartsPoints(List<double> xValues, List<SimulationResults> yValues, string name, string name2, string name3, string nameFlowChart,
            string nameMaxFailsChart)
        {
            chartLambdaAnalysis.Series[name].Points.Clear();
            chartLambdaAnalysis.Series[name2].Points.Clear();
            chartLambdaAnalysis.Series[name3].Points.Clear();
            for (int i = 0; i < xValues.Count; ++i)
            {
                var yPoint = yValues[i];
                chartLambdaAnalysis.Series[name].Points.AddXY(xValues[i], yPoint.LostPackagesMean, yPoint.ErrorLowBound,
                    yPoint.ErrorUpBound);
                chartLambdaAnalysis.Series[name2].Points.AddXY(xValues[i], 0.1);
                chartLambdaAnalysis.Series[name3].Points.AddXY(xValues[i], yPoint.LostPackagesMean);
                chartAverageFlow.Series[nameFlowChart].Points.AddXY(xValues[i], yPoint.Flow);
                chartMaxError.Series[nameMaxFailsChart].Points.AddXY(xValues[i], yPoint.MaxLostPackagesRatio);
            }
        }

        private void InitChartsParameters(out string name, out string name2, out string name3, out string nameFlowChart,
            out string nameMaxFailsChart)
        {
            name = "Lambda Analysis";
            name2 = "max error";
            name3 = "lambda aproximation";
            nameFlowChart = "System Flow";
            nameMaxFailsChart = "Max Fails Ratio";
            chartLambdaAnalysis.Series.Add(new Series(name));
            chartLambdaAnalysis.Series.Add(new Series(name2));
            chartLambdaAnalysis.Series.Add(new Series(name3));
            chartAverageFlow.Series.Add(new Series(nameFlowChart));
            chartMaxError.Series.Add(new Series(nameMaxFailsChart));
            chartLambdaAnalysis.Series[name].ChartType = SeriesChartType.ErrorBar;
            chartLambdaAnalysis.Series[name].XValueType = ChartValueType.Double;
            chartLambdaAnalysis.Series[name2].ChartType = SeriesChartType.FastLine;
            chartLambdaAnalysis.Series[name2].XValueType = ChartValueType.Double;
            chartLambdaAnalysis.Series[name2].BorderWidth = 5;
            chartLambdaAnalysis.Series[name3].ChartType = SeriesChartType.FastLine;
            chartLambdaAnalysis.Series[name3].XValueType = ChartValueType.Double;
            chartAverageFlow.Series[nameFlowChart].ChartType = SeriesChartType.FastLine;
            chartAverageFlow.Series[nameFlowChart].XValueType = ChartValueType.Double;
            chartMaxError.Series[nameMaxFailsChart].ChartType = SeriesChartType.FastLine;
            chartMaxError.Series[nameMaxFailsChart].XValueType = ChartValueType.Double;
        }

        private void ClearCharts()
        {
            chartLambdaAnalysis.Series.Clear();
            chartAverageFlow.Series.Clear();
            chartMaxError.Series.Clear();
        }

        public void ClearSteadyStatePlot()
        {
            chartSteadyState.Series.Clear();
        }


        public BackgroundWorker GetBackgroundWorker()
        {
            return backgroundWorkerSimulationLoop;
        }

        public void DisableControls()
        {
            DisableSingleRunTabControls();
            DisableGeneratorAnalysisTabControls();
            DisableLambdaAnalysisTabControls();
        }

        private void DisableLambdaAnalysisTabControls()
        {
            textBoxEndLambda.Enabled = false;
            textBoxStartLambda.Enabled = false;
            buttonLambdaAnalysis.Enabled = false;
            buttonSaveLambdaAnalysis.Enabled = false;
            buttonSaveAverageFlow.Enabled = false;
            buttonSaveMaxError.Enabled = false;
        }

        private void DisableGeneratorAnalysisTabControls()
        {
            textBoxGeneratorAnalysisLambda.Enabled = false;
            textBoxGeneratorAnalysisSeedSet.Enabled = false;
            textBoxSamplesNumber.Enabled = false;
            textBoxUniformGeneratorLowBound.Enabled = false;
            textBoxUniformGeneratorUpBound.Enabled = false;
            buttonAnalyzeGenerators.Enabled = false;
            buttonSaveExponentialPlot.Enabled = false;
            buttonSaveUniformPlot.Enabled = false;
        }

        private void DisableSingleRunTabControls()
        {
            textBoxSimulationTime.Enabled = false;
            textBoxLambda.Enabled = false;
            textBoxSeedSet.Enabled = false;
            textBoxTransmissions.Enabled = false;
            buttonRun.Enabled = false;
            buttonSteadyStateAnalysis.Enabled = false;
            buttonSaveSteadyState.Enabled = false;
            buttonClear.Enabled = false;
            buttonMainSimulation.Enabled = false;
            checkBoxEnableLogger.Enabled = false;
        }

        public void EnableControls()
        {
            EnableSingleRunTabControls();
            EnableGeneratorAnalysisTabControls();
            EnableLambdaAnalysisTabControls();
        }

        private void EnableLambdaAnalysisTabControls()
        {
            textBoxEndLambda.Enabled = true;
            textBoxStartLambda.Enabled = true;
            buttonLambdaAnalysis.Enabled = true;
            buttonSaveLambdaAnalysis.Enabled = true;
            buttonSaveAverageFlow.Enabled = true;
            buttonSaveMaxError.Enabled = true;
        }

        private void EnableGeneratorAnalysisTabControls()
        {
            textBoxGeneratorAnalysisLambda.Enabled = true;
            textBoxGeneratorAnalysisSeedSet.Enabled = true;
            textBoxSamplesNumber.Enabled = true;
            textBoxUniformGeneratorLowBound.Enabled = true;
            textBoxUniformGeneratorUpBound.Enabled = true;
            buttonAnalyzeGenerators.Enabled = true;
            buttonSaveExponentialPlot.Enabled = true;
            buttonSaveUniformPlot.Enabled = true;
        }

        private void EnableSingleRunTabControls()
        {
            textBoxSimulationTime.Enabled = true;
            textBoxLambda.Enabled = true;
            textBoxSeedSet.Enabled = true;
            textBoxTransmissions.Enabled = true;
            buttonRun.Enabled = true;
            buttonSteadyStateAnalysis.Enabled = true;
            buttonSaveSteadyState.Enabled = true;
            buttonClear.Enabled = true;
            buttonMainSimulation.Enabled = true;
            checkBoxEnableLogger.Enabled = true;
        }

        public void SavePlot(string path, object chart)
        {
            try
            {
                (chart as Chart).SaveImage(path,ChartImageFormat.Png);
            }
            catch (Exception e)
            {
                throw;
            }
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
                case BackgroundWorkerMode.LambdaAnalysis:
                    _wirelessNetworkController.LambdaAnalysis();
                    break;
                case BackgroundWorkerMode.MainSimulation:
                        _wirelessNetworkController.MainSimulation();
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
            _wirelessNetworkController.PlotLambdaAnalysis();
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

        private void textBoxTransmissions_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetMaxTransmissions(textBoxTransmissions.Text);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
           ClearSteadyStatePlot();
        }

        private void buttonSaveSteadyState_Click(object sender, EventArgs e)
        {
           saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
               SavePlot(path+".png",chartSteadyState);
            }
        }

        private void buttonLambdaAnalysis_Click(object sender, EventArgs e)
        {
            DisableControls();
            backgroundWorkerSimulationLoop.RunWorkerAsync(BackgroundWorkerMode.LambdaAnalysis);
        }

        private void textBoxStartLambda_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetStartLambda(textBoxStartLambda.Text);
        }

        private void textBoxEndLambda_TextChanged(object sender, EventArgs e)
        {
            _wirelessNetworkController.SetEndLambda(textBoxEndLambda.Text);
        }

        private void buttonSaveLambdaAnalysis_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                SavePlot(path + ".png", chartLambdaAnalysis);
            }
        }

        private void buttonSaveMaxError_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                SavePlot(path + ".png", chartMaxError);
            }
        }

        private void buttonSaveAverageFlow_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                SavePlot(path + ".png", chartAverageFlow);
            }
        }

        private void buttonSaveUniformPlot_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                SavePlot(path + ".png", chartUniformGeneratorAnalysis);
            }
        }

        private void buttonSaveExponentialPlot_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                SavePlot(path + ".png", chartExponentialGeneratorAnalysis);
            }
        }

        private void buttonMainSimulation_Click(object sender, EventArgs e)
        {
            DisableControls();
            var path = saveFileDialog.FileName;
            if (path != "")
            {
                _wirelessNetworkController.SetExcelOutPath(path);
            }
            backgroundWorkerSimulationLoop.RunWorkerAsync(BackgroundWorkerMode.MainSimulation);
        }
    }
}
