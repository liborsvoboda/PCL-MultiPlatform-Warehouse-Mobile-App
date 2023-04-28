using System;
using System.Globalization;
using System.Threading;
using Terminal.Languages;
using Terminal.DbModels;
using Terminal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Terminal.Singleton;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerAddressPage : ContentPage
    {

        public Settings Item { get; set; }
        public ServerAddressPage()
        {
            InitializeComponent();
            //Title = LangResources.ServerAddress;
            Item = App.Settings;
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            App.Settings.ldapServerIp = (!string.IsNullOrWhiteSpace(ldapServerIp.Text)) ? ldapServerIp.Text : DefaultValues.ldapServerIpDefault;
            App.Settings.ldapPort = (!string.IsNullOrWhiteSpace(ldapPort.Text)) ? int.Parse(ldapPort.Text) : int.Parse(DefaultValues.ldapPortDefault);
            App.Settings.ldapDN = (!string.IsNullOrWhiteSpace(ldapDN.Text)) ? ldapDN.Text : DefaultValues.ldapDNDefault;
            App.Settings.roleDN = (!string.IsNullOrWhiteSpace(roleDN.Text)) ? roleDN.Text : DefaultValues.roleDNDefault;
            App.Settings.refreshInterval = (!string.IsNullOrWhiteSpace(refreshInterval.Text)) ? int.Parse(refreshInterval.Text) : int.Parse(DefaultValues.refreshIntervalDefault);
            App.serverAccessTimer.Interval = (double)((!string.IsNullOrWhiteSpace(refreshInterval.Text)) ? int.Parse(refreshInterval.Text) * 1000 : int.Parse(DefaultValues.refreshIntervalDefault) * 1000);
            DatabaseCommunication.UpdateSettingsAsync(App.Settings);
            await Navigation.PopModalAsync();
            FormFunctions.reloadApp((int)MenuItemType.Settings);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

      
    }
}