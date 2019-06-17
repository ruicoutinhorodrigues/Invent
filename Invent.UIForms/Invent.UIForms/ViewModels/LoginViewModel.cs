using GalaSoft.MvvmLight.Command;
using Invent.Common.Helpers;
using Invent.Common.Models;
using Invent.Common.Models.Local;
using Invent.Common.Services;
using Invent.UIForms.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

        private readonly NetService netService;

        private readonly DbService dbService;

        private bool isRunning;

        private bool isEnabled;

        public bool IsRemember { get; set; }

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
            this.Email = "snob09.dev@gmail.com";
            this.Password = "Lagarto#75";
            this.apiService = new ApiService();
            this.netService = new NetService();
            this.dbService = new DbService();
            this.isEnabled = true;
            this.IsRemember = true;
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

            TokenResponse token = new TokenResponse();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                //await Application.Current.MainPage.DisplayAlert(
                //    "Error",
                //    connection.Message,
                //    "Accept");

                foreach (var user in (List<LocalUser>)this.dbService.GetDB<LocalUser>().Result)
                {
                    if (user.Email == this.Email && user.Password == this.Password) continue;
                }

                //return;
            }
            else
            {
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

                token = (TokenResponse)response.Result;

                //Save user in DB

                this.dbService.StoreDBItem<LocalUser>(new LocalUser() { Email = this.Email, Password = this.Password });
            }
        

            var mainViewModel = MainViewModel.GetInstance();

            mainViewModel.UserEmail = this.Email;

            mainViewModel.UserPassword = this.Password;

            mainViewModel.Token = token;

            mainViewModel.Products = new ProductsViewModel();

            Settings.IsRemember = this.IsRemember;
            Settings.UserEmail = this.Email;
            Settings.UserPassword = this.Password;
            Settings.Token = JsonConvert.SerializeObject(token);



           

            Application.Current.MainPage = new MasterPage();
        }
    }
}
