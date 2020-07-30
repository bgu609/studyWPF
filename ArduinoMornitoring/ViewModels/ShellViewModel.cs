using System;
using System.IO.Ports;
using System.Windows.Forms;
using Caliburn.Micro;
using ArduinoMornitoring.Models;


namespace ArduinoMornitoring.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
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

        public BindableCollection<ShellModel> Ports { get; set; }

        private ShellModel selectedport;
        public ShellModel SelectedPort
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

            Ports = new BindableCollection<ShellModel>();
            InitControls(Ports);
        }



        public void InitControls(BindableCollection<ShellModel> Ports)
        {
            ConnectTime = "연결 시간 : ";
            DisconnectTime = "중단 시간 : ";

            foreach (var item in SerialPort.GetPortNames())
            {
                Ports.Add(new ShellModel { PortName = item });
            }
        }

        public void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            //this.BeginInvoke(new Action(delegate { DisplayValue(sVal); }));
        }



        public bool CanConnectButton
        {
            get => !string.IsNullOrEmpty(PortName) && !ConnectMode;
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

            NotifyOfPropertyChange(() => CanDisconnectButton);
        }

        public void DisconnectButton()
        {
            serial.Close();

            DisconnectTime = $"중단 시간 : {DateTime.Now:yyyy-MM-dd hh:mm:ss}";
            ConnectMode = false;

            NotifyOfPropertyChange(() => CanConnectButton);
        }
    }
}
