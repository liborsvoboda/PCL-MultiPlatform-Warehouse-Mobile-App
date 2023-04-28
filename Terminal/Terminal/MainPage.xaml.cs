using Terminal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Terminal.Singleton;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }

        public async Task NavigateFromMenu(int id)
        {
            try
            {
                if (!MenuPages.ContainsKey(id) && App.AppStatus.UserLoggedIn || !App.AppStatus.UserLoggedIn && id == (int)MenuItemType.Settings || !App.AppStatus.UserLoggedIn && id == (int)MenuItemType.About) {
                    switch (id)
                    {
                        case (int)MenuItemType.Browse:
                            MenuPages.Add(id, new NavigationPage(new MenuItemsPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
                            break;
                        case (int)MenuItemType.Settings:
                            MenuPages.Add(id, new NavigationPage(new SettingsPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
                            break;
                        case (int)MenuItemType.About:
                            MenuPages.Add(id, new NavigationPage(new AboutPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
                            break;
                        case (int)MenuItemType.Logout:
                            Singleton.GlobalResources.Current.UserLoggedIn = false;
                            FormFunctions.reloadApp(id);
                            MenuPages.Clear();
                            MenuPages.Add(id, new NavigationPage(new LoginPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor });
                            break;
                    }
                } else if (!App.AppStatus.UserLoggedIn)
                {
                    MenuPages.Clear();
                    MenuPages.Add(id, new NavigationPage(new LoginPage()) { BarBackgroundColor = GlobalResources.Current.SapConnectionTypeColor});
                }
         
                var newPage = MenuPages[id];

                if (newPage != null && Detail != newPage)
                {
                    Detail = newPage;

                    if (Device.RuntimePlatform == Device.Android)
                        await Task.Delay(100);

                    IsPresented = false;
                }
            } catch (Exception) { }

        }

    }
}