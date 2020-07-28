﻿using Caliburn.Micro;
using System.Dynamic;

namespace SecondCaliburnApp.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        string firstName;
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }

        public ShellViewModel()
        {
            DisplayName = "Second Caliburn App";
            FirstName = "GW";
        }
    }
}
