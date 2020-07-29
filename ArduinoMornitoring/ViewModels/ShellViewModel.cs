using Caliburn.Micro;
using ArduinoMornitoring.Models;
using System.IO.Ports;

namespace ArduinoMornitoring.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            DisplayName = "Arduino Mornitoring App";

            Ports = new BindableCollection<ShellModel>();

            foreach (var item in SerialPort.GetPortNames())
            {
                Ports.Add(new ShellModel { PortName = item });
            }
            //InitControls(Ports);
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

        public BindableCollection<ShellModel> Ports { get; set; }
        private ShellModel selectedport;
        public ShellModel SelectedPort
        {
            get => selectedport;
            set
            {
                selectedport = value;
                PortName = selectedport.PortName;
                NotifyOfPropertyChange(() => SelectedPort);
            }
        }

        private void InitControls(BindableCollection<ShellModel> Ports)
        {
            foreach (var item in SerialPort.GetPortNames())
            {
                Ports.Add(new ShellModel { PortName = item });
            }
        }
    }
}
