using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StartCaliburnApp.ViewModels
{
    class ShellViewModel : PropertyChangedBase
    {
        string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanSayHello);
            }
        }

        public bool CanSayHello
        {
            get => string.IsNullOrEmpty(Name);
        }

        public void SayHello()
        {
            MessageBox.Show($"Hello {name}");
        }
    }
}
