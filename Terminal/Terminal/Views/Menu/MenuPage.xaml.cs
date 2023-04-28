using Terminal.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Globalization;
using Terminal.Languages;
using Terminal.Images;

namespace Terminal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;

        public MenuPage()
        {
            InitializeComponent();

            if (App.AppStatus.UserLoggedIn) { 
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.Browse, Title=LangResources.Browse, Image= Images.Images.BrowseImage },
                    new HomeMenuItem {Id = MenuItemType.Settings, Title=LangResources.Settings, Image= Images.Images.SettingImage },
                    new HomeMenuItem {Id = MenuItemType.About, Title=LangResources.About, Image= Images.Images.AboutImage },
                    new HomeMenuItem {Id = MenuItemType.Logout, Title=LangResources.Logout, Image= Images.Images.LogoutImage }
                }; 
            } else {
                menuItems = new List<HomeMenuItem>
                {
                    new HomeMenuItem {Id = MenuItemType.Browse, Title=LangResources.Browse, Image= Images.Images.BrowseImage },
                    new HomeMenuItem {Id = MenuItemType.Settings, Title=LangResources.Settings, Image= Images.Images.SettingImage },
                    new HomeMenuItem {Id = MenuItemType.About, Title=LangResources.About, Image= Images.Images.AboutImage },
                    new HomeMenuItem {Id = MenuItemType.Login, Title=LangResources.Login, Image= Images.Images.LoginImage }
                };
            }

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                FormFunctions.reloadApp(id);
            };
        }

     
    }
}