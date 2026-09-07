namespace LEDdriverControlApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbPorts = new ComboBox();
            btnConnect = new Button();
            lblVout = new Label();
            lblDACValue = new TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btnLedToggle = new Button();
            trackBarDAC = new TrackBar();
            lblIout = new Label();
            chkPotMode = new CheckBox();
            btnSetMax1 = new Button();
            txtMaxmA1 = new TextBox();
            mA1 = new Label();
            btnSetMax2 = new Button();
            txtMaxmA2 = new TextBox();
            label1 = new Label();
            trackBarDAC2 = new TrackBar();
            lblDACValue2 = new TextBox();
            chkPotMode2 = new CheckBox();
            output1 = new Label();
            output2 = new Label();
            line = new Label();
            lblVout2 = new Label();
            lblIout2 = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBarDAC).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarDAC2).BeginInit();
            SuspendLayout();
            // 
            // cmbPorts
            // 
            cmbPorts.FormattingEnabled = true;
            cmbPorts.Location = new Point(55, 44);
            cmbPorts.Name = "cmbPorts";
            cmbPorts.Size = new Size(182, 33);
            cmbPorts.TabIndex = 0;
            // 
            // btnConnect
            // 
            btnConnect.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConnect.Location = new Point(905, 34);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(215, 51);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // lblVout
            // 
            lblVout.AutoSize = true;
            lblVout.Font = new Font("Segoe UI", 15F);
            lblVout.Location = new Point(55, 189);
            lblVout.Name = "lblVout";
            lblVout.Size = new Size(87, 41);
            lblVout.TabIndex = 3;
            lblVout.Text = "Vout:";
            lblVout.Click += lblVout_Click;
            // 
            // lblDACValue
            // 
            lblDACValue.Location = new Point(55, 257);
            lblDACValue.Name = "lblDACValue";
            lblDACValue.Size = new Size(80, 31);
            lblDACValue.TabIndex = 5;
            lblDACValue.TextChanged += lblDACValue_TextChanged;
            // 
            // btnLedToggle
            // 
            btnLedToggle.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLedToggle.Location = new Point(667, 36);
            btnLedToggle.Name = "btnLedToggle";
            btnLedToggle.Size = new Size(216, 46);
            btnLedToggle.TabIndex = 7;
            btnLedToggle.Text = "LED ";
            btnLedToggle.UseVisualStyleBackColor = true;
            btnLedToggle.Click += btnLedToggle_Click;
            // 
            // trackBarDAC
            // 
            trackBarDAC.Location = new Point(42, 322);
            trackBarDAC.Maximum = 100;
            trackBarDAC.Name = "trackBarDAC";
            trackBarDAC.Size = new Size(524, 69);
            trackBarDAC.TabIndex = 8;
            trackBarDAC.TickFrequency = 10;
            trackBarDAC.Scroll += trackBarDAC_Scroll;
            // 
            // lblIout
            // 
            lblIout.AutoSize = true;
            lblIout.Font = new Font("Segoe UI", 15F);
            lblIout.Location = new Point(264, 189);
            lblIout.Name = "lblIout";
            lblIout.Size = new Size(86, 41);
            lblIout.TabIndex = 10;
            lblIout.Text = "Iout: ";
            lblIout.Click += lblIout_Click_1;
            // 
            // chkPotMode
            // 
            chkPotMode.AutoSize = true;
            chkPotMode.Checked = true;
            chkPotMode.CheckState = CheckState.Checked;
            chkPotMode.FlatStyle = FlatStyle.System;
            chkPotMode.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPotMode.Location = new Point(648, 299);
            chkPotMode.Name = "chkPotMode";
            chkPotMode.Size = new Size(211, 43);
            chkPotMode.TabIndex = 11;
            chkPotMode.Text = "Control: POT";
            chkPotMode.UseVisualStyleBackColor = true;
            chkPotMode.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // btnSetMax1
            // 
            btnSetMax1.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetMax1.Location = new Point(905, 230);
            btnSetMax1.Name = "btnSetMax1";
            btnSetMax1.Size = new Size(215, 51);
            btnSetMax1.TabIndex = 12;
            btnSetMax1.Text = "SET MAX Current";
            btnSetMax1.UseVisualStyleBackColor = true;
            btnSetMax1.Click += btnSetMax1_Click;
            // 
            // txtMaxmA1
            // 
            txtMaxmA1.Location = new Point(710, 240);
            txtMaxmA1.Name = "txtMaxmA1";
            txtMaxmA1.Size = new Size(84, 31);
            txtMaxmA1.TabIndex = 13;
            txtMaxmA1.TextChanged += textBox1_TextChanged;
            // 
            // mA1
            // 
            mA1.AutoSize = true;
            mA1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            mA1.Location = new Point(800, 233);
            mA1.Name = "mA1";
            mA1.Size = new Size(59, 38);
            mA1.TabIndex = 14;
            mA1.Text = "mA";
            mA1.Click += label1_Click;
            // 
            // btnSetMax2
            // 
            btnSetMax2.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetMax2.Location = new Point(905, 569);
            btnSetMax2.Name = "btnSetMax2";
            btnSetMax2.Size = new Size(207, 46);
            btnSetMax2.TabIndex = 15;
            btnSetMax2.Text = "SET MAX Current";
            btnSetMax2.UseVisualStyleBackColor = true;
            btnSetMax2.Click += btnSetMax2_Click_1;
            // 
            // txtMaxmA2
            // 
            txtMaxmA2.Location = new Point(710, 577);
            txtMaxmA2.Name = "txtMaxmA2";
            txtMaxmA2.Size = new Size(84, 31);
            txtMaxmA2.TabIndex = 16;
            txtMaxmA2.TextChanged += txtMaxmA2_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(800, 570);
            label1.Name = "label1";
            label1.Size = new Size(59, 38);
            label1.TabIndex = 17;
            label1.Text = "mA";
            label1.Click += label1_Click_1;
            // 
            // trackBarDAC2
            // 
            trackBarDAC2.Location = new Point(42, 689);
            trackBarDAC2.Name = "trackBarDAC2";
            trackBarDAC2.Size = new Size(524, 69);
            trackBarDAC2.TabIndex = 18;
            trackBarDAC2.Scroll += trackBarDAC2_Scroll;
            trackBarDAC2.Maximum = 100;
            trackBarDAC2.TickFrequency = 10;
            // 
            // lblDACValue2
            // 
            lblDACValue2.Location = new Point(55, 622);
            lblDACValue2.Name = "lblDACValue2";
            lblDACValue2.Size = new Size(80, 31);
            lblDACValue2.TabIndex = 19;
            // 
            // chkPotMode2
            // 
            chkPotMode2.AutoSize = true;
            chkPotMode2.Checked = true;
            chkPotMode2.CheckState = CheckState.Checked;
            chkPotMode2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkPotMode2.Location = new Point(648, 669);
            chkPotMode2.Name = "chkPotMode2";
            chkPotMode2.Size = new Size(199, 42);
            chkPotMode2.TabIndex = 20;
            chkPotMode2.Text = "Control: POT";
            chkPotMode2.UseVisualStyleBackColor = true;
            chkPotMode2.CheckedChanged += chkPotMode2_CheckedChanged;
            // 
            // output1
            // 
            output1.AutoSize = true;
            output1.Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            output1.Location = new Point(42, 102);
            output1.Name = "output1";
            output1.Size = new Size(239, 48);
            output1.TabIndex = 21;
            output1.Text = "OUTPUT 1:";
            output1.Click += label2_Click;
            // 
            // output2
            // 
            output2.AutoSize = true;
            output2.Font = new Font("Tahoma", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            output2.Location = new Point(42, 416);
            output2.Name = "output2";
            output2.Size = new Size(239, 48);
            output2.TabIndex = 22;
            output2.Text = "OUTPUT 2:";
            output2.Click += label3_Click;
            // 
            // line
            // 
            line.AutoSize = true;
            line.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            line.ForeColor = SystemColors.ActiveBorder;
            line.Location = new Point(-4, 379);
            line.Name = "line";
            line.Size = new Size(1174, 29);
            line.TabIndex = 23;
            line.Text = "---------------------------------------------------------------------------------------------------------------------------------";
            // 
            // lblVout2
            // 
            lblVout2.AutoSize = true;
            lblVout2.Font = new Font("Segoe UI", 15F);
            lblVout2.Location = new Point(55, 529);
            lblVout2.Name = "lblVout2";
            lblVout2.Size = new Size(87, 41);
            lblVout2.TabIndex = 25;
            lblVout2.Text = "Vout:";
            // 
            // lblIout2
            // 
            lblIout2.AutoSize = true;
            lblIout2.Font = new Font("Segoe UI", 15F);
            lblIout2.Location = new Point(264, 529);
            lblIout2.Name = "lblIout2";
            lblIout2.Size = new Size(86, 41);
            lblIout2.TabIndex = 26;
            lblIout2.Text = "Iout: ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1159, 793);
            Controls.Add(lblIout2);
            Controls.Add(lblVout2);
            Controls.Add(line);
            Controls.Add(output2);
            Controls.Add(output1);
            Controls.Add(chkPotMode2);
            Controls.Add(lblDACValue2);
            Controls.Add(trackBarDAC2);
            Controls.Add(label1);
            Controls.Add(txtMaxmA2);
            Controls.Add(btnSetMax2);
            Controls.Add(mA1);
            Controls.Add(txtMaxmA1);
            Controls.Add(btnSetMax1);
            Controls.Add(chkPotMode);
            Controls.Add(lblIout);
            Controls.Add(trackBarDAC);
            Controls.Add(btnLedToggle);
            Controls.Add(lblDACValue);
            Controls.Add(lblVout);
            Controls.Add(btnConnect);
            Controls.Add(cmbPorts);
            Name = "Form1";
            Text = "LED Driver Controler";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarDAC).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarDAC2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbPorts;
        private Button btnConnect;
        private Label lblVout;
        private TextBox lblDACValue;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btnLedToggle;
        private TrackBar trackBarDAC;
        private Label lblIout;
        private CheckBox chkPotMode;
        private Button btnSetMax1;
        private TextBox txtMaxmA1;
        private Label mA1;
        private Button btnSetMax2;
        private TextBox txtMaxmA2;
        private Label label1;
        private TrackBar trackBarDAC2;
        private TextBox lblDACValue2;
        private CheckBox chkPotMode2;
        private Label output1;
        private Label output2;
        private Label line;
        private Label lblVout2;
        private Label lblIout2;
    }
}
