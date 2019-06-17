using Invent.Common.Helpers;
using Invent.Common.Models;
using Invent.Common.Models.Local;
using Invent.Common.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Invent.UIForms.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;

        private readonly NetService netService;

        private readonly DbService dbService;

        private List<Product> myProducts;

        private ObservableCollection<ProductItemViewModel> products;

        private bool isRefreshing;

        private bool isVisible;

        private int numberOfItems;

        private string currentInventory;

        private string inventoryImage;

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetValue(ref this.isVisible, value);
        }


        public int NumberOfItems
        {
            get => this.numberOfItems;
            set => this.SetValue(ref this.numberOfItems, value);
        }

        public string CurrentInventory
        {
            get => this.currentInventory;
            set => this.SetValue(ref this.currentInventory, value);
        }

        public string InventoryImage
        {
            get => this.inventoryImage;
            set => this.SetValue(ref this.inventoryImage, value);
        }


        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.netService = new NetService();
            this.dbService = new DbService();
            this.LoadProducts();
        }


        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.netService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //await Application.Current.MainPage.DisplayAlert(
                //    "No connection",
                //    connection.Message,
                //    "Accept");

                this.IsVisible = true;

                var localProducts = Transform.ToProducts((List<LocalProduct>)this.dbService.GetDB<LocalProduct>().Result);

                if (localProducts != null)
                {
                    this.myProducts = localProducts;
                }
                else
                {
                    return;
                }
            }
            else
            {
                var url = Application.Current.Resources["UrlAPI"].ToString();

                var response = await this.apiService.GetListAsync<Product>(
                    url,
                    "/api",
                    "/products",
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

                this.myProducts = (List<Product>)response.Result;

                if (this.myProducts != null)
                {
                    List<LocalProduct> localProducts = Transform.ToLocalProducts(this.myProducts);

                    this.dbService.StoreDB(localProducts);
                }             
            }

            this.IsRefreshing = false;

            this.RefreshProductList();
        }

        private async void GetInventoryImage()
        {
            this.IsRefreshing = true;

            var connection = await this.netService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //await Application.Current.MainPage.DisplayAlert(
                //    "Error",
                //    connection.Message,
                //    "Accept");
                //return;

                var localInventories = (List<LocalInventory>)this.dbService.GetDB<LocalInventory>().Result;

                if (localInventories != null)
                {
                    this.InventoryImage = localInventories.First().ImageFullPath;
                }

                this.IsRefreshing = false;
            }
            else
            {
                var url = Application.Current.Resources["UrlAPI"].ToString();

                var response = await this.apiService.GetListAsync<Picker2Items>(
                    url,
                    "/api",
                    "/products/inventories",
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

                var inventories = (List<Picker2Items>)response.Result;

                if (inventories != null)
                {
                    this.InventoryImage = inventories.First().ImageFullPath;

                    var localInventories = Transform.ToLocalInventories(inventories);

                    //this.dbService.StoreDB((List<LocalInventory>)localInventories);
                }             
            }          
        }

        public void AddProductsToList(Product product)
        {
            this.myProducts.Add(product);
            this.RefreshProductList();
        }

        public void UpdateProductInList(Product product)
        {
            var previousProduct = this.myProducts.Where(p => p.ReferenceCode == product.ReferenceCode).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(product);
            this.RefreshProductList();
        }

        private void RefreshProductList()
        {
            foreach (var product in this.myProducts)
            {
                if (product.ImageFullPath == null)
                {
                    product.ImageFullPath = "noImage.png";
                }
            }

            var filteredProducts = this.myProducts;

            if (MainViewModel.GetInstance().UserEmail != "rui.coutinho.rodrigues@gmail.com") //ADMIN
            {
                filteredProducts = filteredProducts.FindAll(p => p.InventoryManagerName == MainViewModel.GetInstance().UserEmail);
            }

            this.NumberOfItems = filteredProducts.Count();


            if (NumberOfItems > 0)
            {
                if (MainViewModel.GetInstance().UserEmail != "rui.coutinho.rodrigues@gmail.com")
                {
                    this.CurrentInventory = filteredProducts.First().InventoryName;
                }
                else
                {
                    this.CurrentInventory = "All";
                }             
            }
            else
            {
                this.CurrentInventory = "No items yet";
            }
            

            this.Products = new ObservableCollection<ProductItemViewModel>(
                filteredProducts.Select(p => new ProductItemViewModel
                {
                    ReferenceCode = p.ReferenceCode,
                    ImageFullPath = p.ImageFullPath,
                    Value = p.Value,                 
                    InventoryManagerName = p.InventoryManagerName,
                    InventoryName = p.InventoryName,
                    Location = p.Location,
                    Status = p.Status,
                    ProductModel = p.ProductModel,
                    Category = p.Category,
                    Supplier = p.Supplier,
                    Color = p.Color,
                    Size = p.Size
                })
                .OrderByDescending(p => p.LastChangeDate)
                .ToList());

            if (MainViewModel.GetInstance().UserEmail != "rui.coutinho.rodrigues@gmail.com")
            {
                this.GetInventoryImage();
            }
            else
            {
                this.InventoryImage = "admin.png";
            }
        }
    }
}
