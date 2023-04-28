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
using System.Threading.Tasks;
using System.Linq;
using Terminal.Functions;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodsIssuePage : ContentPage, INotifyPropertyChanged
    {

        private SapUIID sapUIIDGenerated = new SapUIID() { MovementCode="03", MovementIndicator="F", BatchNumber = "NEW" };
        List<MaterialResponse> materialList = new List<MaterialResponse>();
        List<FunctionalLocationsResponse> functionalLocationsResponses = new List<FunctionalLocationsResponse>();
        List<CSOrderResponse> cSOrderResponse = new List<CSOrderResponse>();
        List<TaskListResponse> taskListResponse = new List<TaskListResponse>();
        CsOrderCreateOrChangeRequest csOrderCreateOrChangeRequest = new CsOrderCreateOrChangeRequest();

        public GoodsIssuePage()
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
                if (saveBtn.Text == LangResources.Continue || saveBtn.Text == LangResources.CreateOrder) // order selected
                {
                    CallCsOrderCreateOrChangeRequest();
                    if (SapUIIDGenerated.SapCsOrderCreateOrChangeRequestSuccess) CallServiceNotificationRequest();
                    if (SapUIIDGenerated.SapServiceNotificationRequestRequestSuccess) CallCsOrderRequest07();
                    if (SapUIIDGenerated.SapCsOrderRequest07Success) CallMaterialDocumentRequest();

                }
                else if (loadBtn.Text == LangResources.Save) //save quantity
                {
                    SapUIIDGenerated.Quantity = PnMnUiid.Text;
                    PnMnUiid.Keyboard = Keyboard.Plain; insertLabel.Text = LangResources.InsertSerialNumber; loadBtn.Text = LangResources.Load; PnMnUiid.Behaviors.Clear();
                    SapUIIDGenerated.SapEquipmentRequestSuccess = true;
                    goodsIssueRequest.IsVisible = false; requestButtons.IsVisible = false;
                    responseButtons.IsVisible = true;
                    siteNumberRequest.IsVisible = true;
                }
                else if (string.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) && !SapUIIDGenerated.SapMaterialRequestSuccess)
                {
                    CallMaterialRequest();
                }
                else if ((!string.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) && !SapUIIDGenerated.SapEquipmentRequestSuccess) ||
                  (SapUIIDGenerated.SapMaterialRequestSuccess && !SapUIIDGenerated.SapEquipmentRequestSuccess && SapUIIDGenerated.ValuationCategoryId == "S"))
                {
                    //remove  from serial start
                    if (SapUIIDGenerated.SapMaterialRequestSuccess && !SapUIIDGenerated.SapEquipmentRequestSuccess && SapUIIDGenerated.ValuationCategoryId == "S") SapUIIDGenerated.inputText = SapUIIDGenerated.inputText.TrimStart('0');
                    CallEquipmentRequest();
                }
                else if (SapUIIDGenerated.SapMaterialRequestSuccess && SapUIIDGenerated.SapEquipmentRequestSuccess && !string.IsNullOrWhiteSpace(siteNumber.Text)) // load order list
                {
                    if (!SapUIIDGenerated.SapFunctionalLocationsSuccess) CallFunctionalLocationRequest();
                    if (SapUIIDGenerated.SapFunctionalLocationsSuccess) CallTaskListRequest();
                    if (SapUIIDGenerated.SapTaskListRequestSuccess) CallCsOrderRequest24();
                    if (SapUIIDGenerated.SapCsOrderRequest24Success)
                    {
                        //order selection
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                await FormFunctions.waitigForm(Navigation, true);
            }
        }

        private void CallServiceNotificationRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.serviceNotificationCreateOrChangeRequest("A1", SapUIIDGenerated.newOrderId);
            if (responseData.Count > 0 && !(responseData[0].Find(item => item.name == "Type").realValue == "E"))
            {
                SapUIIDGenerated.SapServiceNotificationRequestRequestSuccess = true;
            }
            else if (responseData.Count == 0 || (responseData.Count > 0 && (responseData[0].Find(item => item.name == "Type").realValue == "E")))
            {
                DisplayAlert(LangResources.Error, responseData[0].Find(item => item.name == "Text").realValue, "OK");
            }
        }

        private void CallCsOrderCreateOrChangeRequest()
        {
            csOrderCreateOrChangeRequest.OrderId = OrderSelected.Text;
            csOrderCreateOrChangeRequest.FunctionalLocationId = functionalLocationsResponses[0].FunctionalLocationsId;
            csOrderCreateOrChangeRequest.CategoryId = "X";
            csOrderCreateOrChangeRequest.TicketId = ticketNumber.Text;
            csOrderCreateOrChangeRequest.Quantity = (string.IsNullOrWhiteSpace(SapUIIDGenerated.Quantity)) ? "1" : SapUIIDGenerated.Quantity;
            csOrderCreateOrChangeRequest.MaterialId = functionalLocationsResponses[0].MaterialId;
            csOrderCreateOrChangeRequest.ComponentNumber = "0010";
            csOrderCreateOrChangeRequest.RequirementUnitId = SapUIIDGenerated.UnitId;


            List<List<SapResponses>> responseData = SapCommunication.csOrderCreateOrChangeRequest(csOrderCreateOrChangeRequest);
            if (responseData.Count > 0 && !(responseData[0].Find(item => item.name == "Type").realValue == "E"))
            {
                SapUIIDGenerated.newOrderId = responseData[0].Find(item => item.name == "NewId").realValue;
                SapUIIDGenerated.SapCsOrderCreateOrChangeRequestSuccess = true;
            } else if (responseData.Count == 0 || (responseData.Count > 0 && (responseData[0].Find(item => item.name == "Type").realValue == "E")))
            {
                DisplayAlert(LangResources.Error, responseData[0].Find(item => item.name == "Text").realValue, "OK");
            }
        }

        private void CallMaterialRequest(bool asEquipInfo = false)
        {
            List<List<SapResponses>> responseData = SapCommunication.materialRequest(SapUIIDGenerated, true);
            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert(LangResources.Error, LangResources.ItemNotExist, "OK");
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
                    SapUIIDGenerated.AssetClassId = responseData[0].Find(item => item.name == "AssetClassId").realValue;
                    SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                    SapUIIDGenerated.ValuationCategoryId = responseData[0].Find(item => item.name == "ValuationCategoryId").realValue;
                    SapUIIDGenerated.SerialNumberProfileId = responseData[0].Find(item => item.name == "SerialNumberProfileId").realValue;
                    SapUIIDGenerated.UnitId = responseData[0].Find(item => item.name == "UnitId").realValue;

                    if (!asEquipInfo)
                    {
                        if (SapUIIDGenerated.ValuationCategoryId != "S")
                        {
                            PnMnUiid.Behaviors.Add(new NumericValidationBehavior());
                            PnMnUiid.Keyboard = Keyboard.Numeric;
                            insertLabel.Text = LangResources.InsertQuantity;
                            loadBtn.Text = LangResources.Save;
                            scannerBtn.IsVisible = false;
                            DisplayAlert(LangResources.Warning, LangResources.MaterialIsNotSerializable, "OK");
                        }
                        else { PnMnUiid.Keyboard = Keyboard.Plain; insertLabel.Text = LangResources.InsertSerialNumber; loadBtn.Text = LangResources.Load; PnMnUiid.Behaviors.Clear(); }
                    }
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
                            AssetClassId = child.Find(item => item.name == "AssetClassId").realValue,
                            MaterialName = child.Find(item => item.name == "Text").realValue,
                            ValuationCategoryId = child.Find(item => item.name == "ValuationCategoryId").realValue,
                            SerialNumberProfileId = child.Find(item => item.name == "SerialNumberProfileId").realValue,
                            UnitId = child.Find(item => item.name == "UnitId").realValue,
                            asEquipInfo = asEquipInfo
                        });
                        materialItemList.Add((materialList.Count - 1).ToString() + " | " + child.Find(item => item.name == "Id").realValue.TrimStart('0') + " | " + child.Find(item => item.name == "Text").realValue);

                    });
                    materialListView.HeightRequest = 30 * responseData.Count; materialListView.ItemsSource = materialItemList; materialListView.IsVisible = materialItemList.Count > 0;
                }
            }
        }

        private void CallEquipmentRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.equipmentRequest(SapUIIDGenerated);

            if (responseData.Count == 0)
            {
                SapUIIDGenerated.UiidRequest = null;
                //SapUIIDGeneratedList[sapUIIDCounter].targetLocation = (!string.IsNullOrWhiteSpace(targetLocation.Text)) ? targetLocation.Text.ToUpper().Split(" / ".ToCharArray())[0] : null;
                DisplayAlert(LangResources.Warning, LangResources.SerialNumberNotExist, "OK");
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
                goodsIssueRequest.IsVisible = false; requestButtons.IsVisible = false;
                responseButtons.IsVisible = true;
                siteNumberRequest.IsVisible = true;
            }
        }

        private void CallFunctionalLocationRequest()
        {
            functionalLocationsResponses = new List<FunctionalLocationsResponse>();
            SapUIIDGenerated.SiteNumber = "X" + MathFunctions.DecimalToHexadecimal(Int32.Parse(siteNumber.Text)).ToUpper();
            List<List<SapResponses>> responseData = SapCommunication.functionalLocationRequest(SapUIIDGenerated);
            if (responseData.Count == 0)
            {
                DisplayAlert(LangResources.Error, LangResources.SiteNumberNotExist, "OK");
            }
            else
            {
                responseData[0].Find(item => item.name == "Id").realValue.Split(',').Where(item => item.Length == 20).ToList().ForEach(id => {
                    functionalLocationsResponses.Add(new FunctionalLocationsResponse
                    {
                        MaterialName = SapUIIDGenerated.Text,
                        MaterialId = SapUIIDGenerated.mnFormated,
                        FunctionalLocationsId = id
                    });
                });
            }

            if (functionalLocationsResponses.Count > 0)
            {
                DatabaseCommunication.SaveSiteHistory(new SiteHistory() { site = siteNumber.Text.ToUpper() });
                SapUIIDGenerated.SapFunctionalLocationsSuccess = true;
            }
        }

        private void CallTaskListRequest()
        {
            List<List<SapResponses>> responseData = SapCommunication.taskListRequest(SapUIIDGenerated);
            if (responseData.Count == 0)
            {
                DisplayAlert(LangResources.Error, LangResources.TaskNumberNotExist, "OK");
            } else
            {
                taskListResponse.Add(new TaskListResponse()
                {
                    TypeId = responseData[0].Find(item => item.name == "TypeId").realValue,
                    Group = responseData[0].Find(item => item.name == "Group").realValue,
                    GroupCounter = responseData[0].Find(item => item.name == "GroupCounter").realValue,
                    Number = responseData[0].Find(item => item.name == "Number").realValue
                });
                SapUIIDGenerated.SapTaskListRequestSuccess = true;
            }
        }

        private void CallCsOrderRequest07()
        {
            int gotoLimit = 0;
            again:

            List<List<SapResponses>> responseData = SapCommunication.csOrderRequest(SapUIIDGenerated.newOrderId, true);
            if (responseData.Count > 0)
            {
                SapUIIDGenerated.SapCsOrderRequest07Success = true;
            } 
            else if (responseData.Count == 0 && gotoLimit < DefaultValues.gotoLimit)
            {
                gotoLimit += 1;
                goto again;
            }
        }

        private void CallMaterialDocumentRequest()
        {
            SapUIIDGenerated.sourceLocation = siteNumber.Text;

            List<List<SapResponses>> responseData;
            responseData = SapCommunication.materialDocumentRequest(SapUIIDGenerated);

        }

        private void CallCsOrderRequest24()
        {
            List<List<SapResponses>> responseData = SapCommunication.csOrderRequest(functionalLocationsResponses.Where(item => item.FunctionalLocationsId.Length == 20).FirstOrDefault().FunctionalLocationsId, false);
            if (responseData.Count > 0)
            {
                List<string> orderList = new List<string>();
                responseData.ForEach(child => {
                    if (child.Find(subitem => subitem.name == "OrderTypeId").realValue == "ZI10"
                    && child.Find(subitem => subitem.name == "ResponsibleCostCenter").realValue == App.actualUser.costCenter
                    && child.Find(subitem => subitem.name == "FunctionalLocationId").realValue == functionalLocationsResponses.Where(item => item.FunctionalLocationsId.Length == 20).FirstOrDefault().FunctionalLocationsId
                    && child.Find(subitem => subitem.name == "WbsElementId").realValue == App.actualUser.wbs
                    ) {
                        cSOrderResponse.Add(new CSOrderResponse()
                        {
                            Id = child.Find(subitem => subitem.name == "Id").realValue,
                            OperationRoutingNumberInOrder = child.Find(subitem => subitem.name == "OperationRoutingNumberInOrder").realValue,
                            GeneralCounterForOrder = child.Find(subitem => subitem.name == "GeneralCounterForOrder").realValue,
                            TypeId = child.Find(subitem => subitem.name == "TypeId").realValue,
                            Group = child.Find(subitem => subitem.name == "Group").realValue,
                            GroupCounter = child.Find(subitem => subitem.name == "GroupCounter").realValue,
                            Number = child.Find(subitem => subitem.name == "Number").realValue,
                            MaterialId = child.Find(subitem => subitem.name == "MaterialId").realValue,
                        });
                        orderList.Add(child.Find(subitem => subitem.name == "Id").realValue);
                    }
                });
                orderSelectionList.HeightRequest = 30 * orderList.Count; orderSelectionList.ItemsSource = orderList; 
                orderSelectionList.IsVisible = orderList.Count > 0 ;

                SapUIIDGenerated.SapCsOrderRequest24Success = true;
                siteNumberRequest.IsEnabled = false;
                orderSelectionRequest.IsVisible = true;
                saveBtn.Text = orderList.Count >0 ? LangResources.Continue : LangResources.CreateOrder;
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
                    if (_scanView.IsTorchOn) _scanView.ToggleTorch();
                    scanArea.HeightRequest = 0;

                    _scanView.IsEnabled = false;
                    scanArea.IsVisible = false;
                    Grid scanAreaContent = scanArea; scanArea = null; scanArea = scanAreaContent;
                }
            }
            catch (Exception ex) { }
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

                //var test = result.Tostring().Split((char)29);
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
            }
            else { GlobalResources.Current.FlashImage = Images.Images.FlashOn; }

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

        private void searchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PnMnUiid.Text = (string)searchResults.SelectedItem;
            searchResults.IsVisible = false;
        }

        private void ClearForm()
        {
            goodsIssueRequest.IsVisible = true; requestButtons.IsVisible = true; scannerBtn.IsVisible = true;
            goodsIssueResponse.IsVisible = false; responseButtons.IsVisible = false;
            PnMnUiid.Text = null;
            SapUIIDGenerated = new SapUIID();
            pnMnLoaded.IsVisible = false;
            insertLabel.Text = LangResources.InsertPnMnUiid;
            functionalLocationsResponses = new List<FunctionalLocationsResponse>();
            cSOrderResponse = new List<CSOrderResponse>();
            siteNumberRequest.IsVisible = false;

            //materialId.Focus();
        }

        private void hideScanArea()
        {
            if (_scanView.IsTorchOn) _scanView.ToggleTorch();
            scanArea.HeightRequest = 0;
            scanArea.IsVisible = false;
            _scanView.IsScanning = false;
            _scanView.IsVisible = false;
            _scanView.IsEnabled = false;
            _scanView.IsAnalyzing = false;
        }

        private async Task activity(bool stop = false)
        {
            hideScanArea();
            await Navigation.PushModalAsync(new WaitingPage());
            if (stop)
            {
                await Navigation.PopModalAsync();
                await Task.Delay(1000);
                await Navigation.PopModalAsync();
            }
            await Task.Delay(1000);
        }

        private void siteNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> itemList = new List<string>();
            if (!string.IsNullOrWhiteSpace(siteNumber.Text))
            {
                List<SiteHistory> foudedList = DatabaseCommunication.GetSiteHistory(new SiteHistory() { site = siteNumber.Text.ToUpper() });
                foudedList.ForEach(item => itemList.Add(item.site));
                searchSiteResults.ItemsSource = itemList;

                if (itemList.Count > 0) { if (itemList.Count == 1 && siteNumber.Text.ToUpper() == itemList[0]) { searchSiteResults.IsVisible = false; } else { searchSiteResults.IsVisible = true; } }
                else { searchSiteResults.IsVisible = false; }
            }
            else { searchSiteResults.ItemsSource = itemList; searchSiteResults.IsVisible = false; }
            saveBtn.IsVisible = !string.IsNullOrWhiteSpace(siteNumber.Text);
        }

        private void ClearSiteNumberClicked(object sender, EventArgs e)
        {
            searchSiteResults.IsVisible = false;
            siteNumber.Text = null;
            ClearForm();
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

        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);

                SapUIIDGenerated.pnInserted = materialList[itemIndex].Pn;
                SapUIIDGenerated.mnFormated = materialList[itemIndex].Mnoriginal;
                SapUIIDGenerated.mnInserted = materialList[itemIndex].Mnformated;
                SapUIIDGenerated.AssetClassId = materialList[itemIndex].AssetClassId;
                SapUIIDGenerated.Text = materialList[itemIndex].MaterialName;
                materialListView.IsVisible = false;

                goodsIssueRequest.IsVisible = false;
                siteNumberRequest.IsVisible = true;
                PnMnUiid.Text = materialList[itemIndex].Mnformated;
            }
        }

        private void ticketNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            goodsIssueRequest.IsVisible = !string.IsNullOrWhiteSpace(ticketNumber.Text);
        }

        private void ClearTicketClicked(object sender, EventArgs e)
        {
            ticketNumber.Text = null;
        }

        private void CreateOrderClicked(object sender, EventArgs e)
        {
            OrderSelected.Text = new DateTimeOffset(DateTime.Now.ToUniversalTime().AddHours(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).Hours)).ToUnixTimeSeconds().ToString() + "00";
            csOrderCreateOrChangeRequest.Insert = true;
            orderSelectionList.IsVisible = false; 
            saveBtn.Text = LangResources.CreateOrder;
        }

        private void ClearOrderSelectionClicked(object sender, EventArgs e)
        {
            OrderSelected.Text = null;
            csOrderCreateOrChangeRequest.Insert = false;
            orderSelectionList.IsVisible = true;
            saveBtn.Text = LangResources.Continue;
        }

        private void OrderSelection_TextChanged(object sender, TextChangedEventArgs e)
        {
            saveBtn.IsEnabled = !string.IsNullOrWhiteSpace(OrderSelected.Text);
        }

        private void orderSelectionList_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            OrderSelected.Text = (string)orderSelectionList.SelectedItem;
            orderSelectionList.IsVisible = false;
        }
    }
}