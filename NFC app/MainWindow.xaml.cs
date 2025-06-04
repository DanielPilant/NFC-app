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
            serialPort = new SerialPort("COM11", 115200); // שנה ל־COM הנכון
            serialPort.DataReceived += SerialPort_DataReceived;
            try
            {
                serialPort.Open();
                // Set initial status text with timer 
                InitTimer();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }

        // set timer to show "Ready. Waiting for card..." after 2 seconds
        private void InitTimer()
        {
            resetTimer = new DispatcherTimer();
            resetTimer.Interval = TimeSpan.FromSeconds(2);
            resetTimer.Tick += (s, e) =>
            {
                
                StatusText.Text = "Ready. Waiting for card...";
                resetTimer.Stop(); // Stop the timer after resetting the status

            };
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort.ReadLine().Trim(); 

            Dispatcher.Invoke(() =>
            {
                StatusText.Text = $"Card detected: {data}";
                resetTimer.Stop();
                resetTimer.Start(); // Restart the timer to reset status after 3 seconds
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            if (serialPort?.IsOpen == true)
                serialPort.Close();
            base.OnClosed(e);
        }
    }
}
