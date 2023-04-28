using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Terminal.Models;
using Terminal.ViewModels;
using Terminal.Languages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZXing.Net.Mobile.Forms;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;
          
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = true,
                TopText = "Please scan the barcode...",
                BottomText = string.Empty
            };

            overlay.BindingContext = overlay;

            var scanPage = new ZXingScannerPage(null, overlay);

            overlay.FlashButtonClicked += (s, ed) =>
            {
                scanPage.ToggleTorch();
            };

            scanPage.Title = "Scan";
            scanPage.DefaultOverlayShowFlashButton = true;
            scanPage.OnScanResult += (result) => {

                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushAsync(scanPage);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

     
    }

}