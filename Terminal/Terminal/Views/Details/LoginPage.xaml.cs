using System;
using Terminal.Languages;
using Terminal.DbModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Timers;
using System.ComponentModel;
using Terminal.Functions;

namespace Terminal.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
    {
        private static Timer messageTimer = new Timer() { Enabled = false, Interval = 3000 };

        public LoginPage()
        {
            InitializeComponent();
            messageTimer.Elapsed += new ElapsedEventHandler(CurrentTimerTick);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            usernameEntry.Focus();           
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            App.actualUser.username = usernameEntry.Text;
            App.actualUser.password = passwordEntry.Text;



            //if (LDapFunctions.ldapLogin(ref App.actualUser))
            //{
                Singleton.GlobalResources.Current.UserLoggedIn = true;
                //App.actualUser.location = "4002";
                App.actualUser.password = MathFunctions.HashSHA512(App.actualUser.password);
                await DatabaseCommunication.SaveUserAsync(App.actualUser);

                Application.Current.MainPage = new MainPage();
                ////await Navigation.PopModalAsync();
            //}
            //else
            //{
            //    Singleton.GlobalResources.Current.LoginFailed = "1";
            //    messageTimer.Start();
            //    await DisplayAlert(LangResources.Login, LangResources.LoginFailed, "OK");
            //}
        }
        

        private void CurrentTimerTick(object sender, ElapsedEventArgs e)
        {
            Singleton.GlobalResources.Current.LoginFailed = null;
            messageTimer.Stop();
        }

       
    }
}