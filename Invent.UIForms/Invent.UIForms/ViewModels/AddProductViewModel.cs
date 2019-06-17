using GalaSoft.MvvmLight.Command;
using Invent.Common.Helpers;
using Invent.Common.Models;
using Invent.Common.Models.Local;
using Invent.Common.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;


namespace Invent.UIForms.ViewModels
{
    public class AddProductViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private bool isVisible;

        private readonly ApiService apiService;
        private readonly NetService netService;
        private readonly DbService dbService;

        private ImageSource imageSource;
        private MediaFile file;

        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }


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

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetValue(ref this.isVisible, value);
        }

        public string Value { get; set; }

        //inventory
        private ObservableCollection<Picker2Items> inventoryList;
        public ObservableCollection<Picker2Items> InventoryList
        {
            get => this.inventoryList;
            set => this.SetValue(ref this.inventoryList, value);
        }

        private Picker2Items selectedInventory;
        public Picker2Items SelectedInventory
        {
            get => this.selectedInventory;
            set => this.SetValue(ref selectedInventory, value);
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

        //Category
        private ObservableCollection<PickerItem> categoryList;
        public ObservableCollection<PickerItem> CategoryList
        {
            get => this.categoryList;
            set => this.SetValue(ref this.categoryList, value);
        }

        private PickerItem selectedCategory;
        public PickerItem SelectedCategory
        {
            get => this.selectedCategory;
            set => this.SetValue(ref selectedCategory, value);
        }

        //ProductModel
        private ObservableCollection<PickerItem> productModelList;
        public ObservableCollection<PickerItem> ProductModelList
        {
            get => this.productModelList;
            set => this.SetValue(ref this.productModelList, value);
        }

        private PickerItem selectedProductModel;
        public PickerItem SelectedProductModel
        {
            get => this.selectedProductModel;
            set => this.SetValue(ref selectedProductModel, value);
        }

        //Supplier
        private ObservableCollection<PickerItem> supplierList;
        public ObservableCollection<PickerItem> SupplierList
        {
            get => this.supplierList;
            set => this.SetValue(ref this.supplierList, value);
        }

        private PickerItem selectedSupplier;
        public PickerItem SelectedSupplier
        {
            get => this.selectedSupplier;
            set => this.SetValue(ref selectedSupplier, value);
        }

        private ObservableCollection<PickerItem> colorList;
        public ObservableCollection<PickerItem> ColorList
        {
            get => this.colorList;
            set => this.SetValue(ref this.colorList, value);
        }

        private PickerItem selectedColor;
        public PickerItem SelectedColor
        {
            get => this.selectedColor;
            set => this.SetValue(ref selectedColor, value);
        }

        private ObservableCollection<PickerItem> sizeList;
        public ObservableCollection<PickerItem> SizeList
        {
            get => this.sizeList;
            set => this.SetValue(ref this.sizeList, value);
        }

        private PickerItem selectedSize;
        public PickerItem SelectedSize
        {
            get => this.selectedSize;
            set => this.SetValue(ref selectedSize, value);
        }


        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand ChangeImageCommand => new RelayCommand(this.ChangeImage);

        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            this.netService = new NetService();
            this.dbService = new DbService();
            this.ImageSource = "noImage";
            this.IsEnabled = true;
            GetInventories();
            GetLocations();
            GetStatus();
            GetCategories();
            GetModels();
            GetSuppliers();
            GetColors();
            GetSizes();
        }

        private async void GetInventories()
        {
            if (MainViewModel.GetInstance().UserEmail == "rui.coutinho.rodrigues@gmail.com")
            {
                this.IsVisible = true;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<Picker2Items>(
                url,
                "/api",
                "/products/inventories",
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

            this.InventoryList = new ObservableCollection<Picker2Items>((List<Picker2Items>)response.Result);

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

        private async void GetCategories()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/categories",
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

            this.CategoryList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void GetModels()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/productModels",
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

            this.ProductModelList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void GetSuppliers()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/suppliers",
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

            this.SupplierList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void GetColors()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/colors",
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

            this.ColorList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void GetSizes()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();

            var response = await this.apiService.GetListAsync<PickerItem>(
                url,
                "/api",
                "/products/sizes",
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

            this.SizeList = new ObservableCollection<PickerItem>((List<PickerItem>)response.Result);
        }

        private async void Save()
        {

            if (string.IsNullOrEmpty(this.Value))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a value.",
                    "Accept");

                return;
            }

            var value = decimal.Parse(this.Value);
            if (value < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   "The value must be greather than zero.",
                   "Accept");

                return;
            }

            if (MainViewModel.GetInstance().UserEmail == "rui.coutinho.rodrigues@gmail.com")
            {
                if (this.SelectedInventory == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "You must enter a inventory name.",
                        "Accept");

                    return;
                }
            }

            if (this.SelectedLocation == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a location.",
                    "Accept");

                return;
            }

            if (this.SelectedStatus == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a status.",
                    "Accept");

                return;
            }

            if (this.SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a category.",
                    "Accept");

                return;
            }

            if (this.SelectedProductModel == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a model.",
                    "Accept");

                return;
            }

            if (this.SelectedSupplier == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a supplier.",
                    "Accept");

                return;
            }

            if (this.SelectedColor == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a color.",
                    "Accept");

                return;
            }

            if (this.SelectedSize == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a size.",
                    "Accept");

                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            //Get manager inventory
            var currentLoggedUser = MainViewModel.GetInstance().UserEmail;

            var currentInventory = "";

            if (currentLoggedUser != "rui.coutinho.rodrigues@gmail.com")
            {
                currentInventory = this.InventoryList.FirstOrDefault(p => p.UserName == currentLoggedUser).Name;
            }
            else
            {
                currentInventory = this.SelectedInventory.Name;
            }

            //Build reference code tweaks
            var refSize = this.SelectedSize.Name;
            var refColor = this.SelectedColor.Name;


            if (refSize.Length == 1) refSize += "X";
            else if (refSize.Length > 2) refSize = "NA";

            if (refColor.Length == 3) refColor += "A";
            if (refColor.Length > 6) refColor = "NTAV"; //Warning: 6 is the length of the biggest color

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var product = new Product
            {
                ReferenceCode = (this.SelectedCategory.Name.Substring(0, 2) +
                                this.SelectedProductModel.Name.Substring(0, 2) +
                                refSize.Substring(0, 2) +
                                refColor.Substring(0, 4) + "$" +
                                this.SelectedSupplier.Name.Substring(0, 4) + "_")
                                .ToUpper(),
                Value = (long)value,
                //ImageFullPath = "https://inventory2019.ddns.net/images/Products/293a7bf6-ec6e-492f-bd0a-a21faa226cea.jpg",
                InventoryName = currentInventory,
                InventoryManagerName = currentLoggedUser,
                Location = this.SelectedLocation.Name,
                Status = this.SelectedStatus.Name,
                Category = this.SelectedCategory.Name,
                ProductModel = this.SelectedProductModel.Name,
                Supplier = this.SelectedSupplier.Name,
                Color = this.SelectedColor.Name,
                Size = this.SelectedSize.Name,
                EntryDate = DateTime.Now,
                LastChangeDate = DateTime.Now,
                ImageArray = imageArray
            };

            //TODO: Testing connection

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PostAsync(
                url,
                "/api",
                "/Products",
                product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   response.Message,
                   "Accept");

                return;
            }

            var newProduct = (Product)response.Result;

            MainViewModel.GetInstance().Products.AddProductsToList(newProduct);

            this.IsRunning = false;

            await Application.Current.MainPage.DisplayAlert(
                   "Product added to inventory",
                   response.Message,
                   "Accept");

            this.IsEnabled = true;
            await App.Navigator.PopAsync();
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you take the picture ",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                this.file = null;
                return;
            }

            if (source == "From Camera")
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Pictures",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    });
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
    }
}
