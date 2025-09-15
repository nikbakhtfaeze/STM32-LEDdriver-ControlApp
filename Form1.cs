using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml;

namespace LEDdriverControlApp
{
    public partial class Form1 : Form
    {
        SerialPort serialPort1 = new SerialPort();
        bool ledState = false; // false = OFF, true = ON

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshPorts();

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

                serialPort1.Open();
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
            int percent = trackBarDAC.Value; // مقدار بین 0 تا 100
            lblDACValue.Text = percent.ToString() + " %";

            if (serialPort1.IsOpen)
            {
                // دستور رو به صورت "DAC:50" می‌فرسته
                serialPort1.WriteLine("DAC:" + percent.ToString());
            }
        }

    }
}

