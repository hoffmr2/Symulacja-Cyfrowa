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
            this.multiRun = new System.Windows.Forms.TabPage();
            this.chartSteadyStateAnalysis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.backgroundWorkerSimulationLoop = new System.ComponentModel.BackgroundWorker();
            this.tabControl.SuspendLayout();
            this.singleRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyState)).BeginInit();
            this.multiRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyStateAnalysis)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.singleRun);
            this.tabControl.Controls.Add(this.multiRun);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(749, 417);
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
            this.singleRun.Size = new System.Drawing.Size(741, 391);
            this.singleRun.TabIndex = 0;
            this.singleRun.Text = "Single Run";
            this.singleRun.UseVisualStyleBackColor = true;
            // 
            // progressBarSimulationLoop
            // 
            this.progressBarSimulationLoop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarSimulationLoop.Location = new System.Drawing.Point(208, 305);
            this.progressBarSimulationLoop.MarqueeAnimationSpeed = 20;
            this.progressBarSimulationLoop.Name = "progressBarSimulationLoop";
            this.progressBarSimulationLoop.Size = new System.Drawing.Size(527, 23);
            this.progressBarSimulationLoop.TabIndex = 10;
            // 
            // buttonSteadyStateAnalysis
            // 
            this.buttonSteadyStateAnalysis.Location = new System.Drawing.Point(339, 197);
            this.buttonSteadyStateAnalysis.Name = "buttonSteadyStateAnalysis";
            this.buttonSteadyStateAnalysis.Size = new System.Drawing.Size(159, 23);
            this.buttonSteadyStateAnalysis.TabIndex = 9;
            this.buttonSteadyStateAnalysis.Text = "analize steady state";
            this.buttonSteadyStateAnalysis.UseVisualStyleBackColor = true;
            this.buttonSteadyStateAnalysis.Click += new System.EventHandler(this.buttonSteadyStateAnalysis_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(34, 305);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(159, 23);
            this.buttonRun.TabIndex = 8;
            this.buttonRun.Text = "run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // labelSeedSet
            // 
            this.labelSeedSet.AutoSize = true;
            this.labelSeedSet.Location = new System.Drawing.Point(118, 254);
            this.labelSeedSet.Name = "labelSeedSet";
            this.labelSeedSet.Size = new System.Drawing.Size(47, 13);
            this.labelSeedSet.TabIndex = 7;
            this.labelSeedSet.Text = "seed set";
            // 
            // labelLambda
            // 
            this.labelLambda.AutoSize = true;
            this.labelLambda.Location = new System.Drawing.Point(118, 227);
            this.labelLambda.Name = "labelLambda";
            this.labelLambda.Size = new System.Drawing.Size(41, 13);
            this.labelLambda.TabIndex = 6;
            this.labelLambda.Text = "lambda";
            // 
            // labelSimulationTime
            // 
            this.labelSimulationTime.AutoSize = true;
            this.labelSimulationTime.Location = new System.Drawing.Point(118, 200);
            this.labelSimulationTime.Name = "labelSimulationTime";
            this.labelSimulationTime.Size = new System.Drawing.Size(97, 13);
            this.labelSimulationTime.TabIndex = 5;
            this.labelSimulationTime.Text = "simulation time [ms]";
            // 
            // textBoxSeedSet
            // 
            this.textBoxSeedSet.Location = new System.Drawing.Point(34, 251);
            this.textBoxSeedSet.Name = "textBoxSeedSet";
            this.textBoxSeedSet.Size = new System.Drawing.Size(78, 20);
            this.textBoxSeedSet.TabIndex = 4;
            this.textBoxSeedSet.TextChanged += new System.EventHandler(this.textBoxSeedSet_TextChanged);
            // 
            // textBoxLambda
            // 
            this.textBoxLambda.Location = new System.Drawing.Point(34, 224);
            this.textBoxLambda.Name = "textBoxLambda";
            this.textBoxLambda.Size = new System.Drawing.Size(78, 20);
            this.textBoxLambda.TabIndex = 3;
            this.textBoxLambda.TextChanged += new System.EventHandler(this.textBoxLambda_TextChanged);
            // 
            // textBoxSimulationTime
            // 
            this.textBoxSimulationTime.Location = new System.Drawing.Point(34, 197);
            this.textBoxSimulationTime.Name = "textBoxSimulationTime";
            this.textBoxSimulationTime.Size = new System.Drawing.Size(78, 20);
            this.textBoxSimulationTime.TabIndex = 2;
            this.textBoxSimulationTime.TextChanged += new System.EventHandler(this.textBoxSimulationTime_TextChanged);
            // 
            // checkBoxEnableLogger
            // 
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
            this.chartSteadyState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartSteadyState.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSteadyState.Legends.Add(legend1);
            this.chartSteadyState.Location = new System.Drawing.Point(6, 6);
            this.chartSteadyState.Name = "chartSteadyState";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.Name = "error mean";
            this.chartSteadyState.Series.Add(series1);
            this.chartSteadyState.Size = new System.Drawing.Size(657, 161);
            this.chartSteadyState.TabIndex = 0;
            this.chartSteadyState.Text = "chart1";
            // 
            // multiRun
            // 
            this.multiRun.Controls.Add(this.chartSteadyStateAnalysis);
            this.multiRun.Location = new System.Drawing.Point(4, 22);
            this.multiRun.Name = "multiRun";
            this.multiRun.Padding = new System.Windows.Forms.Padding(3);
            this.multiRun.Size = new System.Drawing.Size(741, 391);
            this.multiRun.TabIndex = 1;
            this.multiRun.Text = "Steady state analysis";
            this.multiRun.UseVisualStyleBackColor = true;
            // 
            // chartSteadyStateAnalysis
            // 
            chartArea2.Name = "ChartArea1";
            this.chartSteadyStateAnalysis.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartSteadyStateAnalysis.Legends.Add(legend2);
            this.chartSteadyStateAnalysis.Location = new System.Drawing.Point(6, 6);
            this.chartSteadyStateAnalysis.Name = "chartSteadyStateAnalysis";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartSteadyStateAnalysis.Series.Add(series2);
            this.chartSteadyStateAnalysis.Size = new System.Drawing.Size(729, 300);
            this.chartSteadyStateAnalysis.TabIndex = 0;
            this.chartSteadyStateAnalysis.Text = "chart1";
            // 
            // backgroundWorkerSimulationLoop
            // 
            this.backgroundWorkerSimulationLoop.WorkerReportsProgress = true;
            this.backgroundWorkerSimulationLoop.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSimulationLoop_DoWork);
            this.backgroundWorkerSimulationLoop.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerSimulationLoop_ProgressChanged);
            this.backgroundWorkerSimulationLoop.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSimulationLoop_RunWorkerCompleted);
            // 
            // SimulationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 441);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimulationView";
            this.Text = "SymulationView";
            this.tabControl.ResumeLayout(false);
            this.singleRun.ResumeLayout(false);
            this.singleRun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyState)).EndInit();
            this.multiRun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSteadyStateAnalysis)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage singleRun;
        private System.Windows.Forms.TabPage multiRun;
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSteadyStateAnalysis;
    }
}