using System.IO.Ports;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace NFC_app
{
    /// <summary>  
    /// Interaction logic for App.xaml  
    /// </summary>  
    public partial class App : Application
    {
        private SerialPort serialPort;

        public App()
        {
            InitializeComponent();
        }

    }

}
