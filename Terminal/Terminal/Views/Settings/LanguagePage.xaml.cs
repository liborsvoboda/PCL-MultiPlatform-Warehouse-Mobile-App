using System;
using System.Globalization;
using System.Threading;
using Terminal.Languages;
using Terminal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguagePage : ContentPage
    {
        public LanguagePage()
        {
            InitializeComponent();
            //Title = LangResources.SelectLanguage;

            picker.SelectedIndex = App.Settings.selectedLanguage == "sk" ? 2 : App.Settings.selectedLanguage == "cs" ? 1 : 0;

            BindingContext = this;
        }

        //private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (App.Settings.selectedLanguage == (picker.SelectedIndex == 0 ? "en" : picker.SelectedIndex == 1 ? "cs" : "sk"))
        //        return;
        //    CultureInfo language = new CultureInfo(picker.SelectedIndex == 0 ? "en" : picker.SelectedIndex == 1 ? "cs" : "sk");
        //    Thread.CurrentThread.CurrentUICulture = language;
        //    LangResources.Culture = language;
        //}

        void Save_Clicked(object sender, EventArgs e)
        {
            App.Settings.selectedLanguage = (picker.SelectedIndex == 0 ? "en" : picker.SelectedIndex == 1 ? "cs" : "sk");
            CultureInfo language = new CultureInfo(picker.SelectedIndex == 0 ? "en" : picker.SelectedIndex == 1 ? "cs" : "sk");
            Thread.CurrentThread.CurrentUICulture = language;
            LangResources.Culture = language;
            DatabaseCommunication.UpdateSettingsAsync(App.Settings);
            FormFunctions.reloadApp((int)MenuItemType.Settings);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}