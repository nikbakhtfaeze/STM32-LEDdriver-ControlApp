using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using WinFormsTimer = System.Windows.Forms.Timer;




namespace LEDdriverControlApp
{
    public partial class Form1 : Form
    {
        SerialPort serialPort1 = new SerialPort();
        bool ledState = false; // false = OFF, true = ON
        private readonly WinFormsTimer pollTimer = new WinFormsTimer();


        public Form1()
        {
            InitializeComponent();
        }

        // Fix for CS1513 and CS8622: 
        // 1. Move PollTimer_Tick method outside of Form1_Load to correct the misplaced method definition (fixes CS1513).
        // 2. Add nullable annotations to match EventHandler delegate (fixes CS8622).

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshPorts();
            pollTimer.Interval = 250;
            pollTimer.Tick += PollTimer_Tick;
            // Set UI state based on checkbox at startup
            checkBox1_CheckedChanged(chkPotMode, EventArgs.Empty);
            chkPotMode2_CheckedChanged(chkPotMode2, EventArgs.Empty);


            // handler
        } // <-- This closes Form1_Load properly

        // handler
        private void PollTimer_Tick(object? sender, EventArgs e)
        {
            if (!serialPort1.IsOpen) return;
            try { serialPort1.WriteLine("GET:MEAS"); } catch { }
        }

        private void RefreshPorts()
        {
            cmbPorts.Items.Clear();
            cmbPorts.Items.AddRange(SerialPort.GetPortNames());
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cmbPorts.SelectedItem == null)
            {
                MessageBox.Show("Please select a COM port first.");
                return;
            }

            try
            {
                serialPort1.PortName = cmbPorts.SelectedItem.ToString();
                serialPort1.BaudRate = 115200;
                serialPort1.DataBits = 8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.NewLine = "\r\n";

                serialPort1.Open();


                serialPort1.DataReceived += SerialPort1_DataReceived;

                // start polling the MCU for Vout/Imon
                pollTimer.Start();   // <-- ADD THIS


                MessageBox.Show("Connected to " + serialPort1.PortName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnLedToggle_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Please connect first.");
                return;
            }

            if (ledState == false)
            {
                serialPort1.WriteLine("ON");
                btnLedToggle.Text = "Turn LED OFF";
                ledState = true;
            }
            else
            {
                serialPort1.WriteLine("OFF");
                btnLedToggle.Text = "Turn LED ON";
                ledState = false;
            }
        }

        private void trackBarDAC_Scroll(object sender, EventArgs e)
        {
            int percent = trackBarDAC.Value;
            lblDACValue.Text = percent.ToString() + " %";

            // If POT mode is active, PC must NOT control DAC
            if (chkPotMode.Checked) return;

            if (serialPort1.IsOpen)
            {
                serialPort1.WriteLine("DAC1:" + percent.ToString());
            }
        }

        private void lblIout_Click(object sender, EventArgs e)
        {

        }

        private void lblDACValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadLine();

