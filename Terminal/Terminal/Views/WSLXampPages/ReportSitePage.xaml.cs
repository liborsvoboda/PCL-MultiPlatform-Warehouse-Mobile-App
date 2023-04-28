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
using ZXing.Mobile;
using System.Linq;
using Terminal.Functions;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportSitePage : ContentPage, INotifyPropertyChanged
    {

        private SapUIID sapUIIDGenerated = new SapUIID();
        List<FunctionalLocationsResponse> functionalLocationsResponses = new List<FunctionalLocationsResponse>();
        List<FunctionalLocationsResponse> allEquipmentResponses = new List<FunctionalLocationsResponse>();
        List<MaterialResponse> materialList = new List<MaterialResponse>();

        public ReportSitePage()
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
                } else
                {
                    CallFunctionalLocationRequest();
                }
            }
            catch (Exception) { }
            finally
            {
                await FormFunctions.waitigForm(Navigation, true);
            }
        }

        private void CallMaterialRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.materialRequest(SapUIIDGenerated, true);materialList.Clear();

            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert("", LangResources.ItemNotExist, "OK");
            }
            else
            {
                SapUIIDGenerated.SapMaterialRequestSuccess = true;
                DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                pnMnLoaded.Text = (string.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();

                requestButtons.IsVisible = false; responseButtons.IsVisible = true;
                if (responseData.Count == 1)
                {
                    SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                    SapUIIDGenerated.mnFormated = responseData[0].Find(item => item.name == "Id").realValue;
                    SapUIIDGenerated.mnInserted = responseData[0].Find(item => item.name == "Id").realValue.TrimStart('0');
                    SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                    reportSiteRequest.IsVisible = false;
                    siteNumberRequest.IsVisible = true;
                    //PnMnUiid.Text = null;
                }
                else
                {
                    List<string> materialItemList = new List<string>();
                    responseData.ForEach(child => { 
                        materialList.Add(new MaterialResponse()
                        {
                            Pn = child.Find(item => item.name == "ManufacturerPartNumber").realValue,
                            Mnoriginal = child.Find(item => item.name == "Id").realValue,
                            Mnformated = child.Find(item => item.name == "Id").realValue.TrimStart('0'),
                            MaterialName = child.Find(item => item.name == "Text").realValue
                        });
                        materialItemList.Add((materialList.Count - 1).ToString() + " | " + child.Find(item => item.name == "Id").realValue.TrimStart('0') + " | " + child.Find(item => item.name == "Text").realValue);

                    });
                    materialListView.HeightRequest = 30 * responseData.Count; materialListView.ItemsSource = materialItemList; materialListView.IsVisible = true;
                }
            }
        }

        private void CallFunctionalLocationRequest()
        {
            functionalLocationsResponses = new List<FunctionalLocationsResponse>();allEquipmentResponses = new List<FunctionalLocationsResponse>();
            SapUIIDGenerated.SiteNumber = "X" + MathFunctions.DecimalToHexadecimal(Int32.Parse(siteNumber.Text)).ToUpper();
            List<List<SapResponses>> responseData = SapCommunication.functionalLocationRequest(SapUIIDGenerated);
            if (responseData.Count == 0)
            {
                DisplayAlert("", LangResources.SiteNumberNotExist, "OK");
            } else
            {
                responseData[0].Find(item => item.name == "Id").realValue.Split(',').ToList().ForEach(id => {
                    functionalLocationsResponses.Add(new FunctionalLocationsResponse
                    {
                        MaterialName = SapUIIDGenerated.Text,
                        MaterialId = SapUIIDGenerated.mnFormated,
                        FunctionalLocationsId = id
                    });
                });
            }

            if(functionalLocationsResponses.Count > 0) DatabaseCommunication.SaveSiteHistory(new SiteHistory() { site = siteNumber.Text.ToUpper() });

            functionalLocationsResponses.ForEach(item => {
                responseData = SapCommunication.equipmentSiteRequest(item);
                if (responseData.Count > 0)
                {
                    responseData.ForEach(child =>
                    {
                        allEquipmentResponses.Add(new FunctionalLocationsResponse()
                        {
                            MaterialName = item.MaterialName,
                            Id = child.Find(subitem => subitem.name == "Id").realValue,
                            MaterialId = child.Find(subitem => subitem.name == "MaterialId").realValue,
                            FunctionalLocationsId = item.FunctionalLocationsId,
                            SN = child.Find(subitem => subitem.name == "ManufacturerSerialNumber").realValue,
                            UIID = child.Find(subitem => subitem.name == "UniqueItemId").realValue,
                        });
                    });
                }
            });
            insertAllEquipments(allEquipmentResponses);
            reportSiteResponse.IsVisible = true;
        }

        private void insertAllEquipments(List<FunctionalLocationsResponse> allEquipments)
        {
            allEquipments.ForEach(equipment =>
            {
                Frame addFrame = new Frame() { BackgroundColor= (Color)Application.Current.Resources["LightColor"] };
                equipmentList.Children.Add(addFrame, 0, equipmentList.RowDefinitions.Count);

                Label addItem = new Label();
                if (!string.IsNullOrWhiteSpace(equipment.SN))
                {
                    addItem = new Label
                    {
                        Text = equipment.SN,
                        LineBreakMode = LineBreakMode.NoWrap,
                        FontSize = 16,
                        Margin = new Thickness(2, 0, 0, 0),
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                    }; equipmentList.Children.Add(addItem, 0, equipmentList.RowDefinitions.Count);
                }
             
                addItem = new Label
                {
                    Text = equipment.Id,
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    Margin = new Thickness(200, 0, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                }; equipmentList.Children.Add(addItem, 0, equipmentList.RowDefinitions.Count);

                if (!string.IsNullOrWhiteSpace(equipment.UIID))
                {
                    addItem = new Label
                    {
                        Text = equipment.UIID,
                        LineBreakMode = LineBreakMode.NoWrap,
                        FontSize = 16,
                        Margin = new Thickness(2, 20, 0, 0),
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                    }; equipmentList.Children.Add(addItem, 0, equipmentList.RowDefinitions.Count);
                }
              
                addItem = new Label
                {
                    Text = equipment.MaterialName,
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    Margin = (string.IsNullOrWhiteSpace(equipment.UIID)) ? new Thickness(2, 20, 0, 0) : new Thickness(2, 40, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                }; equipmentList.Children.Add(addItem, 0, equipmentList.RowDefinitions.Count);

                equipmentList.RowDefinitions.Add(new RowDefinition() { Height = 60 });
            });
            
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

        private SapUIID SapUIIDGenerated
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
            SapUIIDGenerated.inputText = (!string.IsNullOrWhiteSpace(PnMnUiid.Text)) ? PnMnUiid.Text.ToUpper() : null;

            List<string> itemList = new List<string>();
            if (!string.IsNullOrWhiteSpace(PnMnUiid.Text)) {
                if (string.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) && string.IsNullOrWhiteSpace(SapUIIDGenerated.pnInserted))
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

        private void searchResultsItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PnMnUiid.Text = (string)searchResults.SelectedItem;
            searchResults.IsVisible = false;
        }

        private void ClearForm()
        {
            reportSiteRequest.IsVisible = true; siteNumberRequest.IsVisible = false; requestButtons.IsVisible = true; responseButtons.IsVisible = false;
            reportSiteResponse.IsVisible = false;materialList = new List<MaterialResponse>(); 
            equipmentList.Children.Clear(); equipmentList.RowDefinitions.Clear();
            PnMnUiid.Text = siteNumber.Text = null;
            SapUIIDGenerated = new SapUIID();
            pnMnLoaded.IsVisible = false;
            if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.startInputAutoFocus) != null) { _ = PnMnUiid.Focus(); }
        }

        private void siteNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> itemList = new List<string>();
            if (!string.IsNullOrWhiteSpace(siteNumber.Text))
            {
                List<SiteHistory> foudedList = DatabaseCommunication.GetSiteHistory(new SiteHistory() { site = siteNumber.Text.ToUpper() });
                foudedList.ForEach(item => itemList.Add(item.site));
                searchSiteResults.ItemsSource = itemList;

                if (itemList.Count > 0) {if (itemList.Count == 1 && siteNumber.Text.ToUpper() == itemList[0]) { searchSiteResults.IsVisible = false; } else { searchSiteResults.IsVisible = true; } }
                else { searchSiteResults.IsVisible = false; }
            }
            else { searchSiteResults.ItemsSource = itemList; searchSiteResults.IsVisible = false; }
            saveBtn1.IsVisible = !string.IsNullOrWhiteSpace(siteNumber.Text);
        }

        private void ClearSiteNumberClicked(object sender, EventArgs e)
        {
            searchSiteResults.IsVisible = false;
            siteNumber.Text = null;
        }


        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);

                SapUIIDGenerated.pnInserted = materialList[itemIndex].Pn;
                SapUIIDGenerated.mnFormated = materialList[itemIndex].Mnoriginal;
                SapUIIDGenerated.mnInserted = materialList[itemIndex].Mnformated;
                SapUIIDGenerated.Text = materialList[itemIndex].MaterialName;
                materialListView.IsVisible = false;

                reportSiteRequest.IsVisible = false;
                siteNumberRequest.IsVisible = true;
                PnMnUiid.Text = materialList[itemIndex].Mnformated;
            }
        }

        private void searchSiteResults_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            siteNumber.Text = (string)searchSiteResults.SelectedItem;
            searchSiteResults.IsVisible = false;
        }

        private void BackClicked(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}