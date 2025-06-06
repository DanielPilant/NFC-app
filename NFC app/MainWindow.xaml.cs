using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Threading;

namespace NFC_app
{
    public partial class MainWindow : Window
    {
        private SerialPort serialPort;
        private DispatcherTimer resetTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitSerial();
        }

        private void InitSerial()
        {
            serialPort = new SerialPort("COM11", 115200);
            serialPort.DataReceived += SerialPort_DataReceived;
            try
            {
                serialPort.Open();
                InitTimer();
                StatusText.Text = "Ready. Waiting for card...";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }

        private void InitTimer()
        {
            resetTimer = new DispatcherTimer();
            resetTimer.Interval = TimeSpan.FromSeconds(2);
            resetTimer.Tick += (s, e) =>
            {
                StatusText.Text = "Ready. Waiting for card...";
                GreenIndicator.Visibility = Visibility.Collapsed; 
                resetTimer.Stop();
            };
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine().Trim();

            Dispatcher.Invoke(() =>
            {
                StatusText.Text = $"Card detected: {data}";
                GreenIndicator.Visibility = Visibility.Visible;
                string uid = null;
                int uidIndex = data.IndexOf("UID:");
                if (uidIndex != -1 && data.Length >= uidIndex + 14) 
                {
                    uid = data.Substring(uidIndex + 4).Replace(" ", "").ToUpper();
                }

                if (uid == "F97E2A9C")
                {
                    showTirza();
                    showTirzaTxt();
                }
                else
                {
                    showSimon();
                    showSimonTxt();
                }
                resetTimer.Stop(); 
                resetTimer.Start();
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            if (serialPort?.IsOpen == true)
                serialPort.Close();
            base.OnClosed(e);
        }

        protected void showTirza()
        {
            tirza.Visibility = Visibility.Visible;
        }
        protected void showTirzaTxt()
        {
            tirza_txt.Visibility = Visibility.Visible;
        }

        protected void showSimon()
        {
            simon.Visibility = Visibility.Visible;
        }

        protected void showSimonTxt()
        {
            simon_txt.Visibility = Visibility.Visible;
        }
    }
}
