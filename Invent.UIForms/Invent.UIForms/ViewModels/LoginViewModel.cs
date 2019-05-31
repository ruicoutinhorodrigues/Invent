using GalaSoft.MvvmLight.Command;
using Invent.UIForms.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        public LoginViewModel()
        {
            this.Email = "rui.coutinho.rodrigues@gmail.com";
            this.Password = "Lagarto#75";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email",
                    "Accept");

                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password",
                    "Accept");

                return;
            }

            if (!this.Email.Equals("rui.coutinho.rodrigues@gmail.com") || !this.Password.Equals("Lagarto#75"))
            {
                await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Wrong user or password",
                "Accept");

                return;
            }

            MainViewModel.GetInstance().Products = new ProductsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());
        }
    }
}
