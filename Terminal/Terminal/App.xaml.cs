using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Terminal.Views;
using Terminal.DbModels;
using Terminal.Languages;
using System.Globalization;
using System.Timers;
using Terminal.Singleton;
using Terminal.Models;
using System.Net.Http;
using Terminal.Interfaces;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Terminal
{
    public partial class App : Application
    {
        static DatabaseCommunication database;
        public static UserHistory actualUser = new UserHistory();
        public static Settings Settings = GetDatabase().GetDefaultSettingsAsync(DbParams.Settings, true).Result[0];
        public static Timer serverAccessTimer = new Timer() { Enabled = true, Interval = 1000 };
        public static AppStatus AppStatus = new AppStatus();
        public static SapConnections SapConnections = DatabaseCommunication.GetSapConnections(true)[0];
        public static List<LabelDefinitions> LabelDefinitions = DatabaseCommunication.GetLabelDefinitions(false);
        public static List<UIIDDefinitions> UIIDDefinitions = DatabaseCommunication.GetUIIDDefinitions();
        public static List<SupplierCodesDefinitions> SupplierCodesDefinitions = DatabaseCommunication.GetSupplierCodesDefinitions();
        public static List<SapFormats> SapFormats = DatabaseCommunication.GetSapFormats();
        public static List<SapResponses> SapResponses = DatabaseCommunication.GetSapResponses();
        public static List<SapWareHouses> SapWareHouses = DatabaseCommunication.GetSapWareHouses();
        public static List<SapAreas> SapAreas = DatabaseCommunication.GetSapAreas();
        public static List<MenuConfigurations> MenuConfigurations = DatabaseCommunication.GetMenuConfigurations();
        public static List<AdvanceMenuConfigurations> AdvanceMenuConfigurations = DatabaseCommunication.GetAdvanceMenuConfigurations();

        public App()
        {
            InitializeComponent();
            LangResources.Culture = new CultureInfo(Settings.selectedLanguage);
            TranslateExtension.Init(LangResources.ResourceManager);
            FormFunctions.reloadApp((int)MenuItemType.Browse);
        }

        private static DatabaseCommunication GetDatabase()
        {
            if (database == null)
            {
                database = new DatabaseCommunication();
            }
            return database;
        }

        protected override void OnStart()
        {
            serverAccessTimer.Elapsed += new ElapsedEventHandler(OnserverAccessElapsedTime);

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void OnserverAccessElapsedTime(object source, ElapsedEventArgs e)
        {
            serverAccessTimer.Enabled = true;
            if (!string.IsNullOrWhiteSpace(Settings.ldapServerIp))
            {
                GlobalResources.Current.OnlineStatus = SystemFunctions.checkOnline(Settings.ldapServerIp, (int)Settings.ldapPort);
            } else
            {
                GlobalResources.Current.OnlineStatus = false;
            }
            serverAccessTimer.Interval = (double)((Settings.refreshInterval > 0) ? Settings.refreshInterval * 1000 : 1000);

            // only on start
            if (GlobalResources.Current.Platform == null) {
                SystemFunctions.checkPlatform();
            }

        }

    }
}
