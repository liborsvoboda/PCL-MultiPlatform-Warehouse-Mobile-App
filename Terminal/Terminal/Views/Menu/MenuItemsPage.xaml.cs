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
    public partial class MenuItemsPage : ContentPage 
    {
       public MenuItemsViewModel viewModel;

        public MenuItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MenuItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null || !item.isEnabled)
                return;

            if (item.xamlPage == null)
            {
                await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
                MenuItemsListView.SelectedItem = null;
            } else
            {
                switch (item.xamlPage)
                {
                    case "FastGoodsInfoPage":
                        //await Navigation.PushModalAsync(new NavigationPage(new MaterialRequestPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
                        await Navigation.PushAsync(new FastGoodsInfoPage() { });
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "GoodsIssuePage":
                        await Navigation.PushAsync(new GoodsIssuePage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "GoodsReceiptPage":
                        await Navigation.PushAsync(new GoodsReceiptPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "TransferReleasePage":
                        await Navigation.PushAsync(new TransferReleasePage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "TransferReceiptPage":
                        await Navigation.PushAsync(new TransferReceiptPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "ReportSitePage":
                        await Navigation.PushAsync(new ReportSitePage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "ReportLocationPage":
                        await Navigation.PushAsync(new ReportLocationPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "LabelPrintPage":
                        await Navigation.PushAsync(new LabelPrintPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "FaultReportPrintPage":
                        await Navigation.PushAsync(new FaultReportPrintPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "GoodReceiptFromPOPage":
                        await Navigation.PushAsync(new GoodReceiptFromPOPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "StockTakingPage":
                        await Navigation.PushAsync(new StockTakingPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                    case "DataTransferPage":
                        await Navigation.PushAsync(new DataTransferPage());
                        MenuItemsListView.SelectedItem = null;
                        break;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

    }
}