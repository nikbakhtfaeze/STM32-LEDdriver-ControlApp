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
            btnRead = new Button();
            lblVout = new Label();
            lblIout = new Label();
            lblDACValue = new TextBox();
            btnSetCurrent = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btnLedToggle = new Button();
            trackBarDAC = new TrackBar();
            trackBarDAC.Minimum = 0;
            trackBarDAC.Maximum = 100;
            trackBarDAC.TickFrequency = 10; // هر 10 تا یک خط نشون بده (اختیاری)

            ((System.ComponentModel.ISupportInitialize)trackBarDAC).BeginInit();
            SuspendLayout();
            // 
            // cmbPorts
            // 
            cmbPorts.FormattingEnabled = true;
            cmbPorts.Location = new Point(219, 80);
            cmbPorts.Name = "cmbPorts";
            cmbPorts.Size = new Size(182, 33);
            cmbPorts.TabIndex = 0;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(724, 62);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(215, 51);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(724, 131);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(215, 45);
            btnRead.TabIndex = 2;
            btnRead.Text = "Read";
            btnRead.UseVisualStyleBackColor = true;
            // 
            // lblVout
            // 
            lblVout.AutoSize = true;
            lblVout.Location = new Point(219, 151);
            lblVout.Name = "lblVout";
            lblVout.Size = new Size(49, 25);
            lblVout.TabIndex = 3;
            lblVout.Text = "Vout";
            // 
            // lblIout
            // 
            lblIout.AutoSize = true;
            lblIout.Location = new Point(458, 151);
            lblIout.Name = "lblIout";
            lblIout.Size = new Size(44, 25);
            lblIout.TabIndex = 4;
            lblIout.Text = "Iout";
            // 
            // lblDACValue
            // 
            lblDACValue.Location = new Point(742, 342);
            lblDACValue.Name = "lblDACValue";
            lblDACValue.Size = new Size(150, 31);
            lblDACValue.TabIndex = 5;

            // 
            // btnSetCurrent
            // 
            btnSetCurrent.Location = new Point(724, 195);
            btnSetCurrent.Name = "btnSetCurrent";
            btnSetCurrent.Size = new Size(215, 41);
            btnSetCurrent.TabIndex = 6;
            btnSetCurrent.Text = "Set Current";
            btnSetCurrent.UseVisualStyleBackColor = true;
            // 
            // btnLedToggle
            // 
            btnLedToggle.Location = new Point(724, 265);
            btnLedToggle.Name = "btnLedToggle";
            btnLedToggle.Size = new Size(216, 41);
            btnLedToggle.TabIndex = 7;
            btnLedToggle.Text = "LED ";
            btnLedToggle.UseVisualStyleBackColor = true;
            btnLedToggle.Click += btnLedToggle_Click;
            // 
            // trackBarDAC
            // 
            trackBarDAC.Location = new Point(135, 342);
            trackBarDAC.Name = "trackBarDAC";
            trackBarDAC.Size = new Size(524, 69);
            trackBarDAC.TabIndex = 8;
            trackBarDAC.Scroll += trackBarDAC_Scroll;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1059, 510);
            Controls.Add(trackBarDAC);
            Controls.Add(btnLedToggle);
            Controls.Add(btnSetCurrent);
            Controls.Add(lblDACValue);
            Controls.Add(lblIout);
            Controls.Add(lblVout);
            Controls.Add(btnRead);
            Controls.Add(btnConnect);
            Controls.Add(cmbPorts);
            Name = "Form1";
            Text = "LED Driver Controler";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarDAC).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cmbPorts;
        private Button btnConnect;
        private Button btnRead;
        private Label lblVout;
        private Label lblIout;
        private TextBox lblDACValue;
        private Button btnSetCurrent;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btnLedToggle;
        private TrackBar trackBarDAC;
    }
}
