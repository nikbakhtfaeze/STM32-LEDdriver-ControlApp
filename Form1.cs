using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace LEDdriverControlApp
{
    public partial class Form1 : Form
    {
        private readonly SerialPort serialPort1 = new SerialPort();
        private readonly WinFormsTimer pollTimer = new WinFormsTimer();

        private bool ledState = false;

        public Form1()
        {
            InitializeComponent();

            // Attach BEFORE SerialPort.Open() so no incoming line is missed
            // once the port is enumerated/opened.
            serialPort1.DataReceived += SerialPort1_DataReceived;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TryEnableDarkTitleBar(Handle);

            RefreshPorts();
            UpdateConnectionUi(false);

            pollTimer.Interval = 250;
            pollTimer.Tick += PollTimer_Tick;

            // UI-only initialization. Never call CheckedChanged handlers here,
            // because startup defaults must not be written to the board.
            UpdateModeUi(channel2: false);
            UpdateModeUi(channel2: true);
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        // Best-effort: asks the OS to draw this window's title bar in dark
        // mode too, so the native chrome matches the dark client area
        // instead of showing a light bar above a dark app. Cosmetic only --
        // silently does nothing on Windows versions that don't support it.
        private static void TryEnableDarkTitleBar(IntPtr handle)
        {
            try
            {
                int useDark = 1;
                // 20 = DWMWA_USE_IMMERSIVE_DARK_MODE (Win10 20H1+ / Win11).
                // 19 was the same attribute on earlier Win10 dark-mode builds.
                if (DwmSetWindowAttribute(handle, 20, ref useDark, sizeof(int)) != 0)
                    DwmSetWindowAttribute(handle, 19, ref useDark, sizeof(int));
            }
            catch
            {
                // Unsupported OS / API unavailable -- not fatal.
            }
        }

        private void PollTimer_Tick(object? sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
                return;

            try
            {
                serialPort1.WriteLine("GET:MEAS");
            }
            catch
            {
                // The write failed -- most likely the cable/USB device was
                // removed. Reset to a clean disconnected state so the user
                // can reconnect instead of the app silently going dead.
                HandleUnexpectedDisconnect();
            }
        }

        private void RefreshPorts()
        {
            string? previousSelection = cmbPorts.SelectedItem?.ToString();
            string[] ports = SerialPort.GetPortNames();

            cmbPorts.BeginUpdate();
            try
            {
                cmbPorts.Items.Clear();
                cmbPorts.Items.AddRange(ports);

                if (!string.IsNullOrWhiteSpace(previousSelection) && cmbPorts.Items.Contains(previousSelection))
                    cmbPorts.SelectedItem = previousSelection;
                else if (cmbPorts.Items.Count > 0)
                    cmbPorts.SelectedIndex = 0;
            }
            finally
            {
                cmbPorts.EndUpdate();
            }
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private void UpdateConnectionUi(bool connected)
        {
            if (connected)
            {
                lblConnectionStatus.Text = $"{serialPort1.PortName}  •  Connected";
                lblConnectionStatus.ForeColor = BrandSuccess;
                pnlStatusDot.BackColor = BrandSuccess;
                btnConnect.Text = "Disconnect";
                cmbPorts.Enabled = false;
                btnRefreshPorts.Enabled = false;
                btnLedToggle.Enabled = true;
            }
            else
            {
                lblConnectionStatus.Text = "Disconnected";
                lblConnectionStatus.ForeColor = BrandTextSecondary;
                pnlStatusDot.BackColor = BrandTextMuted;
                btnConnect.Text = "Connect";
                cmbPorts.Enabled = true;
                btnRefreshPorts.Enabled = true;
                btnLedToggle.Enabled = false;
                ledState = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                DisconnectDevice();
                return;
            }

            ConnectDevice();
        }

        private void ConnectDevice()
        {
            if (cmbPorts.SelectedItem == null)
            {
                MessageBox.Show("Please select a COM port first.");
                return;
            }

            try
            {
                serialPort1.PortName = cmbPorts.SelectedItem.ToString()!;
                serialPort1.BaudRate = 115200;
                serialPort1.DataBits = 8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.NewLine = "\r\n";
                serialPort1.ReadTimeout = 1000;
                serialPort1.WriteTimeout = 1000;

                serialPort1.Open();
                pollTimer.Start();

                UpdateConnectionUi(true);

                // The PCB is the source of truth for the configured current
                // limits; STATE already carries IMAX1/IMAX2, so ask for it
                // once on connect instead of guessing/defaulting in the UI.
                try { serialPort1.WriteLine("GET:STATE"); } catch { }

                // Fire-and-forget: gives immediate physical confirmation
                // that communication is working without blocking Connect.
                _ = BlinkConnectionIndicatorAsync();
            }
            catch (Exception ex)
            {
                pollTimer.Stop();
                UpdateConnectionUi(false);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DisconnectDevice()
        {
            pollTimer.Stop();

            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
            }
            catch
            {
                // Already gone (e.g. USB was pulled) -- nothing more to do.
            }

            UpdateConnectionUi(false);
        }

        // Called when a write to a port we believed was open fails -- the
        // most common real-world cause is the USB cable being unplugged.
        // Brings the UI back to a clean disconnected state so the user can
        // simply plug back in and press Connect again, instead of the app
        // silently going dead.
        private void HandleUnexpectedDisconnect()
        {
            if (!serialPort1.IsOpen)
                return;

            DisconnectDevice();
        }

        // Two short ON/OFF flashes right after a successful connection, so
        // the user gets immediate physical confirmation that the software
        // actually talked to the driver. Uses Task.Delay (not Thread.Sleep)
        // so the UI thread keeps pumping messages between flashes. This is
        // independent of the LED Toggle button, which must never change
        // its own label -- ledState is intentionally left untouched here.
        private async Task BlinkConnectionIndicatorAsync()
        {
            for (int i = 0; i < 2; i++)
            {
                if (!serialPort1.IsOpen)
                    return;

                try { serialPort1.WriteLine("ON"); }
                catch { return; }

                await Task.Delay(200);

                if (!serialPort1.IsOpen)
                    return;

                try { serialPort1.WriteLine("OFF"); }
                catch { return; }

                await Task.Delay(200);
            }
        }

        private void btnLedToggle_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Please connect first.");
                return;
            }

            try
            {
                if (!ledState)
                {
                    serialPort1.WriteLine("ON");
                    ledState = true;
                }
                else
                {
                    serialPort1.WriteLine("OFF");
                    ledState = false;
                }
            }
            catch { }
        }

        private void trackBarDAC_Scroll(object sender, EventArgs e)
        {
            int percent = trackBarDAC.Value;
            lblDACValue.Text = percent + "%";

            // Physical potentiometer controls the LED in POT mode.
            if (chkPotMode.Checked)
                return;

            // PC controls LED Bar 1 in PC mode.
            if (serialPort1.IsOpen)
            {
                try { serialPort1.WriteLine("DAC1:" + percent); } catch { }
            }
        }

        private void trackBarDAC2_Scroll(object sender, EventArgs e)
        {
            int percent = trackBarDAC2.Value;
            lblDACValue2.Text = percent + "%";

            // Physical potentiometer controls the LED in POT mode.
            if (chkPotMode2.Checked)
                return;

            // PC controls LED Bar 2 in PC mode.
            if (serialPort1.IsOpen)
            {
                try { serialPort1.WriteLine("DAC2:" + percent); } catch { }
            }
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort1.ReadLine().Trim();

                BeginInvoke((MethodInvoker)delegate
                {
                    if (data.StartsWith("STATE:", StringComparison.OrdinalIgnoreCase))
                    {
                        ApplyImaxFromState(data);
                        return;
                    }

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

                        if (handled)
                            return;
                    }

                    // Compatibility with old one-channel replies.
                    if (data.StartsWith("IOUT:", StringComparison.OrdinalIgnoreCase))
                        lblIout.Text = data.Substring("IOUT:".Length).Trim();
                    else if (data.StartsWith("VOUT:", StringComparison.OrdinalIgnoreCase))
                        lblVout.Text = data.Substring("VOUT:".Length).Trim();
                });
            }
            catch
            {
                // Serial disconnect/partial line: ignore; no state is written.
            }
        }

        // The firmware's existing STATE response already carries the
        // currently configured IMAX1/IMAX2 (it's the same response this app
        // used to require for a full startup handshake). Reusing it here --
        // once on connect and once after each Apply Limit -- is the only
        // source of truth for what the PCB actually has configured; the UI
        // must never fabricate a value. Mode/percentage fields in the same
        // response are intentionally ignored so this can't reintroduce the
        // old startup-blocking synchronization behavior.
        private void ApplyImaxFromState(string data)
        {
            string? imax1Text = TryExtractBetween(data, "IMAX1=", "mA");
            string? imax2Text = TryExtractBetween(data, "IMAX2=", "mA");

            if (!string.IsNullOrWhiteSpace(imax1Text) && int.TryParse(imax1Text, out int imax1))
                txtMaxmA1.Text = imax1.ToString();

            if (!string.IsNullOrWhiteSpace(imax2Text) && int.TryParse(imax2Text, out int imax2))
                txtMaxmA2.Text = imax2.ToString();
        }

        private void UpdateModeUi(bool channel2)
        {
            if (!channel2)
            {
                bool potMode = chkPotMode.Checked;
                trackBarDAC.Enabled = !potMode;
                lblDACValue.Enabled = !potMode;
                SetSegmentActive(btnComputer1, !potMode);
                SetSegmentActive(btnPot1, potMode);
                lblControlDesc1.Text = potMode
                    ? "Controlled by potentiometer connected to POT1"
                    : "Controlled from this application";
            }
            else
            {
                bool potMode = chkPotMode2.Checked;
                trackBarDAC2.Enabled = !potMode;
                lblDACValue2.Enabled = !potMode;
                SetSegmentActive(btnComputer2, !potMode);
                SetSegmentActive(btnPot2, potMode);
                lblControlDesc2.Text = potMode
                    ? "Controlled by potentiometer connected to POT2"
                    : "Controlled from this application";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateModeUi(channel2: false);

            if (!serialPort1.IsOpen)
                return;

            try
            {
                serialPort1.WriteLine(chkPotMode.Checked ? "MODE1:POT" : "MODE1:PC");
            }
            catch { }
        }

        private void chkPotMode2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateModeUi(channel2: true);

            if (!serialPort1.IsOpen)
                return;

            try
            {
                serialPort1.WriteLine(chkPotMode2.Checked ? "MODE2:POT" : "MODE2:PC");
            }
            catch { }
        }

        private void btnSetMax1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Not connected to device.");
                return;
            }

            if (!int.TryParse(txtMaxmA1.Text.Trim(), out int imax_mA))
            {
                MessageBox.Show("Please enter a valid integer number (mA).");
                return;
            }

            if (imax_mA < 0 || imax_mA > 800)
            {
                MessageBox.Show("Max current must be between 0 and 800 mA.");
                return;
            }

            try
            {
                serialPort1.WriteLine($"IMAX1:{imax_mA}");
                serialPort1.WriteLine("GET:STATE"); // confirm what the PCB actually stored
            }
            catch { }
        }

        private void btnSetMax2_Click_1(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Not connected to device.");
                return;
            }

            if (!int.TryParse(txtMaxmA2.Text.Trim(), out int imax_mA))
            {
                MessageBox.Show("Please enter a valid integer number (mA).");
                return;
            }

            if (imax_mA < 0 || imax_mA > 800)
            {
                MessageBox.Show("Max current must be between 0 and 800 mA.");
                return;
            }

            try
            {
                serialPort1.WriteLine($"IMAX2:{imax_mA}");
                serialPort1.WriteLine("GET:STATE"); // confirm what the PCB actually stored
            }
            catch { }
        }

        private void UpdateChannelLabels(string segment, bool isChannel2)
        {
            string? ioutText = TryExtractBetween(segment, "IOUT=", "mA");
            string? voutText = TryExtractBetween(segment, "VOUT=", "mV");

            if (!string.IsNullOrWhiteSpace(ioutText))
            {
                if (isChannel2)
                    lblIout2.Text = $"{ioutText} mA";
                else
                    lblIout.Text = $"{ioutText} mA";
            }

            if (!string.IsNullOrWhiteSpace(voutText) && int.TryParse(voutText, out int mvVal))
            {
                string formatted = $"{mvVal / 1000.0:0.00} V";
                if (isChannel2)
                    lblVout2.Text = formatted;
                else
                    lblVout.Text = formatted;
            }
            else if (!string.IsNullOrWhiteSpace(voutText))
            {
                if (isChannel2)
                    lblVout2.Text = "— V";
                else
                    lblVout.Text = "— V";
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

        private void Form1_FormClosed(object? sender, FormClosedEventArgs e)
        {
            pollTimer.Stop();
            if (serialPort1.IsOpen)
            {
                try { serialPort1.Close(); } catch { }
            }
            serialPort1.Dispose();
        }
    }
}
