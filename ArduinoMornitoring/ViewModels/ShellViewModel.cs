using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ArduinoMornitoring.Models;
using Caliburn.Micro;

namespace ArduinoMornitoring.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        SerialPort serial;
        Timer timer = new Timer();
        Random rand = new Random();
        List<SensorDataModel> photoDatas = new List<SensorDataModel>();
        private short xCount = 200;

        readonly short maxPhotoVal = 1023;
        public short MaxPhotoVal
        {
            get => maxPhotoVal;
        }

        ushort pgbphotoregistor;
        public ushort PgbPhotoRegistor
        {
            get => pgbphotoregistor;
            set
            {
                pgbphotoregistor = value;

                NotifyOfPropertyChange(() => PgbPhotoRegistor);
            }
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

        string portvalue;
        public string PortValue
        {
            get => portvalue;
            set
            {
                portvalue = value;

                NotifyOfPropertyChange(() => PortValue);
            }
        }

        bool issimulation;
        public bool IsSimulation
        {
            get => issimulation;
            set
            {
                issimulation = value;

                NotifyOfPropertyChange(() => IsSimulation);
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

        public BindableCollection<ComboBoxItem> Ports { get; set; }

        private ComboBoxItem selectedport;
        public ComboBoxItem SelectedPort
        {
            get => selectedport;
            set
            {
                selectedport = value;
                PortName = selectedport.Content.ToString();
                serial = new SerialPort(PortName);
                serial.DataReceived += Serial_DataReceived;

                NotifyOfPropertyChange(() => SelectedPort);
                NotifyOfPropertyChange(() => CanConnectButton);
            }
        }

        string txtsensorcount;
        public string TxtSensorCount
        {
            get => txtsensorcount;
            set
            {
                txtsensorcount = value;

                NotifyOfPropertyChange(() => TxtSensorCount);
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
            IsSimulation = false;

            Ports = new BindableCollection<ComboBoxItem>();

            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem.Content = "Select Port";
            comboBoxItem.Visibility = Visibility.Collapsed;
            comboBoxItem.IsSelected = true;
            Ports.Add(comboBoxItem);

            foreach (var item in SerialPort.GetPortNames())
            {
                Ports.Add(new ComboBoxItem { Content = item });
            }
        }




        public void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            DisplayValue(sVal);
        }

        public void DisplayValue(string sVal)
        {
            System.Windows.Forms.RichTextBox RtbLog = new System.Windows.Forms.RichTextBox();
            try
            {
                ushort v = ushort.Parse(sVal);
                if (v < 0 || v > maxPhotoVal)
                    return;

                SensorDataModel data = new SensorDataModel();
                photoDatas.Add(data);
                //InsertDataToDB(data);

                TxtSensorCount = photoDatas.Count.ToString();
                PgbPhotoRegistor = v;

                string item = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}\t{v}";
                RtbLog.AppendText($"{item}\n");
                RtbLog.ScrollToCaret();

                //ChtSensorValues.Series[0].Points.Add(v);

                //ChtSensorValues.ChartAreas[0].AxisX.Minimum = 0;
                //ChtSensorValues.ChartAreas[0].AxisX.Maximum =
                //    (photoDatas.Count >= xCount) ? photoDatas.Count : xCount;

                //if (photoDatas.Count > xCount)
                //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(
                //        photoDatas.Count - xCount, photoDatas.Count);
                //else
                //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(0, xCount);

                if (IsSimulation == false)
                    PortValue = $"{serial.PortName}\n{sVal}";
                else
                    PortValue = $"{sVal}";

                NotifyOfPropertyChange(() => TxtSensorCount);
                NotifyOfPropertyChange(() => PortValue);
            }
            catch (Exception ex)
            {
                RtbLog.AppendText($"Error : {ex.Message}\n");
                RtbLog.ScrollToCaret();
            }
        }

        public void SimulStart(object sender, EventArgs e)
        {
            IsSimulation = true;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // serial통신 끊기
            if(ConnectMode == true)
            {
                DisconnectButton();
            }
            PortName = "Simulation";

            NotifyOfPropertyChange(() => IsSimulation);
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            ushort value = (ushort)rand.Next(1, 1024);
            DisplayValue(value.ToString());
        }

        public void SimulStop(object sender, EventArgs e)
        {
            timer.Stop();
            IsSimulation = false;

            NotifyOfPropertyChange(() => IsSimulation);
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
