namespace WirelessNetworkSymulationView
{
    partial class SimulationView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationView));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.singleRun = new System.Windows.Forms.TabPage();
            this.progressBarSimulationLoop = new System.Windows.Forms.ProgressBar();
            this.buttonSteadyStateAnalysis = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.labelSeedSet = new System.Windows.Forms.Label();
            this.labelLambda = new System.Windows.Forms.Label();
            this.labelSimulationTime = new System.Windows.Forms.Label();
            this.textBoxSeedSet = new System.Windows.Forms.TextBox();
            this.textBoxLambda = new System.Windows.Forms.TextBox();
            this.textBoxSimulationTime = new System.Windows.Forms.TextBox();
            this.checkBoxEnableLogger = new System.Windows.Forms.CheckBox();
            this.chartSteadyState = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.generatorsAnalyTabPage = new System.Windows.Forms.TabPage();
            this.chartUniformGeneratorAnalysis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.backgroundWorkerSimulationLoop = new System.ComponentModel.BackgroundWorker();
            this.chartExponentialGeneratorAnalysis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelParameters = new System.Windows.Forms.Label();
            this.textBoxUniformGeneratorLowBound = new System.Windows.Forms.TextBox();
            this.textBoxUniformGeneratorUpBound = new System.Windows.Forms.TextBox();
            this.labelLowBound = new System.Windows.Forms.Label();
            this.labelUpBound = new System.Windows.Forms.Label();
            this.labelUniformGenerator = new System.Windows.Forms.Label();
            this.labelSamplesNumber = new System.Windows.Forms.Label();
            this.labelGeneratorAnalysisSeedSet = new System.Windows.Forms.Label();
            this.textBoxSamplesNumber = new System.Windows.Forms.TextBox();
            this.textBoxGeneratorAnalysisSeedSet = new System.Windows.Forms.TextBox();
            this.labelExpGenerator = new System.Windows.Forms.Label();
            this.labelGeneratorAnalysisLambda = new System.Windows.Forms.Label();
            this.textBoxGeneratorAnalysisLambda = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.singleRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyState)).BeginInit();
            this.generatorsAnalyTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartUniformGeneratorAnalysis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartExponentialGeneratorAnalysis)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.singleRun);
            this.tabControl.Controls.Add(this.generatorsAnalyTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(749, 306);
            this.tabControl.TabIndex = 2;
            // 
            // singleRun
            // 
            this.singleRun.Controls.Add(this.progressBarSimulationLoop);
            this.singleRun.Controls.Add(this.buttonSteadyStateAnalysis);
            this.singleRun.Controls.Add(this.buttonRun);
            this.singleRun.Controls.Add(this.labelSeedSet);
            this.singleRun.Controls.Add(this.labelLambda);
            this.singleRun.Controls.Add(this.labelSimulationTime);
            this.singleRun.Controls.Add(this.textBoxSeedSet);
            this.singleRun.Controls.Add(this.textBoxLambda);
            this.singleRun.Controls.Add(this.textBoxSimulationTime);
            this.singleRun.Controls.Add(this.checkBoxEnableLogger);
            this.singleRun.Controls.Add(this.chartSteadyState);
            this.singleRun.Location = new System.Drawing.Point(4, 22);
            this.singleRun.Name = "singleRun";
            this.singleRun.Padding = new System.Windows.Forms.Padding(3);
            this.singleRun.Size = new System.Drawing.Size(741, 280);
            this.singleRun.TabIndex = 0;
            this.singleRun.Text = "Single Run";
            this.singleRun.UseVisualStyleBackColor = true;
            // 
            // progressBarSimulationLoop
            // 
            this.progressBarSimulationLoop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBarSimulationLoop.Location = new System.Drawing.Point(184, 220);
            this.progressBarSimulationLoop.MarqueeAnimationSpeed = 20;
            this.progressBarSimulationLoop.Name = "progressBarSimulationLoop";
            this.progressBarSimulationLoop.Size = new System.Drawing.Size(527, 23);
            this.progressBarSimulationLoop.TabIndex = 10;
            // 
            // buttonSteadyStateAnalysis
            // 
            this.buttonSteadyStateAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSteadyStateAnalysis.Location = new System.Drawing.Point(19, 233);
            this.buttonSteadyStateAnalysis.Name = "buttonSteadyStateAnalysis";
            this.buttonSteadyStateAnalysis.Size = new System.Drawing.Size(159, 23);
            this.buttonSteadyStateAnalysis.TabIndex = 9;
            this.buttonSteadyStateAnalysis.Text = "analize steady state";
            this.buttonSteadyStateAnalysis.UseVisualStyleBackColor = true;
            this.buttonSteadyStateAnalysis.Click += new System.EventHandler(this.buttonSteadyStateAnalysis_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRun.Location = new System.Drawing.Point(19, 204);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(159, 23);
            this.buttonRun.TabIndex = 8;
            this.buttonRun.Text = "run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // labelSeedSet
            // 
            this.labelSeedSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSeedSet.AutoSize = true;
            this.labelSeedSet.Location = new System.Drawing.Point(532, 174);
            this.labelSeedSet.Name = "labelSeedSet";
            this.labelSeedSet.Size = new System.Drawing.Size(47, 13);
            this.labelSeedSet.TabIndex = 7;
            this.labelSeedSet.Text = "seed set";
            // 
            // labelLambda
            // 
            this.labelLambda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLambda.AutoSize = true;
            this.labelLambda.Location = new System.Drawing.Point(401, 174);
            this.labelLambda.Name = "labelLambda";
            this.labelLambda.Size = new System.Drawing.Size(41, 13);
            this.labelLambda.TabIndex = 6;
            this.labelLambda.Text = "lambda";
            // 
            // labelSimulationTime
            // 
            this.labelSimulationTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSimulationTime.AutoSize = true;
            this.labelSimulationTime.Location = new System.Drawing.Point(214, 174);
            this.labelSimulationTime.Name = "labelSimulationTime";
            this.labelSimulationTime.Size = new System.Drawing.Size(97, 13);
            this.labelSimulationTime.TabIndex = 5;
            this.labelSimulationTime.Text = "simulation time [ms]";
            // 
            // textBoxSeedSet
            // 
            this.textBoxSeedSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSeedSet.Location = new System.Drawing.Point(448, 171);
            this.textBoxSeedSet.Name = "textBoxSeedSet";
            this.textBoxSeedSet.Size = new System.Drawing.Size(78, 20);
            this.textBoxSeedSet.TabIndex = 4;
            this.textBoxSeedSet.TextChanged += new System.EventHandler(this.textBoxSeedSet_TextChanged);
            // 
            // textBoxLambda
            // 
            this.textBoxLambda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxLambda.Location = new System.Drawing.Point(317, 171);
            this.textBoxLambda.Name = "textBoxLambda";
            this.textBoxLambda.Size = new System.Drawing.Size(78, 20);
            this.textBoxLambda.TabIndex = 3;
            this.textBoxLambda.TextChanged += new System.EventHandler(this.textBoxLambda_TextChanged);
            // 
            // textBoxSimulationTime
            // 
            this.textBoxSimulationTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxSimulationTime.Location = new System.Drawing.Point(130, 171);
            this.textBoxSimulationTime.Name = "textBoxSimulationTime";
            this.textBoxSimulationTime.Size = new System.Drawing.Size(78, 20);
            this.textBoxSimulationTime.TabIndex = 2;
            this.textBoxSimulationTime.TextChanged += new System.EventHandler(this.textBoxSimulationTime_TextChanged);
            // 
            // checkBoxEnableLogger
            // 
            this.checkBoxEnableLogger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnableLogger.AutoSize = true;
            this.checkBoxEnableLogger.Location = new System.Drawing.Point(34, 174);
            this.checkBoxEnableLogger.Name = "checkBoxEnableLogger";
            this.checkBoxEnableLogger.Size = new System.Drawing.Size(90, 17);
            this.checkBoxEnableLogger.TabIndex = 1;
            this.checkBoxEnableLogger.Text = "enable logger";
            this.checkBoxEnableLogger.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogger.CheckedChanged += new System.EventHandler(this.checkBoxEnableLogger_CheckedChanged);
            // 
            // chartSteadyState
            // 
            this.chartSteadyState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartSteadyState.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSteadyState.Legends.Add(legend1);
            this.chartSteadyState.Location = new System.Drawing.Point(6, 6);
            this.chartSteadyState.Name = "chartSteadyState";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "error mean";
            this.chartSteadyState.Series.Add(series1);
            this.chartSteadyState.Size = new System.Drawing.Size(657, 161);
            this.chartSteadyState.TabIndex = 0;
            this.chartSteadyState.Text = "chart1";
            // 
            // generatorsAnalyTabPage
            // 
            this.generatorsAnalyTabPage.Controls.Add(this.labelGeneratorAnalysisLambda);
            this.generatorsAnalyTabPage.Controls.Add(this.textBoxGeneratorAnalysisLambda);
            this.generatorsAnalyTabPage.Controls.Add(this.labelExpGenerator);
            this.generatorsAnalyTabPage.Controls.Add(this.labelSamplesNumber);
            this.generatorsAnalyTabPage.Controls.Add(this.labelGeneratorAnalysisSeedSet);
            this.generatorsAnalyTabPage.Controls.Add(this.textBoxSamplesNumber);
            this.generatorsAnalyTabPage.Controls.Add(this.textBoxGeneratorAnalysisSeedSet);
            this.generatorsAnalyTabPage.Controls.Add(this.labelUniformGenerator);
            this.generatorsAnalyTabPage.Controls.Add(this.labelUpBound);
            this.generatorsAnalyTabPage.Controls.Add(this.labelLowBound);
            this.generatorsAnalyTabPage.Controls.Add(this.textBoxUniformGeneratorUpBound);
            this.generatorsAnalyTabPage.Controls.Add(this.textBoxUniformGeneratorLowBound);
            this.generatorsAnalyTabPage.Controls.Add(this.labelParameters);
            this.generatorsAnalyTabPage.Controls.Add(this.chartExponentialGeneratorAnalysis);
            this.generatorsAnalyTabPage.Controls.Add(this.chartUniformGeneratorAnalysis);
            this.generatorsAnalyTabPage.Location = new System.Drawing.Point(4, 22);
            this.generatorsAnalyTabPage.Name = "generatorsAnalyTabPage";
            this.generatorsAnalyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generatorsAnalyTabPage.Size = new System.Drawing.Size(741, 280);
            this.generatorsAnalyTabPage.TabIndex = 1;
            this.generatorsAnalyTabPage.Text = "Random Generators analysis";
            this.generatorsAnalyTabPage.UseVisualStyleBackColor = true;
            // 
            // chartUniformGeneratorAnalysis
            // 
            this.chartUniformGeneratorAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.chartUniformGeneratorAnalysis.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartUniformGeneratorAnalysis.Legends.Add(legend3);
            this.chartUniformGeneratorAnalysis.Location = new System.Drawing.Point(6, 6);
            this.chartUniformGeneratorAnalysis.Name = "chartUniformGeneratorAnalysis";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Uniform Generator";
            this.chartUniformGeneratorAnalysis.Series.Add(series3);
            this.chartUniformGeneratorAnalysis.Size = new System.Drawing.Size(357, 141);
            this.chartUniformGeneratorAnalysis.TabIndex = 0;
            this.chartUniformGeneratorAnalysis.Text = "chart1";
            // 
            // backgroundWorkerSimulationLoop
            // 
            this.backgroundWorkerSimulationLoop.WorkerReportsProgress = true;
            this.backgroundWorkerSimulationLoop.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSimulationLoop_DoWork);
            this.backgroundWorkerSimulationLoop.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerSimulationLoop_ProgressChanged);
            this.backgroundWorkerSimulationLoop.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSimulationLoop_RunWorkerCompleted);
            // 
            // chartExponentialGeneratorAnalysis
            // 
            this.chartExponentialGeneratorAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartExponentialGeneratorAnalysis.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartExponentialGeneratorAnalysis.Legends.Add(legend2);
            this.chartExponentialGeneratorAnalysis.Location = new System.Drawing.Point(390, 6);
            this.chartExponentialGeneratorAnalysis.Name = "chartExponentialGeneratorAnalysis";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Exp generator";
            this.chartExponentialGeneratorAnalysis.Series.Add(series2);
            this.chartExponentialGeneratorAnalysis.Size = new System.Drawing.Size(345, 141);
            this.chartExponentialGeneratorAnalysis.TabIndex = 2;
            this.chartExponentialGeneratorAnalysis.Text = "chart1";
            // 
            // labelParameters
            // 
            this.labelParameters.AutoSize = true;
            this.labelParameters.Location = new System.Drawing.Point(355, 176);
            this.labelParameters.Name = "labelParameters";
            this.labelParameters.Size = new System.Drawing.Size(63, 13);
            this.labelParameters.TabIndex = 3;
            this.labelParameters.Text = "Parameters:";
            // 
            // textBoxUniformGeneratorLowBound
            // 
            this.textBoxUniformGeneratorLowBound.Location = new System.Drawing.Point(44, 202);
            this.textBoxUniformGeneratorLowBound.Name = "textBoxUniformGeneratorLowBound";
            this.textBoxUniformGeneratorLowBound.Size = new System.Drawing.Size(60, 20);
            this.textBoxUniformGeneratorLowBound.TabIndex = 4;
            // 
            // textBoxUniformGeneratorUpBound
            // 
            this.textBoxUniformGeneratorUpBound.Location = new System.Drawing.Point(44, 237);
            this.textBoxUniformGeneratorUpBound.Name = "textBoxUniformGeneratorUpBound";
            this.textBoxUniformGeneratorUpBound.Size = new System.Drawing.Size(60, 20);
            this.textBoxUniformGeneratorUpBound.TabIndex = 5;
            // 
            // labelLowBound
            // 
            this.labelLowBound.AutoSize = true;
            this.labelLowBound.Location = new System.Drawing.Point(110, 205);
            this.labelLowBound.Name = "labelLowBound";
            this.labelLowBound.Size = new System.Drawing.Size(56, 13);
            this.labelLowBound.TabIndex = 6;
            this.labelLowBound.Text = "low bound";
            // 
            // labelUpBound
            // 
            this.labelUpBound.AutoSize = true;
            this.labelUpBound.Location = new System.Drawing.Point(110, 240);
            this.labelUpBound.Name = "labelUpBound";
            this.labelUpBound.Size = new System.Drawing.Size(52, 13);
            this.labelUpBound.TabIndex = 7;
            this.labelUpBound.Text = "up bound";
            // 
            // labelUniformGenerator
            // 
            this.labelUniformGenerator.AutoSize = true;
            this.labelUniformGenerator.Location = new System.Drawing.Point(41, 176);
            this.labelUniformGenerator.Name = "labelUniformGenerator";
            this.labelUniformGenerator.Size = new System.Drawing.Size(93, 13);
            this.labelUniformGenerator.TabIndex = 8;
            this.labelUniformGenerator.Text = "Uniform Generator";
            // 
            // labelSamplesNumber
            // 
            this.labelSamplesNumber.AutoSize = true;
            this.labelSamplesNumber.Location = new System.Drawing.Point(424, 240);
            this.labelSamplesNumber.Name = "labelSamplesNumber";
            this.labelSamplesNumber.Size = new System.Drawing.Size(83, 13);
            this.labelSamplesNumber.TabIndex = 12;
            this.labelSamplesNumber.Text = "samples number";
            // 
            // labelGeneratorAnalysisSeedSet
            // 
            this.labelGeneratorAnalysisSeedSet.AutoSize = true;
            this.labelGeneratorAnalysisSeedSet.Location = new System.Drawing.Point(424, 205);
            this.labelGeneratorAnalysisSeedSet.Name = "labelGeneratorAnalysisSeedSet";
            this.labelGeneratorAnalysisSeedSet.Size = new System.Drawing.Size(47, 13);
            this.labelGeneratorAnalysisSeedSet.TabIndex = 11;
            this.labelGeneratorAnalysisSeedSet.Text = "seed set";
            // 
            // textBoxSamplesNumber
            // 
            this.textBoxSamplesNumber.Location = new System.Drawing.Point(358, 237);
            this.textBoxSamplesNumber.Name = "textBoxSamplesNumber";
            this.textBoxSamplesNumber.Size = new System.Drawing.Size(60, 20);
            this.textBoxSamplesNumber.TabIndex = 10;
            // 
            // textBoxGeneratorAnalysisSeedSet
            // 
            this.textBoxGeneratorAnalysisSeedSet.Location = new System.Drawing.Point(358, 202);
            this.textBoxGeneratorAnalysisSeedSet.Name = "textBoxGeneratorAnalysisSeedSet";
            this.textBoxGeneratorAnalysisSeedSet.Size = new System.Drawing.Size(60, 20);
            this.textBoxGeneratorAnalysisSeedSet.TabIndex = 9;
            // 
            // labelExpGenerator
            // 
            this.labelExpGenerator.AutoSize = true;
            this.labelExpGenerator.Location = new System.Drawing.Point(216, 176);
            this.labelExpGenerator.Name = "labelExpGenerator";
            this.labelExpGenerator.Size = new System.Drawing.Size(75, 13);
            this.labelExpGenerator.TabIndex = 13;
            this.labelExpGenerator.Text = "Exp Generator";
            // 
            // labelGeneratorAnalysisLambda
            // 
            this.labelGeneratorAnalysisLambda.AutoSize = true;
            this.labelGeneratorAnalysisLambda.Location = new System.Drawing.Point(285, 205);
            this.labelGeneratorAnalysisLambda.Name = "labelGeneratorAnalysisLambda";
            this.labelGeneratorAnalysisLambda.Size = new System.Drawing.Size(45, 13);
            this.labelGeneratorAnalysisLambda.TabIndex = 15;
            this.labelGeneratorAnalysisLambda.Text = "Lambda";
            // 
            // textBoxGeneratorAnalysisLambda
            // 
            this.textBoxGeneratorAnalysisLambda.Location = new System.Drawing.Point(219, 202);
            this.textBoxGeneratorAnalysisLambda.Name = "textBoxGeneratorAnalysisLambda";
            this.textBoxGeneratorAnalysisLambda.Size = new System.Drawing.Size(60, 20);
            this.textBoxGeneratorAnalysisLambda.TabIndex = 14;
            // 
            // SimulationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 330);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimulationView";
            this.Text = "SymulationView";
            this.tabControl.ResumeLayout(false);
            this.singleRun.ResumeLayout(false);
            this.singleRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyState)).EndInit();
            this.generatorsAnalyTabPage.ResumeLayout(false);
            this.generatorsAnalyTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartUniformGeneratorAnalysis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartExponentialGeneratorAnalysis)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage singleRun;
        private System.Windows.Forms.TabPage generatorsAnalyTabPage;
        private System.Windows.Forms.CheckBox checkBoxEnableLogger;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSteadyState;
        private System.Windows.Forms.TextBox textBoxLambda;
        private System.Windows.Forms.TextBox textBoxSimulationTime;
        private System.Windows.Forms.Label labelSeedSet;
        private System.Windows.Forms.Label labelLambda;
        private System.Windows.Forms.Label labelSimulationTime;
        private System.Windows.Forms.TextBox textBoxSeedSet;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonSteadyStateAnalysis;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSimulationLoop;
        private System.Windows.Forms.ProgressBar progressBarSimulationLoop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUniformGeneratorAnalysis;
        private System.Windows.Forms.Label labelGeneratorAnalysisLambda;
        private System.Windows.Forms.TextBox textBoxGeneratorAnalysisLambda;
        private System.Windows.Forms.Label labelExpGenerator;
        private System.Windows.Forms.Label labelSamplesNumber;
        private System.Windows.Forms.Label labelGeneratorAnalysisSeedSet;
        private System.Windows.Forms.TextBox textBoxSamplesNumber;
        private System.Windows.Forms.TextBox textBoxGeneratorAnalysisSeedSet;
        private System.Windows.Forms.Label labelUniformGenerator;
        private System.Windows.Forms.Label labelUpBound;
        private System.Windows.Forms.Label labelLowBound;
        private System.Windows.Forms.TextBox textBoxUniformGeneratorUpBound;
        private System.Windows.Forms.TextBox textBoxUniformGeneratorLowBound;
        private System.Windows.Forms.Label labelParameters;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartExponentialGeneratorAnalysis;
    }
}