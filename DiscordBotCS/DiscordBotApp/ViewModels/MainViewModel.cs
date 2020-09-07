using DiscordBotApp.Models;
using DiscordBotApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

namespace DiscordBotApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region
        private string title = "Prism Discord Bot App";
        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }

        private string menu_login;
        public string Menu_Login
        {
            get => menu_login;
            set
            {
                SetProperty(ref menu_login, value);
            }
        }

        private LoginModel login = new LoginModel();
        public LoginModel Login
        {
            get => login;
            set
            {
                SetProperty(ref login, value);
            }
        }
        #endregion
        
        #region Delegate Commands
        public DelegateCommand Login_Click { get; private set; }
        public DelegateCommand Exit_Click { get; private set; }
        public DelegateCommand About_Click { get; private set; }
        #endregion

        public MainViewModel()
        {
            login.access = false;
            menu_login = "Login";

            Login_Click = new DelegateCommand(Access);
            Exit_Click = new DelegateCommand(Exit);
            About_Click = new DelegateCommand(About);
        }

        #region Commands
        private void Access()
        {
            if(login.access == false)
            {
                LoginView login = new LoginView();
                login.ShowDialog();
            }
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void About()
        {
            AboutView about = new AboutView();
            about.ShowDialog();
        }
        #endregion
    }
}
