using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingPage : ContentPage
    {
        public WaitingPage()
        {
            InitializeComponent();
        }
    }
}