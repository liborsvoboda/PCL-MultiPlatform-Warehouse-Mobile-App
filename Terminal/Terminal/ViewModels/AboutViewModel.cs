using System;
using System.Windows.Input;
using Terminal.Languages;
using Xamarin.Forms;

namespace Terminal.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri(LangResources.DistributorWebPage)));
        }

        public ICommand OpenWebCommand { get; }
    }
}