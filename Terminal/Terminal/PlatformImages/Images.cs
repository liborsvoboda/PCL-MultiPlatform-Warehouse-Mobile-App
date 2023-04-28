using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Interfaces;

namespace Terminal.Images
{
    public class Images
    {
        public static string backImage = new FileService().checkImageExist("back");
        public static string settingsImage = new FileService().checkImageExist("settings");
        public static string settingImage = new FileService().checkImageExist("setting");
        public static string aboutImage = new FileService().checkImageExist("about");
        public static string browseImage = new FileService().checkImageExist("browse");
        public static string loginImage = new FileService().checkImageExist("login");
        public static string logoutImage = new FileService().checkImageExist("logout");
        public static string saveImage = new FileService().checkImageExist("save");
        public static string nrEmpty = new FileService().checkImageExist("nrEmpty");
        public static string cancelImage = new FileService().checkImageExist("cancel");
        public static string flashOn = new FileService().checkImageExist("flash_on");
        public static string flashOff = new FileService().checkImageExist("flash_off");
        public static string flashImage = new FileService().checkImageExist("flash_on");
        public static string checkedImage = new FileService().checkImageExist("checked");
        public static string uncheckedImage = new FileService().checkImageExist("unchecked");
        public static string previous = new FileService().checkImageExist("previous");
        public static string next = new FileService().checkImageExist("next");
        public static string plus = new FileService().checkImageExist("plus");

        public static string Plus
        {
            get { return plus; }
            set
            {
                plus = value;
            }
        }

        public static string Previous
        {
            get { return previous; }
            set
            {
                previous = value;
            }
        }

        public static string Next
        {
            get { return next; }
            set
            {
                next = value;
            }
        }
        public static string Checked
        {
            get { return checkedImage; }
            set
            {
                checkedImage = value;
            }
        }

        public static string Unchecked
        {
            get { return uncheckedImage; }
            set
            {
                uncheckedImage = value;
            }
        }

        public static string NrEmpty
        {
            get { return nrEmpty; }
            set
            {
                nrEmpty = value;
            }
        }

        public static string BackImage
        {
            get { return backImage; }
            set
            {
                backImage = value;
            }
        }

        public static string SettingsImage
        {
            get { return settingsImage; }
            set
            {
                settingsImage = value;
            }
        }

        public static string SettingImage
        {
            get { return settingImage; }
            set
            {
                settingImage = value;
            }
        }
        public static string AboutImage
        {
            get { return aboutImage; }
            set
            {
                aboutImage = value;
            }
        }
        public static string BrowseImage
        {
            get { return browseImage; }
            set
            {
                browseImage = value;
            }
        }

        public static string LoginImage
        {
            get { return loginImage; }
            set
            {
                loginImage = value;
            }
        }

        public static string LogoutImage
        {
            get { return logoutImage; }
            set
            {
                logoutImage = value;
            }
        }

        public static string SaveImage
        {
            get { return saveImage; }
            set
            {
                saveImage = value;
            }
        }

        public static string CancelImage
        {
            get { return cancelImage; }
            set
            {
                cancelImage = value;
            }
        }

        public static string FlashOn
        {
            get { return flashOn; }
            set
            {
                flashOn = value;
            }
        }

        public static string FlashOff
        {
            get { return flashOff; }
            set
            {
                flashOff = value;
            }
        }
        
    }
}
