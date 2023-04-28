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
using System.Threading.Tasks;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportLocationPage : ContentPage, INotifyPropertyChanged
    {

        private SapUIID sapUIIDGenerated = new SapUIID();
        private string selectAll = Images.Images.Unchecked;
        List<checkAvaiabilityResponse> result = new List<checkAvaiabilityResponse>();
        private checkAvaiabilityResponse responseSelected = new checkAvaiabilityResponse();
        private int responseSelectedNr = 0;
        List<MaterialResponse> materialList = new List<MaterialResponse>();

        public ReportLocationPage()
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


        private async void SendMaterialRequestIds(object sender, EventArgs e)
        {
            await activity(); 
            sapUIIDGenerated = GenerateUIID(SapUIIDGenerated);
            if (String.IsNullOrWhiteSpace(SapUIIDGenerated.UiidRequest) && !SapUIIDGenerated.SapMaterialRequestSuccess)
            {
               CallMaterialRequest();
            }
            else if (SapUIIDGenerated.SapMaterialRequestSuccess)
            {
                MaterialCheckAvailabilityRequest();
            }
            await activity(true);
        }


        private void MaterialCheckAvailabilityRequest()
        {
            List<List<SapResponses>> responseData;
            for (int i = 0; i < gridAreaItemList.RowDefinitions.Count-1; i++)
            {
                SapUIIDGenerated.MrpAreaId = ((ExtendedLabel)gridAreaItemList.Children.Where(child => Grid.GetRow(child) == i + 1 && child.GetType().Name == "ExtendedLabel").First()).Text;
                responseData = SapCommunication.materialCheckAvailabilityRequest(SapUIIDGenerated);

                if (responseData.Count > 0 && !(responseData[0].Find(item => item.name == "Type").realValue == "E") && !(responseData[0].Find(item => item.name == "UnitId").realValue == null))
                {
                    result.Add(new checkAvaiabilityResponse()
                    {
                        Area = SapUIIDGenerated.MrpAreaId,
                        ItemName = SapUIIDGenerated.Text,
                        WarehouseNumber = SapUIIDGenerated.MrpAreaId.Split('_')[1],
                        WarehouseName = ((ExtendedLabel)gridAreaItemList.Children.Where(child => Grid.GetRow(child) == i + 1 && child.GetType().Name == "ExtendedLabel").Last()).Text,
                        Pn = SapUIIDGenerated.pnInserted,
                        Mn = SapUIIDGenerated.mnInserted,
                        Plant = SapUIIDGenerated.MrpAreaId.Split('_')[0],
                        QuantityFree = responseData[0].Find(item => item.name == "QuantityFree").realValue,
                        QuantityBlocked = responseData[0].Find(item => item.name == "QuantityBlocked").realValue,
                        QuantityInTransfer = responseData[0].Find(item => item.name == "QuantityInTransfer").realValue,
                        UnitId = responseData[0].Find(item => item.name == "UnitId").realValue
                    });
                }
            }

            if (result.Count > 0) {
                ResponseSelected = result[0];
                areaItemList.IsVisible = false; areasRequest.IsVisible = false;
                saveBtn1.IsVisible = false;
                goodsInformationResponse.IsVisible = true;
            }
            else
            {
                DisplayAlert("", LangResources.ItemNotFound, "OK");
            }

            requestButtons.IsVisible = false;
            responseButtons.IsVisible = true;
            responseLabel.IsVisible = result.Count == 1;
            listButtons.IsVisible = result.Count > 1;
            
        }

        private void CallMaterialRequest()
        {
            List<List<SapResponses>> responseData =SapCommunication.materialRequest(SapUIIDGenerated,true); materialList.Clear();
            if (responseData.Count == 0)
            {
                ClearForm();
                DisplayAlert("", LangResources.ItemNotExist, "OK");
            }
            else
            {

                SapUIIDGenerated.SapMaterialRequestSuccess = true;
                DatabaseCommunication.SaveMpPnUiidHistory(new MpPnUiidHistory() { mpPnUiid = PnMnUiid.Text.ToUpper() });
                res.Text = (String.IsNullOrWhiteSpace(SapUIIDGenerated.mnInserted) ? LangResources.PN + ": " : LangResources.MN + ": ") + PnMnUiid.Text.ToUpper();

                scannerBtn.IsVisible = false; saveBtn.IsVisible = false;
                if (responseData.Count == 1)
                {
                    SapUIIDGenerated.pnInserted = responseData[0].Find(item => item.name == "ManufacturerPartNumber").realValue;
                    SapUIIDGenerated.mnFormated = responseData[0].Find(item => item.name == "Id").realValue;
                    SapUIIDGenerated.mnInserted = responseData[0].Find(item => item.name == "Id").realValue.TrimStart('0');
                    SapUIIDGenerated.Text = responseData[0].Find(item => item.name == "Text").realValue;
                    goodsInformationRequest.IsVisible = false;
                    areasRequest.IsVisible = true;
                } else
                {
                    List<String> materialItemList = new List<String>();
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

        public checkAvaiabilityResponse ResponseSelected
        {
            get { return responseSelected; }
            set
            {
                responseSelected = value;
                OnPropertyChanged();
            }
        }

        public string SelectAll
        {
            get { return selectAll; }
            set
            {
                selectAll = value;
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
            goodsInformationRequest.IsVisible = true; requestButtons.IsVisible = true; responseButtons.IsVisible = false;
            goodsInformationResponse.IsVisible = false; listButtons.IsVisible = false; areaItemList.IsVisible = false; areasRequest.IsVisible = false;
            PnMnUiid.Text = selectArea.Text = null;
            result = new List<checkAvaiabilityResponse>();
            materialList = new List<MaterialResponse>();
            SapUIIDGenerated = new SapUIID(); 
            res.IsVisible = false;
            //materialId.Focus();
        }

        private void SelectAllClicked(object sender, EventArgs e)
        {
            DeleteAllAreas();

            App.SapAreas.ForEach(delegate (SapAreas area) {
                searchAreas_ItemSelected(sender, new ItemTappedEventArgs(new object(), area.plant + "_" + area.location + " / " + area.description));
            });
            ClearSelectAreaClicked(sender, e);
            saveBtn.IsVisible = true;
        }

        private void selectArea_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<String> itemList = new List<String>();
            if (!String.IsNullOrWhiteSpace(selectArea.Text))
            {
                App.SapAreas.FindAll(item => item.location.StartsWith(selectArea.Text.ToUpper()) || item.description.ToUpper().Contains(selectArea.Text.ToUpper())).ForEach(item => itemList.Add(item.plant+ "_" +item.location + " / " + item.description));
            }
            else
            {
                App.SapAreas.ForEach(item => itemList.Add(item.plant + "_" + item.location + " / " + item.description));
            }

            searchAreasResults.ItemsSource = itemList;

            if (itemList.Count > 0)
            {
                searchAreasResults.IsVisible = true;
                if (itemList.Count == 1)
                {
                    searchAreas_ItemSelected(sender, new ItemTappedEventArgs(new object(), itemList[0]));
                    searchAreasResults.IsVisible = false;
                }
                else { searchAreasResults.IsVisible = true; }
            }
            else { searchAreasResults.IsVisible = false; }
        }

        private void searchAreas_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (gridAreaItemList.Children.Where(child => child.GetType().Name == "ExtendedLabel" && ((ExtendedLabel)child).Text == e.Item.ToString().Split(" / ".ToCharArray())[0]).ToList().Count == 0) {

                ImageButton addButton = new ImageButton
                {
                    Source = Images.Images.Checked,
                    HeightRequest = 30,
                    Aspect = Aspect.AspectFit,
                    Margin = 0,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }; addButton.Clicked += new EventHandler(SelectArea);
                gridAreaItemList.Children.Add(addButton, 0, gridAreaItemList.RowDefinitions.Count);

                ExtendedLabel addItem = new ExtendedLabel
                {
                    Text = e.Item.ToString().Split(" / ".ToCharArray())[0],
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                    Margin = 2
                };

                gridAreaItemList.Children.Add(addItem, 1, gridAreaItemList.RowDefinitions.Count);

                addItem = new ExtendedLabel
                {
                    Text = e.Item.ToString().Replace(e.Item.ToString().Split(" / ".ToCharArray())[0] + " / ", ""),
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                gridAreaItemList.Children.Add(addItem, 2, gridAreaItemList.RowDefinitions.Count);
                gridAreaItemList.RowDefinitions.Add(new RowDefinition() { Height = 30 });
            }
            saveBtn.IsVisible = areaItemList.IsVisible = gridAreaItemList.RowDefinitions.Count > 1;
        }

        private void ClearSelectAreaClicked(object sender, EventArgs e)
        {
            selectArea.Text = null;
            searchAreasResults.IsVisible = false;
        }

        private void DeleteArea(int rowId)
        {
            var children = gridAreaItemList.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) == rowId))
            {
                gridAreaItemList.Children.Remove(child);
            }
            foreach (var child in children.Where(child => Grid.GetRow(child) > rowId))
            {
                Grid.SetRow(child, Grid.GetRow(child) - 1);
            }
            gridAreaItemList.RowDefinitions.RemoveAt(gridAreaItemList.RowDefinitions.Count - 1);
            saveBtn.IsVisible = areaItemList.IsVisible = gridAreaItemList.RowDefinitions.Count > 1;
        }

        private void DeleteAllAreas()
        {
            selectArea.Text = null;
            result = new List<checkAvaiabilityResponse>();
            var children = gridAreaItemList.Children.ToList();
            foreach (var child in children.Where(child => Grid.GetRow(child) > 0))
            {
                gridAreaItemList.Children.Remove(child);
            }
            while (gridAreaItemList.RowDefinitions.Count > 1)
            {
                gridAreaItemList.RowDefinitions.RemoveAt(gridAreaItemList.RowDefinitions.Count-1);
            }

            saveBtn.IsVisible = false;
        }

        private void SelectArea(object sender, EventArgs e)
        {
            DeleteArea((int)((ImageButton)sender).GetValue(Grid.RowProperty));
        }

        private void UnselectAllClicked(object sender, EventArgs e)
        {
            DeleteAllAreas();
        }

        private void BackClicked(object sender, EventArgs e)
        {
            ClearForm();
            DeleteAllAreas();
            scannerBtn.IsVisible = true;
            saveBtn.IsVisible = true;
        }

        private void NextClicked(object sender, EventArgs e)
        {
            responseSelectedNr += 1;
            ResponseSelected = result[responseSelectedNr];

            PreviousBtn.IsVisible = true;
            NextBtn.IsVisible = responseSelectedNr < result.Count -1;
        }

        private void PreviousClicked(object sender, EventArgs e)
        {
            responseSelectedNr -= 1;
            ResponseSelected = result[responseSelectedNr];

            NextBtn.IsVisible = true;
            PreviousBtn.IsVisible = responseSelectedNr > 0;
        }

        private void materialItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Item.ToString()))
            {
                int itemIndex = int.Parse(e.Item.ToString().Split(" | ".ToCharArray())[0]);

                SapUIIDGenerated.pnInserted = materialList[itemIndex].Pn;
                SapUIIDGenerated.mnFormated = materialList[itemIndex].Mnoriginal;
                SapUIIDGenerated.mnInserted = materialList[itemIndex].Mnformated;
                SapUIIDGenerated.Text = materialList[itemIndex].MaterialName;
                materialListView.IsVisible = false;

                goodsInformationRequest.IsVisible = false;
                areasRequest.IsVisible = true;
            }
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
            if (stop) { 
                await Navigation.PopModalAsync();
                await Task.Delay(1000);
                await Navigation.PopModalAsync();
            }
            await Task.Delay(1000);
        }
    }
}