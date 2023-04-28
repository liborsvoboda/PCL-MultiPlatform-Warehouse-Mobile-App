using System.Threading.Tasks;
using Terminal.Singleton;
using Terminal.Views;
using Xamarin.Forms;
using Terminal.Database;

namespace Terminal
{
    class FormFunctions
    {
        public static async void reloadApp(int menuId = -1)
        {
            Application.Current.MainPage = new MainPage();
            if (menuId > -1)
            {
                MainPage RootPage = Application.Current.MainPage as MainPage;
                await RootPage.NavigateFromMenu(menuId);
            }
            GlobalResources.Current.SapConnectionTypeColor = Color.FromHex(App.SapConnections.navColor);
            // Application.Current.MainPage = new MainPage();
            // await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
        }

        public static async Task waitigForm(INavigation navigation, bool stop = false)
        {
            await navigation.PushModalAsync(new WaitingPage());
            if (stop)
            {
                await navigation.PopModalAsync();
                await Task.Delay(1000);
                await navigation.PopModalAsync();
            }
            else await Task.Delay(1000);
        }

        public static SapDefinitions.SapUIID ClearSnPart(ref SapDefinitions.SapUIID SapUIIDGenerated)
        {
            SapUIIDGenerated.inputText = SapUIIDGenerated.snInserted = SapUIIDGenerated.UiidRequest = null;
            return SapUIIDGenerated;
        }

        public static SapDefinitions.SapUIID ClearPMUiidPart(ref SapDefinitions.SapUIID SapUIIDGenerated)
        {
            SapUIIDGenerated.inputText = SapUIIDGenerated.snInserted = SapUIIDGenerated.pnInserted = SapUIIDGenerated.mnFormated = SapUIIDGenerated.mnInserted = SapUIIDGenerated.UiidRequest = null;
            return SapUIIDGenerated;
        }

    }
}
