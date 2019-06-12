using GalaSoft.MvvmLight.Command;
using Invent.Common.Models;
using Invent.Common.Services;
using Invent.UIForms.Views;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private ApiService apiService;

        private NetService netService;

        private bool isRunning;

        private bool isEnabled;

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

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
            this.apiService = new ApiService();
            this.netService = new NetService();
            this.isEnabled = true;
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

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new TokenRequest
            {
                Email = this.Email,
                Password = this.Password
            };

            var connection = await this.netService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetTokenAsync(
                url,
                "/AccountToken",
                "/CreateToken",
                request);

            this.isRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email or password incorrect",
                    "Accept");

                return;
            }

            var token = (TokenResponse)response.Result;

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.Token = token;

            mainViewModel.Products = new ProductsViewModel(this.Email);

            Application.Current.MainPage = new MasterPage();

            //await Application.Current.MainPage.Navigation.PushAsync(new ProductsPage());
        }
    }
}
