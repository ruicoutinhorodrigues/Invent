using GalaSoft.MvvmLight.Command;
using Invent.Common.Models;
using Invent.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class EditProductViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public Product Product { get; set; }
        public long NewValue { get; set; }

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

        //Location
        private ObservableCollection<PickerItem> locationList;
        public ObservableCollection<PickerItem> LocationList
        {
            get => this.locationList;
            set => this.SetValue(ref this.locationList, value);
        }

        private PickerItem selectedLocation;
        public PickerItem SelectedLocation
        {
            get => this.selectedLocation;
            set => this.SetValue(ref selectedLocation, value);
        }

        //Status
        private ObservableCollection<PickerItem> statusList;
        public ObservableCollection<PickerItem> StatusList
        {
            get => this.statusList;
            set => this.SetValue(ref this.statusList, value);
        }

        private PickerItem selectedStatus;
        public PickerItem SelectedStatus
        {
            get => this.selectedStatus;
            set => this.SetValue(ref selectedStatus, value);
        }

        public ICommand SaveCommand => new RelayCommand(this.Save);


        //public ICommand DeleteCommand => new RelayCommand(this.Delete);


        public EditProductViewModel(Product product)
        {
            this.Product = product;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            GetLocations();
            GetStatus();
        }

        private async void GetLocations()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/locations",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Errot",
                    response.Message,
                    "Accept");

                return;
            }

            this.LocationList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void GetStatus()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/status",
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Errot",
                    response.Message,
                    "Accept");

                return;
            }

            this.StatusList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);

        }

        private async void Save()
        {

            if (this.NewValue < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The value must be a equal or greather than zero.", "Accept");
                return;
            }

            if (this.SelectedLocation != null)
            {
                Product.Location = this.SelectedLocation.Name;
            }

            if (this.SelectedStatus != null)
            {
                Product.Status = this.selectedStatus.Name;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            if (Product.Value != this.NewValue && this.NewValue > 0)
            {
                this.Product.Value = this.NewValue;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/Products",
                this.Product.ReferenceCode,
                this.Product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var modifiedProduct = (Product)response.Result;
            MainViewModel.GetInstance().Products.UpdateProductInList(modifiedProduct);
            await App.Navigator.PopAsync();

        }

        //private async void Delete()
        //{
        //    var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete the product?", "Yes", "No");
        //    if (!confirm)
        //    {
        //        return;
        //    }

        //    this.IsRunning = true;
        //    this.IsEnabled = false;

        //    //TODO: Test Internet Connection

        //    var url = Application.Current.Resources["UrlAPI"].ToString();
        //    var response = await this.apiService.DeleteAsync(
        //        url,
        //        "/api",
        //        "/Products",
        //        this.Product.Id,
        //        "bearer",
        //        MainViewModel.GetInstance().Token.Token);

        //    this.IsRunning = false;
        //    this.IsEnabled = true;

        //    if (!response.IsSuccess)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
        //        return;
        //    }

        //    MainViewModel.GetInstance().Products.DeleteProductInList(this.Product.Id);
        //    await App.Navigator.PopAsync();
        //}
    }
}
