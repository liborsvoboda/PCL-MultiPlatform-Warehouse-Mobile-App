using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Terminal.DbModels;
using Terminal.Languages;
using Terminal.Models;
using Terminal.Singleton;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SapSettingsPage : ContentPage
    {
        List<SapConnections> items = DatabaseCommunication.GetSapConnections(false);
        public Settings Item { get; set; }
        public SapSettingsPage()
        {
            InitializeComponent();
            //Title = LangResources.SelectLanguage;
            Item = App.Settings;

            items.ForEach(item => picker.Items.Add(item.ConnectionName));
            picker.SelectedIndex = items.FindIndex(i => i.setDefault == true);
            BindingContext = this;
        }

        //private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    App.SapConnections = items.Find(item => item.ConnectionName == picker.SelectedItem.ToString());
        //    GlobalResources.Current.SapConnectionTypeColor = Color.FromHex(App.SapConnections.navColor);
        //}

        async void Save_Clicked(object sender, EventArgs e)
        {
            App.SapConnections = items.Find(item => item.ConnectionName == picker.SelectedItem.ToString());
            GlobalResources.Current.SapConnectionTypeColor = Color.FromHex(App.SapConnections.navColor);
            DatabaseCommunication.setSapConnectionDefault(picker.SelectedItem.ToString());

            App.Settings.reportPrinterIp = (!string.IsNullOrWhiteSpace(reportPrinterIp.Text)) ? reportPrinterIp.Text : null;
            App.Settings.reportPrinterPort = (!string.IsNullOrWhiteSpace(reportPrinterPort.Text)) ? int.Parse(reportPrinterPort.Text) : int.Parse(DefaultValues.reportPrinterPortDefault);
            App.Settings.labelPrinterIp = (!string.IsNullOrWhiteSpace(labelPrinterIp.Text)) ? labelPrinterIp.Text : null;
            App.Settings.labelPrinterPort = (!string.IsNullOrWhiteSpace(labelPrinterPort.Text)) ? int.Parse(labelPrinterPort.Text) : int.Parse(DefaultValues.labelPrinterPortDefault);
            DatabaseCommunication.UpdateSettingsAsync(App.Settings);

            FormFunctions.reloadApp((int)MenuItemType.Settings);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}