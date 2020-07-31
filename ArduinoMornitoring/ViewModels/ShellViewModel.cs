using System;
using System.IO.Ports;
using System.Windows.Forms;
using ArduinoMornitoring.Views;
using Caliburn.Micro;

namespace ArduinoMornitoring.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        //ShellView shellView = new ShellView();
        SerialPort serial;
        Timer timer = new Timer();
        Random rand = new Random();

        readonly short maxPhotoVal = 1023;
        public short MaxPhotoVal
        {
            get => maxPhotoVal;
        }

        string portname;
        public string PortName
        {
            get => portname;
            set
            {
                portname = value;

                NotifyOfPropertyChange(() => PortName);
            }
        }

        string connecttime;
        public string ConnectTime
        {
            get => connecttime;
            set
            {
                connecttime = value;

                NotifyOfPropertyChange(() => ConnectTime);
            }
        }

        string disconnecttime;
        public string DisconnectTime
        {
            get => disconnecttime;
            set
            {
                disconnecttime = value;

                NotifyOfPropertyChange(() => DisconnectTime);
            }
        }

        bool connectmode;
        public bool ConnectMode
        {
            get => connectmode;
            set
            {
                connectmode = value;

                NotifyOfPropertyChange(() => ConnectMode);
                NotifyOfPropertyChange(() => CanConnectButton);
                NotifyOfPropertyChange(() => CanDisconnectButton);
            }
        }

        bool selectmode;
        public bool SelectMode
        {
            get => selectmode;
            set
            {
                selectmode = value;

                NotifyOfPropertyChange(() => SelectMode);
            }
        }

        public BindableCollection<ComboBoxModel> Ports { get; set; }

        private ComboBoxModel selectedport;
        public ComboBoxModel SelectedPort
        {
            get => selectedport;
            set
            {
                selectedport = value;
                PortName = selectedport.PortName;
                serial = new SerialPort(PortName);

                NotifyOfPropertyChange(() => SelectedPort);
                NotifyOfPropertyChange(() => CanConnectButton);
            }
        }




        public ShellViewModel()
        {
            DisplayName = "Arduino Mornitoring App";
            InitControls();
        }




        public void InitControls()
        {
            ConnectTime = "연결 시간 : ";
            DisconnectTime = "중단 시간 : ";
            SelectMode = true;

            Ports = new BindableCollection<ComboBoxModel>();

            Ports.Add(new ComboBoxModel { PortName = "Select Port"/*, Visiblity = 3, IsSelected = true*/ });
            foreach (var item in SerialPort.GetPortNames())
            {
                Ports.Add(new ComboBoxModel { PortName = item });
            }
        }

        public void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            //DisplayValue(sVal);
        }



        public bool CanPorts
        {
            get => string.IsNullOrEmpty(PortName) || !ConnectMode;
        }

        public bool CanConnectButton
        {
            get => (!string.IsNullOrEmpty(PortName) && !(PortName == "Select Port"))
                && !ConnectMode;
        }

        public bool CanDisconnectButton
        {
            get => !string.IsNullOrEmpty(PortName) && ConnectMode;
        }

        public void ConnectButton()
        {
            serial.Open();

            ConnectTime = $"연결 시간 : {DateTime.Now:yyyy-MM-dd hh:mm:ss}";
            DisconnectTime = "중단 시간 : ";
            ConnectMode = true;
            SelectMode = false;

            NotifyOfPropertyChange(() => CanDisconnectButton);
            NotifyOfPropertyChange(() => CanPorts);
            NotifyOfPropertyChange(() => SelectMode);
        }

        public void DisconnectButton()
        {
            serial.Close();

            DisconnectTime = $"중단 시간 : {DateTime.Now:yyyy-MM-dd hh:mm:ss}";
            ConnectMode = false;
            SelectMode = true;

            NotifyOfPropertyChange(() => CanConnectButton);
            NotifyOfPropertyChange(() => CanPorts);
            NotifyOfPropertyChange(() => SelectMode);
        }
    }
}
