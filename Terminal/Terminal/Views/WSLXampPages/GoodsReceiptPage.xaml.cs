using System;
using Terminal.Languages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Terminal.DbModels;
using ZXing;
using Terminal.Singleton;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static Terminal.Database.SapDefinitions;
using SkiaSharp;
using Terminal.SKReports;
using ZXing.Mobile;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodsReceiptPage : ContentPage, INotifyPropertyChanged
    {

        private SapUIID sapUIIDGenerated = new SapUIID();

        public GoodsReceiptPage()
        {
            InitializeComponent();
            BindingContext = this;

            _scanView.Options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                TryInverted = true

        };

            GlobalResources.Current.FlashImage = Images.Images.FlashOn;
            //materialId.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            scanArea.HeightRequest = 264;
            scanArea.IsVisible = true;
            _scanView.IsVisible = true;
            _scanView.IsEnabled = true;
            _scanView.IsAnalyzing = true;
            _scanView.IsScanning = true; 
            var contentHolder = Content;
            Content = null;
            Content = contentHolder;
            //materialId.Focus();
        }


        void SendMaterialRequestIds(object sender, EventArgs e)
        {
            sapUIIDGenerated = GenerateUIID(SapUIIDGenerated);
            if (String.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) && !SapUIIDGenerated.SapMaterialRequestSuccess)
            {
                CallMaterialRequest();
            } else
            {
                CallEquipmentRequest();
            }
        }

        private void CallMaterialRequest()
        {
        
        }

        private void CallEquipmentRequest()
        {
       
        }

       

        void ScanCodeButton(object sender, EventArgs e)
        {
            if (scanArea.HeightRequest == 0) {
                scanArea.HeightRequest = 264;
                scanArea.IsVisible = true;
                _scanView.IsVisible = true;
                _scanView.IsEnabled = true;
                _scanView.IsAnalyzing = true;
                _scanView.IsScanning = true;
                var contentHolder = Content;
                Content = null;
                Content = contentHolder;
                GlobalResources.Current.FlashImage = Images.Images.FlashOn;
                //materialId.Focus();

            } else
            {
                if (_scanView.IsTorchOn) _scanView.ToggleTorch();
                scanArea.HeightRequest = 0;
                scanArea.IsVisible = false;
                _scanView.IsScanning = false;
                _scanView.IsVisible = false;
                _scanView.IsEnabled = false;
                _scanView.IsAnalyzing = false;
            }
        }


        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (_scanView.IsTorchOn) _scanView.ToggleTorch();
                scanArea.HeightRequest = 0;
                scanArea.IsVisible = false;
                _scanView.IsScanning = false;
                _scanView.IsAnalyzing = false;
                _scanView.IsVisible = false;
                _scanView.IsEnabled = false;
                GlobalResources.Current.FlashImage = Images.Images.FlashOn;

                //var test = result.ToString().Split((char)29);
                SapUIIDGenerated.inputText = result.ToString();
                SapUIIDGenerated.codeType = result.BarcodeFormat.ToString();
                GenerateUIID(SapUIIDGenerated);
                PnMnUiid.Text = result.ToString().ToUpper();
                saveBtn.Focus();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _scanView.IsScanning = false;

        }

        private void ButtonFlashlight(object sender, EventArgs e)
        {
            if (GlobalResources.Current.FlashImage == Images.Images.FlashOn)
            {
                GlobalResources.Current.FlashImage = Images.Images.FlashOff; 
            } else { GlobalResources.Current.FlashImage = Images.Images.FlashOn; }

            _scanView.ToggleTorch();
        }


     

        private void Clear_Clicked(object sender, EventArgs e)
        {
            ClearForm();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public SapUIID SapUIIDGenerated
        {
            get { return sapUIIDGenerated; }
            set
            {
                sapUIIDGenerated = value;
                OnPropertyChanged();
            }
        }


        private void PnMnUiiDList_TextChanged(object sender, TextChangedEventArgs e)
        {
            SapUIIDGenerated.codeType = null;
            SapUIIDGenerated.inputText = (!String.IsNullOrWhiteSpace(PnMnUiid.Text)) ? PnMnUiid.Text.ToUpper() : null;

            List<string> itemList = new List<string>();
            if (!String.IsNullOrWhiteSpace(PnMnUiid.Text)) {
                if (String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) && String.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted))
                {
                    List<MpPnUiidHistory> foudedList = DatabaseCommunication.GetMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                    foudedList.ForEach(item => itemList.Add(item.mpPnUiid));
                }
                else
                {
                    List<SnHistory> foudedList = DatabaseCommunication.GetSnHistory(new SnHistory() { mpPnUiid = SapUIIDGenerated.pnInserted, sn = PnMnUiid.Text.ToUpper() });
                    foudedList.ForEach(item => itemList.Add(item.sn));
                }

                searchResults.ItemsSource = itemList;

                if (itemList.Count > 0)
                {
                    if (itemList.Count == 1 && PnMnUiid.Text.ToUpper() == itemList[0]) { searchResults.IsVisible = false; } else { searchResults.IsVisible = true; }
                }
                else { searchResults.IsVisible = false; }
            } else { searchResults.ItemsSource = itemList; searchResults.IsVisible = false; }
        }

        private void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PnMnUiid.Text = (string)searchResults.SelectedItem;
            searchResults.IsVisible = false;
        }

        private void ClearForm()
        {
            //goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;
            //goodsInformationResponse.IsVisible = false;
            PnMnUiid.Text = null;
            SapUIIDGenerated = new SapUIID(); 
            res.IsVisible = false;
            insertLabel.Text = LangResources.InsertPnMnUiid;
            //materialId.Focus();
        }

    }
}