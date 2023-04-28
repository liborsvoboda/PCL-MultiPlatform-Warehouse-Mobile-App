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
using Terminal.SKReports;
using ZXing.Mobile;


namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FastGoodsInfoPage : ContentPage, INotifyPropertyChanged
    {
        private SapUIID sapUIIDGenerated = new SapUIID();
        List<MaterialResponse> materialList = new List<MaterialResponse>();

        public FastGoodsInfoPage()
        {
            InitializeComponent();
            BindingContext = this;

            _scanView.Options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                TryInverted = true,
                UseNativeScanning = true
            };

            GlobalResources.Current.FlashImage = Images.Images.FlashOn;
            PnMnUiid.Text = sapUIIDGenerated.inputText;
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                _scanView.IsAnalyzing = false;
                _scanView.IsScanning = false;
            }
            catch (Exception) { }
            return base.OnBackButtonPressed();
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.autoStartCamera) != null) { _scanView.IsEnabled = true; } else { _scanView.IsEnabled = false; scanArea.IsVisible = false; }
                Grid scanAreaContent = scanArea; scanArea = null; scanArea = scanAreaContent;
                scanArea.IsVisible = App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.hiddenCameraView) == null;
                if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.startInputAutoFocus) != null) { _ = PnMnUiid.Focus(); }
            }
            catch (Exception) { }
        }

        protected override void OnDisappearing()
        {
            try
            {
                //base.OnDisappearing();
                _scanView.IsEnabled = false;
            }
            catch (Exception) { }
        }

        private async void SendMaterialRequestIds(object sender, EventArgs e)
        {
            try
            {
                await FormFunctions.waitigForm(Navigation);
                SapUIIDGenerated = GenerateUIID(SapUIIDGenerated);
                if (string.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) && !SapUIIDGenerated.SapMaterialRequestSuccess)
                {
                    CallMaterialRequest();
                }
                else
                {
                    //remove 0 from serial start
                    if (SapUIIDGenerated.SapMaterialRequestSuccess && !SapUIIDGenerated.SapEquipmentRequestSuccess && SapUIIDGenerated.ValuationCategoryId == "S") SapUIIDGenerated.inputText = SapUIIDGenerated.inputText.TrimStart('0');
                    CallEquipmentRequest();
                }
            }
            catch (Exception) { }
            finally
            {
                await FormFunctions.waitigForm(Navigation, true);
            }
        }


        private void CallMaterialRequest(bool asEquipInfo = false)
        {
            List<List<SapResponses>> responseData = SapCommunication.materialRequest(SapUIIDGenerated, false);
            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert("", LangResources.ItemNotExist, "OK");
            }
            else
            {
                SapUIIDGenerated.SapMaterialRequestSuccess = true;
                if (responseData.Count == 1)
                {
                    if (!asEquipInfo)
                    {
                        DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                        pnMnLoaded.Text = (string.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                        pnMnLoaded.IsVisible = true;
                    }
                    PnMnUiid.Text = null;

                    SapUIIDGenerated.mnFormated = responseData[0].Find(item => item.name == "Id").realValue;
                    SapUIIDGenerated.mnInserted = responseData[0].Find(item => item.name == "Id").realValue.TrimStart('0');
                    SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                    SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                    SapUIIDGenerated.ValuationCategoryId = responseData[0].Find(item => item.name == "ValuationCategoryId").realValue;
                    SapUIIDGenerated.SerialNumberProfileId = responseData[0].Find(item => item.name == "SerialNumberProfileId").realValue;
                    SapUIIDGenerated.UnitId = responseData[0].Find(item => item.name == "UnitId").realValue;

                    if (!asEquipInfo)
                    {
                        if (SapUIIDGenerated.ValuationCategoryId != "S")
                        {
                            DisplayAlert("", LangResources.MaterialIsNotSerializable, "OK");
                            SapUIIDGenerated = SapUIIDGenerated;
                            goodsInformationRequest.IsVisible = false; requestButtons.IsVisible = false;
                            goodsInformationResponse.IsVisible = true; infoButtons.IsVisible = true;
                        }
                        else { insertLabel.Text = LangResources.InsertSerialNumber; }
                    }
                } else
                {
                    List<string> materialItemList = new List<string>();
                    responseData.ForEach(child => {
                        materialList.Add(new MaterialResponse()
                        {
                            Pn = child.Find(item => item.name == "ManufacturerPartNumber").realValue,
                            Mnoriginal = child.Find(item => item.name == "Id").realValue,
                            Mnformated = child.Find(item => item.name == "Id").realValue.TrimStart('0'),
                            MaterialName = child.Find(item => item.name == "Text").realValue,
                            ValuationCategoryId = child.Find(item => item.name == "ValuationCategoryId").realValue,
                            SerialNumberProfileId = child.Find(item => item.name == "SerialNumberProfileId").realValue,
                            UnitId = child.Find(item => item.name == "UnitId").realValue,
                            asEquipInfo = asEquipInfo
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
                SapUIIDGenerated.SapEquipmentRequestSuccess = true;
                SapUIIDGenerated.snInserted = responseData[0].Find(item => item.name == "ManufacturerSerialNumber").realValue;
                SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                SapUIIDGenerated.mnInserted = responseData[0].Find(item => item.name == "MaterialId").realValue.TrimStart('0');
                SapUIIDGenerated.mnFormated = responseData[0].Find(item => item.name == "MaterialId").realValue;
                SapUIIDGenerated.AssetClass = responseData[0].Find(item => item.name == "AssetClass").realValue;

                if (!SapUIIDGenerated.SapMaterialRequestSuccess) DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                else DatabaseCommunication.SaveSnHistory(new SnHistory() { mpPnUiid = (string.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted) ? SapUIIDGenerated.mnInserted : SapUIIDGenerated.pnInserted), sn = SapUIIDGenerated.snInserted });

                SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                SapUIIDGenerated.EquipmentNumber = responseData[0].Find(item => item.name == "Id").realValue;
                SapUIIDGenerated.FunctionalLocationId = responseData[0].Find(item => item.name == "FunctionalLocationId").realValue.Split('-')[2].TrimStart('Y'); SapUIIDGenerated.FunctionalLocationId = SapUIIDGenerated.FunctionalLocationId.Substring(SapUIIDGenerated.FunctionalLocationId.Length - 4, 4);
                SapUIIDGenerated.Status = responseData[0].Find(item => item.name == "Value").realValue;
                SapUIIDGenerated.BatchNumber = responseData[0].Find(item => item.name == "BatchNumber").realValue;

                if (!SapUIIDGenerated.SapMaterialRequestSuccess) CallMaterialRequest(true);
                SapUIIDGenerated = SapUIIDGenerated;

                goodsInformationRequest.IsVisible = false; requestButtons.IsVisible = false;
                goodsInformationResponse.IsVisible = true; infoButtons.IsVisible = true;

            }
        }


        private void ScanCodeButton(object sender, EventArgs e)
        {
            try
            {
                if (!scanArea.IsVisible)
                {
                    scanArea.HeightRequest = 264;
                    scanArea.IsVisible = true;
                    Grid scanAreaContent = scanArea; scanArea = null; scanArea = scanAreaContent;
                    GlobalResources.Current.FlashImage = Images.Images.FlashOn;
                }
                else
                {
                    if (_scanView.IsTorchOn) { _scanView.ToggleTorch(); }
                    scanArea.HeightRequest = 0;
                    _scanView.IsEnabled = false;
                    scanArea.IsVisible = false;
                    Grid scanAreaContent = scanArea; scanArea = null; scanArea = scanAreaContent;
                }
            }
            catch (Exception) { }
            finally
            {
                if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.cameraInputAutoFocus) != null) { _ = PnMnUiid.Focus(); }
            }
        }

        private void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (_scanView.IsTorchOn) _scanView.ToggleTorch();
                scanArea.HeightRequest = 0;
                scanArea.IsVisible = false;
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
                _ = saveBtn.Focus();
            });
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public SapUIID SapUIIDGenerated
        {
            get => sapUIIDGenerated;
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
            saveBtn.IsEnabled = !String.IsNullOrWhiteSpace(PnMnUiid.Text);

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
            goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true;
            goodsInformationResponse.IsVisible = false; infoButtons.IsVisible = false;
            SapUIIDGenerated = new SapUIID();
            PnMnUiid.Text = null; pnMnLoaded.IsVisible = false;
            insertLabel.Text = LangResources.InsertPnMnUiid;
            if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.startInputAutoFocus) != null) { _ = PnMnUiid.Focus(); }
        }

        private void BackButton(object sender, EventArgs e)
        {
            ClearForm();
            OnAppearing();
        }

        private void LabelClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new DefaultLabelPage(SapUIIDGenerated));
        }

        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);
                if (!materialList[itemIndex].asEquipInfo)
                {
                    DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                    pnMnLoaded.Text = (String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                    pnMnLoaded.IsVisible = true;
                }
                PnMnUiid.Text = null;

                SapUIIDGenerated.mnFormated = materialList[itemIndex].Mnoriginal;
                SapUIIDGenerated.mnInserted = materialList[itemIndex].Mnformated;
                SapUIIDGenerated.pnInserted = materialList[itemIndex].Pn;
                SapUIIDGenerated.Text = materialList[itemIndex].MaterialName;
                SapUIIDGenerated.ValuationCategoryId = materialList[itemIndex].ValuationCategoryId;
                SapUIIDGenerated.SerialNumberProfileId = materialList[itemIndex].SerialNumberProfileId;
                SapUIIDGenerated.UnitId = materialList[itemIndex].UnitId;

                if (!materialList[itemIndex].asEquipInfo)
                {
                    if (SapUIIDGenerated.ValuationCategoryId != "S")
                    {
                        DisplayAlert("", LangResources.MaterialIsNotSerializable, "OK");
                    }
                    else { insertLabel.Text = LangResources.InsertSerialNumber; }
                }

                materialListView.IsVisible = false;
            }
        }
    }
}