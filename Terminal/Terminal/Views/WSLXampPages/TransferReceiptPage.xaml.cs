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
using Terminal.Models;
using System.Linq;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferReceiptPage : ContentPage, INotifyPropertyChanged
    {
        private List<SapUIID> sapUIIDGeneratedList = new List<SapUIID>() { new SapUIID() { MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location } };
        private SapUIID sapUIIDGeneratedSelected = new SapUIID() { MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location };
        private int sapUIIDCounter = 0;
        List<MaterialResponse> materialList = new List<MaterialResponse>();
        private bool start = true;

        public TransferReceiptPage()
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

            List<string> itemList = new List<string>();
            App.SapWareHouses.ForEach(item => itemList.Add(item.location + " / " + item.description));
            searchSourceLocationResults.ItemsSource = itemList;
            searchSourceLocationResults.IsVisible = true;
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
                if (start) { start = TransferReceiptRequest.IsVisible = false; }
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

                if (loadBtn.Text == LangResources.Place) {
                    SapUIIDGeneratedList[sapUIIDCounter].Quantity = PnMnUiid.Text;
                    PlaceClicked(sender, new EventArgs());
                } else
                {
                    SapUIIDGeneratedList[sapUIIDCounter] = GenerateUIID(SapUIIDGeneratedList[sapUIIDCounter]);
                    if (String.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].UiidRequest) && !SapUIIDGeneratedList[sapUIIDCounter].SapMaterialRequestSuccess)
                    {
                        CallMaterialRequest();
                    }
                    else if (!String.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].UiidRequest))
                    {
                        //remove  from serial start
                        if (SapUIIDGeneratedList[sapUIIDCounter].SapMaterialRequestSuccess && !SapUIIDGeneratedList[sapUIIDCounter].SapEquipmentRequestSuccess && SapUIIDGeneratedList[sapUIIDCounter].ValuationCategoryId == "S") SapUIIDGeneratedList[sapUIIDCounter].inputText = SapUIIDGeneratedList[sapUIIDCounter].inputText.TrimStart('0');
                        CallEquipmentRequest();
                    }
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
            List<List<SapResponses>> responseData = SapCommunication.materialRequest(SapUIIDGeneratedList[sapUIIDCounter], false);
            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert("", LangResources.ItemNotExist, "OK");
            }
            else
            {
                SapUIIDGeneratedList[sapUIIDCounter].SapMaterialRequestSuccess = true;

                if (responseData.Count == 1)
                {
                    if (!asEquipInfo)
                    {
                        DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                        pnMnLoaded.Text = (String.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                        pnMnLoaded.IsVisible = true;
                    }
                    PnMnUiid.Text = null;

                    SapUIIDGeneratedList[sapUIIDCounter].mnFormated = responseData[0].Find(item => item.name == "Id").realValue;
                    SapUIIDGeneratedList[sapUIIDCounter].mnInserted = responseData[0].Find(item => item.name == "Id").realValue.TrimStart('0');
                    SapUIIDGeneratedList[sapUIIDCounter].pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                    SapUIIDGeneratedList[sapUIIDCounter].Text = responseData[0].Find(item => item.name == "Text").realValue;
                    SapUIIDGeneratedList[sapUIIDCounter].ValuationCategoryId = responseData[0].Find(item => item.name == "ValuationCategoryId").realValue;
                    SapUIIDGeneratedList[sapUIIDCounter].SerialNumberProfileId = responseData[0].Find(item => item.name == "SerialNumberProfileId").realValue;
                    SapUIIDGeneratedList[sapUIIDCounter].UnitId = responseData[0].Find(item => item.name == "UnitId").realValue;

                    if (!asEquipInfo)
                    {
                        if (SapUIIDGeneratedList[sapUIIDCounter].ValuationCategoryId != "S")
                        {
                            PnMnUiid.Behaviors.Add(new NumericValidationBehavior());
                            PnMnUiid.Keyboard = Keyboard.Numeric;
                            insertLabel.Text = LangResources.InsertQuantity;
                            loadBtn.Text = LangResources.Place;
                            DisplayAlert("", LangResources.MaterialIsNotSerializable, "OK");
                        }
                        else { PnMnUiid.Keyboard = Keyboard.Plain; insertLabel.Text = LangResources.InsertSerialNumber; loadBtn.Text = LangResources.Load; PnMnUiid.Behaviors.Clear(); }
                    }
                } else
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
            List<List<SapResponses>> responseData = SapCommunication.equipmentRequest(SapUIIDGeneratedList[sapUIIDCounter]);

            if (responseData.Count == 0)
            {
                SapUIIDGeneratedList[sapUIIDCounter].UiidRequest = null;
                //SapUIIDGeneratedList[sapUIIDCounter].targetLocation = (!String.IsNullOrWhiteSpace(targetLocation.Text)) ? targetLocation.Text.ToUpper().Split(" / ".ToCharArray())[0] : null;
                DisplayAlert("", LangResources.SerialNumberNotExist, "OK");
            }
            else
            {
                SapUIIDGeneratedList[sapUIIDCounter].snInserted = responseData[0].Find(item => item.name == "ManufacturerSerialNumber").realValue;
                SapUIIDGeneratedList[sapUIIDCounter].pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                SapUIIDGeneratedList[sapUIIDCounter].mnInserted = responseData[0].Find(item => item.name == "MaterialId").realValue.TrimStart('0');
                SapUIIDGeneratedList[sapUIIDCounter].mnFormated = responseData[0].Find(item => item.name == "MaterialId").realValue;


                if (!SapUIIDGeneratedList[sapUIIDCounter].SapMaterialRequestSuccess) DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                else DatabaseCommunication.SaveSnHistory(new SnHistory() { mpPnUiid = (String.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].pnInserted) ? SapUIIDGeneratedList[sapUIIDCounter].mnInserted : SapUIIDGeneratedList[sapUIIDCounter].pnInserted), sn = SapUIIDGeneratedList[sapUIIDCounter].snInserted });

                SapUIIDGeneratedList[sapUIIDCounter].Text = responseData[0].Find(item => item.name == "Text").realValue;
                SapUIIDGeneratedList[sapUIIDCounter].EquipmentNumber = responseData[0].Find(item => item.name == "Id").realValue;
                SapUIIDGeneratedList[sapUIIDCounter].FunctionalLocationId = responseData[0].Find(item => item.name == "FunctionalLocationId").realValue.Split('-')[2].TrimStart('Y'); SapUIIDGeneratedList[sapUIIDCounter].FunctionalLocationId = SapUIIDGeneratedList[sapUIIDCounter].FunctionalLocationId.Substring(SapUIIDGeneratedList[sapUIIDCounter].FunctionalLocationId.Length - 4, 4);
                SapUIIDGeneratedList[sapUIIDCounter].Status = responseData[0].Find(item => item.name == "Value").realValue;
                SapUIIDGeneratedList[sapUIIDCounter].BatchNumber = responseData[0].Find(item => item.name == "BatchNumber").realValue;

                if (!SapUIIDGeneratedList[sapUIIDCounter].SapMaterialRequestSuccess) CallMaterialRequest(true);
                SapUIIDGeneratedList = SapUIIDGeneratedList;
                TransferReceiptRequest.IsVisible = false; requestButtons.IsVisible = false;
                TransferReceiptResponse.IsVisible = true; responseButtons.IsVisible = true;
                warehouseRequest.IsVisible = false;
            }
        }

        private void CallMaterialDocumentRequest()
        {
            List<List<SapResponses>> responseData;
            while (SapUIIDGeneratedList.Where(item => item.preparedFor1506 == true && string.IsNullOrWhiteSpace(item.SapError)).Count() != 0)
            {
                var i = SapUIIDGeneratedList.FindIndex(item => item.preparedFor1506 == true && string.IsNullOrWhiteSpace(item.SapError));
                SapUIIDGeneratedList[i].ReceivingIssuingStorageLocationCode = SapUIIDGeneratedList[i].sourceLocation;
                SapUIIDGeneratedList[i].ReceivingIssuingBatchNumber = SapUIIDGeneratedList[i].BatchNumber;

                responseData = SapCommunication.materialDocumentRequest(SapUIIDGeneratedList[i]);
                if (responseData.Count == 0)
                {
                    DisplayAlert("", LangResources.Error, "OK");
                }
                else if (responseData[0].Find(item => item.name == "Type").realValue == "E")
                {
                    gridTransferReceiptItemList.Children.ToList().Where(child => Grid.GetRow(child) == i + 1 && child.GetType().Name == "Frame").ToList().ForEach(item => {
                        item.BackgroundColor = (Color)Application.Current.Resources["ErrorBackground"];
                    });

                    SapUIIDGeneratedList[i].SapError = responseData[0].Find(item => item.name == "Text").realValue;
                    //DisplayAlert("", responseData.Find(item => item.name == "Text").realValue, "OK");
                }
                else
                {
                    DeleteItem(i + 1);
                }
            }

            if (SapUIIDGeneratedList.Where(item => item.preparedFor1506).ToList().Count == 0)
            {
                //ClearForm();
                FormFunctions.reloadApp((int)MenuItemType.Browse);
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
                SapUIIDGeneratedList[sapUIIDCounter].inputText = result.ToString();
                SapUIIDGeneratedList[sapUIIDCounter].codeType = result.BarcodeFormat.ToString();
                SapUIID SapUIIDGeneratedCheck = GenerateUIID(SapUIIDGeneratedList[sapUIIDCounter]);
                PnMnUiid.Text = (!string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.UiidRequest) ? SapUIIDGeneratedCheck.UiidRequest :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.snInserted) ? SapUIIDGeneratedCheck.snInserted :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.mnInserted) ? SapUIIDGeneratedCheck.mnInserted :
                !string.IsNullOrWhiteSpace(SapUIIDGeneratedCheck.pnInserted) ? SapUIIDGeneratedCheck.pnInserted :
                SapUIIDGeneratedCheck.inputText).ToString().ToUpper();
                _ = loadBtn.Focus();
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

        public List<SapUIID> SapUIIDGeneratedList
        {
            get { return sapUIIDGeneratedList; }
            set
            {
                sapUIIDGeneratedList[sapUIIDCounter] = value[sapUIIDCounter];
                SapUIIDGeneratedSelected = SapUIIDGeneratedList[sapUIIDCounter];
                OnPropertyChanged();
            }
        }

        public SapUIID SapUIIDGeneratedSelected
        {
            get { return sapUIIDGeneratedSelected; }
            set
            {
                sapUIIDGeneratedSelected = value;
                if (String.IsNullOrWhiteSpace(sapUIIDGeneratedSelected.SapError)) { ErrorLabel.IsVisible = ErrorValue.IsVisible = false; } else { ErrorLabel.IsVisible = ErrorValue.IsVisible = true; }
                OnPropertyChanged();
            }
        }

        private void PnMnUiiDList_TextChanged(object sender, TextChangedEventArgs e)
        {
            SapUIIDGeneratedList[sapUIIDCounter].codeType = null;
            SapUIIDGeneratedList[sapUIIDCounter].inputText = (!string.IsNullOrWhiteSpace(PnMnUiid.Text)) ? PnMnUiid.Text.ToUpper() : null;

            List<string> itemList = new List<string>();
            if (!string.IsNullOrWhiteSpace(PnMnUiid.Text)) {
                loadBtn.IsEnabled = true;
                if (string.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].mnInserted) && string.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].pnInserted))
                {
                    List<MpPnUiidHistory> foudedList = DatabaseCommunication.GetMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                    foudedList.ForEach(item => itemList.Add(item.mpPnUiid));
                }
                else
                {
                    List<SnHistory> foudedList = DatabaseCommunication.GetSnHistory(new SnHistory() { mpPnUiid = SapUIIDGeneratedList[sapUIIDCounter].pnInserted, sn = PnMnUiid.Text.ToUpper() });
                    foudedList.ForEach(item => itemList.Add(item.sn));
                }

                searchResults.ItemsSource = itemList;

                if (itemList.Count > 0)
                {
                    if (itemList.Count == 1 && PnMnUiid.Text.ToUpper() == itemList[0]) { searchResults.IsVisible = false; } else { searchResults.IsVisible = true; }
                }
                else { searchResults.IsVisible = false; }
            } else { searchResults.ItemsSource = itemList; searchResults.IsVisible = false; loadBtn.IsEnabled = false; }
        }

        private void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PnMnUiid.Text = (string)searchResults.SelectedItem;
            searchResults.IsVisible = false;
        }

        private void ClearForm()
        {
            PnMnUiid.Keyboard = Keyboard.Plain; insertLabel.Text = LangResources.InsertPnMnUiid; loadBtn.Text = LangResources.Load;
            TransferReceiptRequest.IsVisible = true; requestButtons.IsVisible = true;
            TransferReceiptResponse.IsVisible = false; responseButtons.IsVisible = false;
            warehouseRequest.IsVisible = true;
            materialList = new List<MaterialResponse>();

            PnMnUiid.Text = null; PnMnUiid.Behaviors.Clear(); pnMnLoaded.IsVisible = false;
            SapUIIDGeneratedList[sapUIIDCounter] = new SapUIID() { MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location };
            SapUIIDGeneratedList[sapUIIDCounter].sourceLocation = (!string.IsNullOrWhiteSpace(sourceLocation.Text)) ? sourceLocation.Text.ToUpper().Split(" / ".ToCharArray())[0] : null;
            if (App.MenuConfigurations.Find(a => a.pkPageName == BindingContext.GetType().Name && a.startInputAutoFocus) != null) { _ = PnMnUiid.Focus(); }
        }

        private void resetForm()
        {
            sapUIIDGeneratedList = new List<SapUIID>() { new SapUIID() { MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location } };
            sapUIIDGeneratedSelected = new SapUIID(){ MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location };
            sapUIIDCounter = 0;
            materialList = new List<MaterialResponse>();

            var children = gridTransferReceiptItemList.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) > 0))
            {
                gridTransferReceiptItemList.Children.Remove(child);
            }
            while (gridTransferReceiptItemList.RowDefinitions.Count > 1)
            {
                gridTransferReceiptItemList.RowDefinitions.RemoveAt(gridTransferReceiptItemList.RowDefinitions.Count - 1);
            }
            transferReceiptItemList.IsVisible = false; placeBtn.IsVisible = true; issueBtn.IsVisible = false;

            ClearForm();
            SapUIIDGeneratedList[sapUIIDCounter].sourceLocation = sourceLocation.Text = null;
            sourceLocation.IsEnabled = true;
            TransferReceiptRequest.IsVisible = false; requestButtons.IsVisible = false;
            TransferReceiptResponse.IsVisible = false; responseButtons.IsVisible = false;
        }

        private void ClearSourceLocationClicked(object sender, EventArgs e)
        {
            ClearForm();
            SapUIIDGeneratedList[sapUIIDCounter].sourceLocation = sourceLocation.Text = null;
            sourceLocation.IsEnabled = true;
            TransferReceiptRequest.IsVisible = false; requestButtons.IsVisible = false;
            TransferReceiptResponse.IsVisible = false; responseButtons.IsVisible = false;
        }

        private void sourceLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            SapUIIDGeneratedList[sapUIIDCounter].sourceLocation = (!string.IsNullOrWhiteSpace(sourceLocation.Text)) ? sourceLocation.Text.ToUpper().Split(" / ".ToCharArray())[0] : null;

            List<string> itemList = new List<string>();
            if (!string.IsNullOrWhiteSpace(sourceLocation.Text))
            {
                App.SapWareHouses.FindAll(item => item.location.StartsWith(sourceLocation.Text.ToUpper()) || item.description.ToUpper().Contains(sourceLocation.Text.ToUpper())).ForEach(item => itemList.Add(item.location + " / " + item.description));
            } else
            {
                App.SapWareHouses.ForEach(item => itemList.Add(item.location + " / " + item.description));
            }

            searchSourceLocationResults.ItemsSource = itemList;

            if (itemList.Count > 0)
            {
                searchSourceLocationResults.IsVisible = true;
                if (itemList.Count == 1) {
                    sourceLocation.Text = itemList[0];
                    sourceLocation.IsEnabled = false;
                    TransferReceiptRequest.IsVisible = true;
                    requestButtons.IsVisible = true;
                    searchSourceLocationResults.IsVisible = false;
                } else { searchSourceLocationResults.IsVisible = true; }
            } else { searchSourceLocationResults.IsVisible = false; }
        }


        private void searchSourceLocation_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            sourceLocation.Text = (string)searchSourceLocationResults.SelectedItem;
            sourceLocation.IsEnabled = false;
            searchSourceLocationResults.IsVisible = false;
            TransferReceiptRequest.IsVisible = true;
            requestButtons.IsVisible = true;
        }


        private void ClearQuantityClicked(object sender, EventArgs e)
        {
            SapUIIDGeneratedList[sapUIIDCounter].Quantity = quantity.Text = null;
        }

        private void BackClicked(object sender, EventArgs e)
        {
            TransferReceiptRequest.IsVisible = true; requestButtons.IsVisible = true;
            TransferReceiptResponse.IsVisible = false; responseButtons.IsVisible = false;
            warehouseRequest.IsVisible = true; placeBtn.IsVisible = true; issueBtn.IsVisible = false;
            if (sapUIIDCounter > 0)
            {
                transferReceiptItemList.IsVisible = true;
            }
            else { transferReceiptItemList.IsVisible = false; }

        }

        private void PlaceClicked(object sender, EventArgs e)
        {
            //if (!SapUIIDGeneratedList[sapUIIDCounter].SapEquipmentRequestSuccess && SapUIIDGeneratedList[sapUIIDCounter].Quantity == null) { SapUIIDGeneratedList[sapUIIDCounter] = SapUIIDGeneratedSelected; }

            SapUIIDGeneratedList = SapUIIDGeneratedList;
            TransferReceiptRequest.IsVisible = false; requestButtons.IsVisible = false;
            TransferReceiptResponse.IsVisible = true; responseButtons.IsVisible = true;
            warehouseRequest.IsVisible = false;

            gridTransferReceiptItemList.RowDefinitions.Add(new RowDefinition() { Height = 30 });

            Frame addFrame = new Frame();
            gridTransferReceiptItemList.Children.Add(addFrame, 0, 3, sapUIIDCounter + 1, sapUIIDCounter + 2);

            ExtendedLabel addItem = new ExtendedLabel
            {
                Text = SapUIIDGeneratedList[sapUIIDCounter].sourceLocation, LineBreakMode = LineBreakMode.NoWrap, FontSize = 16,
                VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, Margin=2
            }; addItem.Clicked += new EventHandler(SelectItem);

            gridTransferReceiptItemList.Children.Add(addItem, 0, sapUIIDCounter + 1);

            addItem = new ExtendedLabel
            {
                Text = SapUIIDGeneratedList[sapUIIDCounter].Text, LineBreakMode = LineBreakMode.NoWrap, FontSize = 16,
                VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start
            }; addItem.Clicked += new EventHandler(SelectItem);
            gridTransferReceiptItemList.Children.Add(addItem, 1, sapUIIDCounter + 1);

            ImageButton deleteItem = new ImageButton()
            {
                CornerRadius = 8,
                Source = Images.Images.cancelImage, HeightRequest = 30,
                Aspect = Aspect.AspectFit, Margin = 0, HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = (Color)Application.Current.Resources["RedBackground"],

            }; deleteItem.Clicked += new EventHandler(DeleteItemClicked);
            gridTransferReceiptItemList.Children.Add(deleteItem, 2, sapUIIDCounter + 1);
            SapUIIDGeneratedList[sapUIIDCounter].preparedFor1506 = true;

            sapUIIDCounter++;
            SapUIIDGeneratedList.Add(new SapUIID() { MovementCode = "04", PlantId = "CZ02", BatchNumber = "NEW", ReceivingIssuingPlant = "CZ02", MovementTypeId = "315", targetLocation = App.actualUser.location }); 
            insertLabel.Text = LangResources.InsertPnMnUiid; PnMnUiid.Text = null; PnMnUiid.Behaviors.Clear(); loadBtn.Text = LangResources.Load;
            SapUIIDGeneratedList[sapUIIDCounter].sourceLocation = (!string.IsNullOrWhiteSpace(sourceLocation.Text)) ? sourceLocation.Text.ToUpper().Split(" / ".ToCharArray())[0] : null;

            transferReceiptItemList.IsVisible = true;
            placeBtn.IsVisible = false; issueBtn.IsVisible = true;

        }

        private void DeleteItemClicked(object sender, EventArgs e)
        {
            DeleteItem((int)(((ImageButton)sender).GetValue(Grid.RowProperty)));
        }

        private void DeleteItem(int rowId)
        {
            var children = gridTransferReceiptItemList.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) == rowId))
            {
                gridTransferReceiptItemList.Children.Remove(child);
            }
            foreach (var child in children.Where(child => Grid.GetRow(child) > rowId))
            {
                Grid.SetRow(child, Grid.GetRow(child) - 1);
            }
            gridTransferReceiptItemList.RowDefinitions.RemoveAt(gridTransferReceiptItemList.RowDefinitions.Count - 1);
            SapUIIDGeneratedList.RemoveAt(rowId - 1);
            sapUIIDCounter--; 

            if (sapUIIDCounter > 0) { transferReceiptItemList.IsVisible = true; }
            else
            {
                transferReceiptItemList.IsVisible = false;
                if (TransferReceiptResponse.IsVisible == true) { placeBtn.IsVisible = true; issueBtn.IsVisible = false; }
            }
        }


        private void SelectItem(object sender, EventArgs e)
        {
            SapUIIDGeneratedSelected = SapUIIDGeneratedList[(int)(((ExtendedLabel)sender).GetValue(Grid.RowProperty)) - 1];

            TransferReceiptResponse.IsVisible = responseButtons.IsVisible = true;
            requestButtons.IsVisible = TransferReceiptRequest.IsVisible = warehouseRequest.IsVisible = false;

            issueBtn.IsVisible = SapUIIDGeneratedList.Where(item => item.preparedFor1506 == true).Count() > 0;
            placeBtn.IsVisible = !issueBtn.IsVisible && !SapUIIDGeneratedList[sapUIIDCounter].SapEquipmentRequestSuccess;

        }

        private void IssueClicked(object sender, EventArgs e)
        {
            CallMaterialDocumentRequest();
            //SendBackButtonPressed();
            //resetForm();
        }

        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);

                if (!materialList[itemIndex].asEquipInfo)
                {
                    DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                    pnMnLoaded.Text = (String.IsNullOrWhiteSpace(SapUIIDGeneratedList[sapUIIDCounter].mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();
                    pnMnLoaded.IsVisible = true;
                }
                PnMnUiid.Text = null;

                SapUIIDGeneratedList[sapUIIDCounter].mnFormated = materialList[itemIndex].Mnoriginal;
                SapUIIDGeneratedList[sapUIIDCounter].mnInserted = materialList[itemIndex].Mnformated;
                SapUIIDGeneratedList[sapUIIDCounter].pnInserted = materialList[itemIndex].Pn;
                SapUIIDGeneratedList[sapUIIDCounter].Text = materialList[itemIndex].MaterialName;
                SapUIIDGeneratedList[sapUIIDCounter].ValuationCategoryId = materialList[itemIndex].ValuationCategoryId;
                SapUIIDGeneratedList[sapUIIDCounter].SerialNumberProfileId = materialList[itemIndex].SerialNumberProfileId;
                SapUIIDGeneratedList[sapUIIDCounter].UnitId = materialList[itemIndex].UnitId;

                if (!materialList[itemIndex].asEquipInfo)
                {
                    if (SapUIIDGeneratedList[sapUIIDCounter].ValuationCategoryId != "S")
                    {
                        PnMnUiid.Behaviors.Add(new NumericValidationBehavior());
                        PnMnUiid.Keyboard = Keyboard.Numeric;
                        insertLabel.Text = LangResources.InsertQuantity;
                        loadBtn.Text = LangResources.Place;
                        DisplayAlert("", LangResources.MaterialIsNotSerializable, "OK");
                    }
                    else { PnMnUiid.Keyboard = Keyboard.Plain; insertLabel.Text = LangResources.InsertSerialNumber; loadBtn.Text = LangResources.Load; PnMnUiid.Behaviors.Clear(); }
                }
                materialListView.IsVisible = false;
            }
        }

    }
}