                this.Invoke((MethodInvoker)delegate
                {
                    // New combined reply: MEAS:IOUT=123mA,VOUT=12345mV
                    if (data.StartsWith("MEAS:", StringComparison.OrdinalIgnoreCase))
                    {
                        int ch1Start = data.IndexOf("CH1:", StringComparison.OrdinalIgnoreCase);
                        int ch2Start = data.IndexOf("CH2:", StringComparison.OrdinalIgnoreCase);

                        bool handled = false;

                        if (ch1Start >= 0)
                        {
                            int ch1End = ch2Start >= 0 ? ch2Start : data.Length;
                            string ch1 = data.Substring(ch1Start, ch1End - ch1Start);
                            UpdateChannelLabels(ch1, isChannel2: false);
                            handled = true;
                        }

                        if (ch2Start >= 0)
                        {
                            string ch2 = data.Substring(ch2Start);
                            UpdateChannelLabels(ch2, isChannel2: true);
                            handled = true;
                        }

                        if (handled) return;
                    }

                    // (Optional) old formats support
                    if (data.StartsWith("IOUT:", StringComparison.OrdinalIgnoreCase))
                        lblIout.Text = data.Trim();
                    if (data.StartsWith("VOUT:", StringComparison.OrdinalIgnoreCase))
                        lblVout.Text = data.Trim();
                });
            }
            catch { }
        }

        private void lblIout_Click_1(object sender, EventArgs e)
        {

        }

        private void lblVout_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool potMode = chkPotMode.Checked;

            // UI: when POT controls, disable PC slider + textbox
            trackBarDAC.Enabled = !potMode;
            lblDACValue.Enabled = !potMode;

            chkPotMode.Text = potMode ? "Control: POT" : "Control: PC";

            // Send mode to MCU (only if connected)
            if (!serialPort1.IsOpen) return;

            try
            {
                serialPort1.WriteLine(potMode ? "MODE1:POT" : "MODE1:PC");
            }
            catch { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSetMax1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Not connected to device.");
                return;
            }

            var s = txtMaxmA1.Text.Trim();

            if (!int.TryParse(s, out int imax_mA))
            {
                MessageBox.Show("Please enter a valid integer number (mA).");
                return;
            }

            if (imax_mA < 0 || imax_mA > 1000)
            {
                MessageBox.Show("Max current must be between 0 and 1000 mA.");
                return;
            }

            serialPort1.WriteLine($"IMAX1:{imax_mA}");
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSetMax2_Click_1(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Not connected to device.");
                return;
            }

            var s = txtMaxmA2.Text.Trim();

            if (!int.TryParse(s, out int imax_mA))
            {
                MessageBox.Show("Please enter a valid integer number (mA).");
                return;
            }

            if (imax_mA < 0 || imax_mA > 1000)
            {
                MessageBox.Show("Max current must be between 0 and 1000 mA.");
                return;
            }

            serialPort1.WriteLine($"IMAX2:{imax_mA}");
        }

        private void chkPotMode2_CheckedChanged(object sender, EventArgs e)
        {

            bool potMode = chkPotMode2.Checked;

            trackBarDAC2.Enabled = !potMode;
            lblDACValue2.Enabled = !potMode;

            chkPotMode2.Text = potMode ? "Control: POT" : "Control: PC";

            if (!serialPort1.IsOpen) return;

            try
            {
                serialPort1.WriteLine(potMode ? "MODE2:POT" : "MODE2:PC");
            }
            catch { }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void trackBarDAC2_Scroll(object sender, EventArgs e)
        {
            int percent = trackBarDAC2.Value;
            lblDACValue2.Text = percent.ToString() + " %";

            if (chkPotMode2.Checked) return;

            if (serialPort1.IsOpen)
            {
                serialPort1.WriteLine("DAC2:" + percent.ToString());
            }
        }

        private void txtMaxmA2_TextChanged(object sender, EventArgs e)
        {

        }
        private void UpdateChannelLabels(string segment, bool isChannel2)
        {
            string? ioutText = TryExtractBetween(segment, "IOUT=", "mA");
            string? voutText = TryExtractBetween(segment, "VOUT=", "mV");

            if (!string.IsNullOrWhiteSpace(ioutText))
            {
                if (isChannel2)
                    lblIout2.Text = $"Iout: {ioutText} mA";
                else
                    lblIout.Text = $"Iout: {ioutText} mA";
            }

            if (!string.IsNullOrWhiteSpace(voutText) && int.TryParse(voutText, out int mvVal))
            {
                string formatted = $"Vout: {mvVal / 1000.0:0.00} V";

                if (isChannel2)
                    lblVout2.Text = formatted;
                else
                    lblVout.Text = formatted;
            }
            else if (!string.IsNullOrWhiteSpace(voutText))
            {
                if (isChannel2)
                    lblVout2.Text = "Vout: ---";
                else
                    lblVout.Text = "Vout: ---";
            }
        }

        private static string? TryExtractBetween(string text, string startToken, string endToken)
        {
            int start = text.IndexOf(startToken, StringComparison.OrdinalIgnoreCase);
            if (start < 0) return null;
            start += startToken.Length;

            int end = text.IndexOf(endToken, start, StringComparison.OrdinalIgnoreCase);
            if (end < 0) return null;

            return text.Substring(start, end - start).Trim();
        }
    }
}

