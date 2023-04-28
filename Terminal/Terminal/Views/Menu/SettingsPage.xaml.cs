using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Terminal.Models;
using Terminal.Views;
using Terminal.ViewModels;
using Terminal.Languages;
using System.Globalization;
using System.Threading;
using Terminal.Singleton;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage 
    {
       SettingsViewModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            if (item.Text == LangResources.Language)
            {
                await Navigation.PushModalAsync(new NavigationPage(new LanguagePage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
            }
            else if (item.Text == LangResources.ldapServerIp)
            {
                await Navigation.PushModalAsync(new NavigationPage(new ServerAddressPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
            }
            else if (item.Text == LangResources.SapSettings)
            {
                await Navigation.PushModalAsync(new NavigationPage(new SapSettingsPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
            }
            else
            {
                await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            }

            SettingsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}