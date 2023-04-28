using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            lblVersionNumber.Text = VersionTracking.CurrentVersion;
            lblBuildNumber.Text = VersionTracking.CurrentBuild;
        }
    }
}