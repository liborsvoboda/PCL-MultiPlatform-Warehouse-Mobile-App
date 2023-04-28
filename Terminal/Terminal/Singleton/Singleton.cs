using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Terminal.Interfaces;
using Terminal.Languages;
using Xamarin.Forms;

namespace Terminal.Singleton
{

    public class AppStatus
    {
        //public Color SapConnectionTypeColor { get; set; } = Color.FromHex("#ffffff"); //Color.FromHex("#d7dadd");
        public bool StatusOnline { get; set; } = false;
        public bool UserLoggedIn { get; set; } = false;
       // public string Platform { get; set; } = null;
        public static string BasePath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);//ApplicationData LocalApplicationData a personal
        public static string ExternalPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources);
        public static string DatabaseFilename { get; set; } = "Terminal.db3";
    }

    public class DefaultValues
    {
        //default values 
        public const string ldapServerIpDefault = "10.168.70.188";
        public const string ldapPortDefault = "389";
        public const string ldapDNDefault = "ou=contractors_cz,ou=people,dc=st,dc=sk";
        public const string roleDNDefault = "ou=Roles,ou=OMT,ou=Apps,dc=st,dc=sk";
        public const string refreshIntervalDefault = "3";
        public const string reportPrinterPortDefault = "9100";
        public const string labelPrinterPortDefault = "9100";

        public const int gotoLimit = 20;
    }

    public class GlobalResources : INotifyPropertyChanged
    {
        // Singleton
        public static GlobalResources Current = new GlobalResources();

        private string flashImage;
        private string onlineStatusImage;
        private bool onlineStatus;
        private bool userLoggedIn;
        private string platform;
        private Color sapConnectionTypeColor = Color.FromHex("#ffffff");
        private string loginFailed;
        private bool printRunning;

        FileService FileService = new FileService();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Platform
        {
            get { return platform; }
            set
            {
                platform = value;
                OnPropertyChanged("Platform");
            }
        }

        public bool PrintRunning
        {
            get { return printRunning; }
            set
            {
                printRunning = value;
                OnPropertyChanged("PrintRunning");
            }
        }

        public Color SapConnectionTypeColor
        {
            get { return sapConnectionTypeColor; }
            set
            {
                sapConnectionTypeColor = value;
                OnPropertyChanged("SapConnectionTypeColor");
            }
        }

        public bool UserLoggedIn
        {
            get { return userLoggedIn; }
            set
            {
                userLoggedIn = value;
                App.AppStatus.UserLoggedIn = value;
                OnPropertyChanged("UserLoggedIn");
            }
        }

        public bool OnlineStatus
        {
            get { return onlineStatus; }
            set
            {
               
                if (onlineStatus != value || OnlineStatusImage == null)
                {
                    OnlineStatusImage = (value) ? FileService.checkImageExist("online") : FileService.checkImageExist("offline");
                }
                onlineStatus = value;
                App.AppStatus.StatusOnline = value;
                OnPropertyChanged("OnlineStatus");
            }
        }

        public string OnlineStatusImage
        {
            get { return onlineStatusImage; }
            set
            {
                onlineStatusImage = value;
                OnPropertyChanged("OnlineStatusImage");
            }
        }

        public string FlashImage
        {
            get { return flashImage; }
            set
            {
                flashImage = value;
                OnPropertyChanged("FlashImage");
            }
        }

        public string LoginFailed
        {
            get { return loginFailed; }
            set
            {
                loginFailed = (value == "1") ? LangResources.LoginFailed : null;
                OnPropertyChanged("LoginFailed");
            }
        }
    }
}
