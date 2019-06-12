using Invent.Common.Models;
using Invent.Common.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private ApiService apiService;

        private NetService netService;

        private ObservableCollection<Product> products;

        private bool isRefreshing;

        private string userEmail;

        public ObservableCollection<Product> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }


        public ProductsViewModel(string userEmail)
        {
            this.userEmail = userEmail;
            this.apiService = new ApiService();
            this.netService = new NetService();
            this.LoadProducts();
        }


        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.netService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Errot",
                    response.Message,
                    "Accept");

                return;
            }

            var myProducts = (List<Product>)response.Result;

            foreach (var product in myProducts)
            {
                if (product.ImageFullPath == null)
                {
                    product.ImageFullPath = "https://inventory2019.ddns.net/images/no-image.png";
                }
            }

            this.Products = new ObservableCollection<Product>(myProducts.FindAll(p => p.InventoryManagerName == this.userEmail));
        }
    }
}
