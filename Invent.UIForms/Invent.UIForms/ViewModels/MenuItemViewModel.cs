using GalaSoft.MvvmLight.Command;
using Invent.Common.Helpers;
using Invent.UIForms.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class MenuItemViewModel : Common.Models.Menu
    {
        public ICommand SelectMenuCommand => new RelayCommand(this.selectMenu);

        private async void selectMenu()
        {
            App.Master.IsPresented = false;

            switch (this.PageName)
            {
                case "AboutPage":
                    await App.Navigator.PushAsync(new AboutPage());
                    break;

                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;

                default:
                    Settings.IsRemember = false;
                    Settings.Token = string.Empty;
                    Settings.UserEmail = string.Empty;
                    Settings.UserPassword = string.Empty;

                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }
    }
}
