using Invent.Common.Models;
using Invent.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private ApiService apiService;

        private ObservableCollection<Product> _products;

        private bool _isRefreshing;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Product> Products
        {
            get { return this._products; }

            set
            {
                if (this._products != value)
                {
                    this._products = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Products"));
                }
            }
        }

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }

            set
            {
                if (this._isRefreshing != value)
                {
                    this._isRefreshing = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }


        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var response = await this.apiService.GetListAsync<Product>(
                "https://localhost:44323",
                "/api",
                "/products");

            this.IsRefreshing = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Errot",
                    response.Message,
                    "Accept");

                return;
            }

            var myProducts = (List<Product>)response.Result;
            this._products = new ObservableCollection<Product>(myProducts);
        }
    }
}
