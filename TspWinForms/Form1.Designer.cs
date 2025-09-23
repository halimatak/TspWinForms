namespace TspWinForms
{
    partial class TspWinForms
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
            this.btnLoadPoints = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.nudGenN = new System.Windows.Forms.NumericUpDown();
            this.nudGenSeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbExactSolver = new System.Windows.Forms.GroupBox();
            this.btnExactCancel = new System.Windows.Forms.Button();
            this.btnExactRun = new System.Windows.Forms.Button();
            this.gbGaSolver = new System.Windows.Forms.GroupBox();
            this.nudMut = new System.Windows.Forms.NumericUpDown();
            this.nudGen = new System.Windows.Forms.NumericUpDown();
            this.nudPop = new System.Windows.Forms.NumericUpDown();
            this.nudGASeed = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbIColon = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGaCancel = new System.Windows.Forms.Button();
            this.btnRunGa = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbResultInformation = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblSolver = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblBest = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblN = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudGenN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGenSeed)).BeginInit();
            this.gbExactSolver.SuspendLayout();
            this.gbGaSolver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGASeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbResultInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadPoints
            // 
            this.btnLoadPoints.Location = new System.Drawing.Point(23, 21);
            this.btnLoadPoints.Name = "btnLoadPoints";
            this.btnLoadPoints.Size = new System.Drawing.Size(105, 23);
            this.btnLoadPoints.TabIndex = 0;
            this.btnLoadPoints.Text = "Load Points...";
            this.btnLoadPoints.UseVisualStyleBackColor = true;
            this.btnLoadPoints.Click += new System.EventHandler(this.btnLoadPoints_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(146, 21);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(105, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // nudGenN
            // 
            this.nudGenN.Location = new System.Drawing.Point(275, 23);
            this.nudGenN.Name = "nudGenN";
            this.nudGenN.Size = new System.Drawing.Size(73, 20);
            this.nudGenN.TabIndex = 1;
            this.nudGenN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudGenSeed
            // 
            this.nudGenSeed.Location = new System.Drawing.Point(365, 23);
            this.nudGenSeed.Name = "nudGenSeed";
            this.nudGenSeed.Size = new System.Drawing.Size(78, 20);
            this.nudGenSeed.TabIndex = 1;
            this.nudGenSeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudGenSeed.ValueChanged += new System.EventHandler(this.nudGenSeed_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Generate N  /  Seed";
            // 
            // gbExactSolver
            // 
            this.gbExactSolver.Controls.Add(this.btnExactCancel);
            this.gbExactSolver.Controls.Add(this.btnExactRun);
            this.gbExactSolver.Location = new System.Drawing.Point(887, 76);
            this.gbExactSolver.Name = "gbExactSolver";
            this.gbExactSolver.Size = new System.Drawing.Size(291, 77);
            this.gbExactSolver.TabIndex = 3;
            this.gbExactSolver.TabStop = false;
            this.gbExactSolver.Text = "Exact Solver";
            // 
            // btnExactCancel
            // 
            this.btnExactCancel.Location = new System.Drawing.Point(180, 30);
            this.btnExactCancel.Name = "btnExactCancel";
            this.btnExactCancel.Size = new System.Drawing.Size(75, 23);
            this.btnExactCancel.TabIndex = 0;
            this.btnExactCancel.Text = "Cancel";
            this.btnExactCancel.UseVisualStyleBackColor = true;
            this.btnExactCancel.Click += new System.EventHandler(this.btnExactCancel_Click);
            // 
            // btnExactRun
            // 
            this.btnExactRun.Location = new System.Drawing.Point(30, 30);
            this.btnExactRun.Name = "btnExactRun";
            this.btnExactRun.Size = new System.Drawing.Size(75, 23);
            this.btnExactRun.TabIndex = 0;
            this.btnExactRun.Text = "Run Exact";
            this.btnExactRun.UseVisualStyleBackColor = true;
            this.btnExactRun.Click += new System.EventHandler(this.btnExactRun_Click);
            // 
            // gbGaSolver
            // 
            this.gbGaSolver.Controls.Add(this.nudMut);
            this.gbGaSolver.Controls.Add(this.nudGen);
            this.gbGaSolver.Controls.Add(this.nudPop);
            this.gbGaSolver.Controls.Add(this.nudGASeed);
            this.gbGaSolver.Controls.Add(this.label8);
            this.gbGaSolver.Controls.Add(this.label6);
            this.gbGaSolver.Controls.Add(this.label4);
            this.gbGaSolver.Controls.Add(this.lbIColon);
            this.gbGaSolver.Controls.Add(this.label7);
            this.gbGaSolver.Controls.Add(this.label5);
            this.gbGaSolver.Controls.Add(this.label3);
            this.gbGaSolver.Controls.Add(this.label2);
            this.gbGaSolver.Controls.Add(this.btnGaCancel);
            this.gbGaSolver.Controls.Add(this.btnRunGa);
            this.gbGaSolver.Location = new System.Drawing.Point(887, 168);
            this.gbGaSolver.Name = "gbGaSolver";
            this.gbGaSolver.Size = new System.Drawing.Size(291, 220);
            this.gbGaSolver.TabIndex = 3;
            this.gbGaSolver.TabStop = false;
            this.gbGaSolver.Text = "GA Solver";
            // 
            // nudMut
            // 
            this.nudMut.Location = new System.Drawing.Point(153, 132);
            this.nudMut.Name = "nudMut";
            this.nudMut.Size = new System.Drawing.Size(106, 20);
            this.nudMut.TabIndex = 2;
            this.nudMut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudGen
            // 
            this.nudGen.Location = new System.Drawing.Point(153, 102);
            this.nudGen.Name = "nudGen";
            this.nudGen.Size = new System.Drawing.Size(106, 20);
            this.nudGen.TabIndex = 2;
            this.nudGen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudPop
            // 
            this.nudPop.Location = new System.Drawing.Point(153, 73);
            this.nudPop.Name = "nudPop";
            this.nudPop.Size = new System.Drawing.Size(106, 20);
            this.nudPop.TabIndex = 2;
            this.nudPop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudGASeed
            // 
            this.nudGASeed.Location = new System.Drawing.Point(153, 45);
            this.nudGASeed.Name = "nudGASeed";
            this.nudGASeed.Size = new System.Drawing.Size(106, 20);
            this.nudGASeed.TabIndex = 2;
            this.nudGASeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(113, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(10, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(113, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = ":";
            // 
            // lbIColon
            // 
            this.lbIColon.AutoSize = true;
            this.lbIColon.Location = new System.Drawing.Point(113, 47);
            this.lbIColon.Name = "lbIColon";
            this.lbIColon.Size = new System.Drawing.Size(10, 13);
            this.lbIColon.TabIndex = 1;
            this.lbIColon.Text = ":";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Mutation Rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Generations";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Population";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Seed";
            // 
            // btnGaCancel
            // 
            this.btnGaCancel.Location = new System.Drawing.Point(180, 174);
            this.btnGaCancel.Name = "btnGaCancel";
            this.btnGaCancel.Size = new System.Drawing.Size(75, 23);
            this.btnGaCancel.TabIndex = 0;
            this.btnGaCancel.Text = "Cancel";
            this.btnGaCancel.UseVisualStyleBackColor = true;
            this.btnGaCancel.Click += new System.EventHandler(this.btnGaCancel_Click);
            // 
            // btnRunGa
            // 
            this.btnRunGa.Location = new System.Drawing.Point(30, 174);
            this.btnRunGa.Name = "btnRunGa";
            this.btnRunGa.Size = new System.Drawing.Size(75, 23);
            this.btnRunGa.TabIndex = 0;
            this.btnRunGa.Text = "Run GA";
            this.btnRunGa.UseVisualStyleBackColor = true;
            this.btnRunGa.Click += new System.EventHandler(this.btnRunGa_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(854, 559);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // gbResultInformation
            // 
            this.gbResultInformation.Controls.Add(this.label23);
            this.gbResultInformation.Controls.Add(this.label9);
            this.gbResultInformation.Controls.Add(this.label10);
            this.gbResultInformation.Controls.Add(this.label11);
            this.gbResultInformation.Controls.Add(this.label12);
            this.gbResultInformation.Controls.Add(this.lblProgress);
            this.gbResultInformation.Controls.Add(this.lblSolver);
            this.gbResultInformation.Controls.Add(this.label21);
            this.gbResultInformation.Controls.Add(this.label13);
            this.gbResultInformation.Controls.Add(this.lblElapsed);
            this.gbResultInformation.Controls.Add(this.label14);
            this.gbResultInformation.Controls.Add(this.lblBest);
            this.gbResultInformation.Controls.Add(this.label15);
            this.gbResultInformation.Controls.Add(this.lblN);
            this.gbResultInformation.Controls.Add(this.label16);
            this.gbResultInformation.Location = new System.Drawing.Point(887, 407);
            this.gbResultInformation.Name = "gbResultInformation";
            this.gbResultInformation.Size = new System.Drawing.Size(291, 228);
            this.gbResultInformation.TabIndex = 3;
            this.gbResultInformation.TabStop = false;
            this.gbResultInformation.Text = "Result Information";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(72, 172);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(10, 13);
            this.label23.TabIndex = 1;
            this.label23.Text = ":";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(72, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(10, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = ":";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(72, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(10, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = ":";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(72, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(10, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = ":";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(72, 40);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(10, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = ":";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(84, 172);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(10, 13);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.Text = "-";
            // 
            // lblSolver
            // 
            this.lblSolver.AutoSize = true;
            this.lblSolver.Location = new System.Drawing.Point(84, 139);
            this.lblSolver.Name = "lblSolver";
            this.lblSolver.Size = new System.Drawing.Size(10, 13);
            this.lblSolver.TabIndex = 1;
            this.lblSolver.Text = "-";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(25, 172);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(48, 13);
            this.label21.TabIndex = 1;
            this.label21.Text = "Progress";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(25, 139);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Solver";
            // 
            // lblElapsed
            // 
            this.lblElapsed.AutoSize = true;
            this.lblElapsed.Location = new System.Drawing.Point(84, 106);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(10, 13);
            this.lblElapsed.TabIndex = 1;
            this.lblElapsed.Text = "-";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Elapsed";
            // 
            // lblBest
            // 
            this.lblBest.AutoSize = true;
            this.lblBest.Location = new System.Drawing.Point(84, 73);
            this.lblBest.Name = "lblBest";
            this.lblBest.Size = new System.Drawing.Size(10, 13);
            this.lblBest.TabIndex = 1;
            this.lblBest.Text = "-";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(25, 73);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Best";
            // 
            // lblN
            // 
            this.lblN.AutoSize = true;
            this.lblN.Location = new System.Drawing.Point(84, 40);
            this.lblN.Name = "lblN";
            this.lblN.Size = new System.Drawing.Size(10, 13);
            this.lblN.TabIndex = 1;
            this.lblN.Text = "-";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(25, 40);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(15, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "N";
            // 
            // TspWinForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 653);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gbResultInformation);
            this.Controls.Add(this.gbGaSolver);
            this.Controls.Add(this.gbExactSolver);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudGenSeed);
            this.Controls.Add(this.nudGenN);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnLoadPoints);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1317, 692);
            this.MinimumSize = new System.Drawing.Size(1182, 692);
            this.Name = "TspWinForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TspWinForms";
            this.Load += new System.EventHandler(this.TspWinForms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudGenN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGenSeed)).EndInit();
            this.gbExactSolver.ResumeLayout(false);
            this.gbGaSolver.ResumeLayout(false);
            this.gbGaSolver.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGASeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbResultInformation.ResumeLayout(false);
            this.gbResultInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadPoints;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.NumericUpDown nudGenN;
        private System.Windows.Forms.NumericUpDown nudGenSeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbExactSolver;
        private System.Windows.Forms.Button btnExactCancel;
        private System.Windows.Forms.Button btnExactRun;
        private System.Windows.Forms.GroupBox gbGaSolver;
        private System.Windows.Forms.Button btnGaCancel;
        private System.Windows.Forms.Button btnRunGa;
        private System.Windows.Forms.NumericUpDown nudGASeed;
        private System.Windows.Forms.Label lbIColon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown nudMut;
        private System.Windows.Forms.NumericUpDown nudGen;
        private System.Windows.Forms.NumericUpDown nudPop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbResultInformation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblSolver;
        private System.Windows.Forms.Label lblElapsed;
        private System.Windows.Forms.Label lblBest;
        private System.Windows.Forms.Label lblN;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label21;
    }
}

