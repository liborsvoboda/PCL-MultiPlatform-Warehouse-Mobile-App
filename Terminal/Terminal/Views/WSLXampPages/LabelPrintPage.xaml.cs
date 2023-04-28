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
    public partial class LabelPrintPage : ContentPage, INotifyPropertyChanged
    {

        private SapUIID sapUIIDGenerated = new SapUIID();
        List<MaterialResponse> materialList = new List<MaterialResponse>();

        public LabelPrintPage()
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
                //remove  from serial start
                if (SapUIIDGenerated.SapMaterialRequestSuccess && !SapUIIDGenerated.SapEquipmentRequestSuccess && SapUIIDGenerated.ValuationCategoryId == "S") SapUIIDGenerated.inputText = SapUIIDGenerated.inputText.TrimStart('0');

                CallEquipmentRequest();
            }
        }

        private void CallMaterialRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.materialRequest(SapUIIDGenerated, false);
            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert("", LangResources.MaterialIsNotSerializable, "OK");
            }
            else
            {
                if (responseData.Count == 1)
                {
                    SapUIIDGenerated.SapMaterialRequestSuccess = true;
                    DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                    insertLabel.Text = LangResources.InsertSerialNumber;
                    res.IsVisible = true;
                    res.Text = (String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                    SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                    SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                    SapUIIDGenerated.ValuationCategoryId = responseData[0].Find(item => item.name == "ValuationCategoryId").realValue;
                    PnMnUiid.Text = null;

                    if (SapUIIDGenerated.ValuationCategoryId != "S")
                    {
                        goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;
                        Navigation.PushModalAsync(new DefaultLabelPage(SapUIIDGenerated));
                        ClearForm();
                    }
                }
                else
                {
                    List<String> materialItemList = new List<String>();
                    responseData.ForEach(child => {
                        materialList.Add(new MaterialResponse()
                        {
                            Pn = child.Find(item => item.name == "ManufacturerPartNumber").realValue,
                            Mnoriginal = child.Find(item => item.name == "Id").realValue,
                            Mnformated = child.Find(item => item.name == "Id").realValue.TrimStart('0'),
                            MaterialName = child.Find(item => item.name == "Text").realValue,
                            ValuationCategoryId = child.Find(item => item.name == "ValuationCategoryId").realValue,

                        });
                        materialItemList.Add((materialList.Count - 1).ToString() + " | " + child.Find(item => item.name == "Id").realValue.TrimStart('0') + " | " + child.Find(item => item.name == "Text").realValue);

                    });
                    materialListView.HeightRequest = 30 * responseData.Count; materialListView.ItemsSource = materialItemList; materialListView.IsVisible = true;
                }
            }
        }

        private void CallEquipmentRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.equipmentRequest(SapUIIDGenerated);

            if (responseData.Count == 0)
            {
                SapUIIDGenerated.UiidRequest = null;
                DisplayAlert("", LangResources.SerialNumberNotExist, "OK");
            }
            else
            {
                SapUIIDGenerated.snInserted = responseData[0].Find(item => item.name == "ManufacturerSerialNumber").realValue;
                SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;

                if (!SapUIIDGenerated.SapMaterialRequestSuccess) DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                else DatabaseCommunication.SaveSnHistory(new SnHistory() { mpPnUiid = (String.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted) ? SapUIIDGenerated.mnInserted : SapUIIDGenerated.pnInserted), sn = SapUIIDGenerated.snInserted });

                SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                SapUIIDGenerated.EquipmentNumber = responseData[0].Find(item => item.name == "Id").realValue;
                SapUIIDGenerated.FunctionalLocationId = responseData[0].Find(item => item.name == "FunctionalLocationId").realValue.Split('-')[2].TrimStart('Y');
                SapUIIDGenerated.Status = responseData[0].Find(item => item.name == "Value").realValue;
                SapUIIDGenerated = SapUIIDGenerated;

                goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;

                Navigation.PushModalAsync(new DefaultLabelPage(SapUIIDGenerated));
                ClearForm();
            }
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
                SapUIID SapUIIDGeneratedCheck = GenerateUIID(SapUIIDGenerated);
                PnMnUiid.Text = (!string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.UiidRequest) ? SapUIIDGeneratedCheck.UiidRequest :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.snInserted) ? SapUIIDGeneratedCheck.snInserted :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.mnInserted) ? SapUIIDGeneratedCheck.mnInserted :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.pnInserted) ? SapUIIDGeneratedCheck.pnInserted :
                SapUIIDGeneratedCheck.inputText).ToString().ToUpper();
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

            saveBtn.IsVisible = !String.IsNullOrWhiteSpace(SapUIIDGenerated.inputText);
        }

        private void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PnMnUiid.Text = (string)searchResults.SelectedItem;
            searchResults.IsVisible = false;
        }

        private void ClearForm()
        {
            //goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;
            PnMnUiid.Text = null;
            SapUIIDGenerated = new SapUIID(); 
            res.IsVisible = false;
            insertLabel.Text = LangResources.InsertPnMnUiid;
            materialList = new List<MaterialResponse>();
            //materialId.Focus();
        }

        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);

                SapUIIDGenerated.SapMaterialRequestSuccess = true;
                DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                insertLabel.Text = LangResources.InsertSerialNumber;
                res.IsVisible = true;
                res.Text = (String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                SapUIIDGenerated.pnInserted = materialList[itemIndex].Pn;
                SapUIIDGenerated.Text = materialList[itemIndex].MaterialName;
                SapUIIDGenerated.ValuationCategoryId = materialList[itemIndex].ValuationCategoryId;
                PnMnUiid.Text = null;

                if (SapUIIDGenerated.ValuationCategoryId != "S")
                {
                    goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;
                    Navigation.PushModalAsync(new DefaultLabelPage(SapUIIDGenerated));
                    ClearForm();
                }

                materialListView.IsVisible = false;
            }
        }
    }
}