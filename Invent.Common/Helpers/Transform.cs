using Invent.Common.Models;
using Invent.Common.Models.Local;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invent.Common.Helpers
{
    public class Transform
    {
        public static List<LocalProduct> ToLocalProducts(List<Product> list)
        {
            var localProdList = new List<LocalProduct>();

            foreach (var item in list)
            {
                localProdList.Add(new LocalProduct()
                {
                     Value = item.Value,

                    ImageFullPath = item.ImageFullPath,

                    ImageArray = item.ImageArray,

                    InventoryName = item.InventoryName,

                    InventoryManagerName = item.InventoryManagerName,

                    Location = item.Location,

                    Status = item.Status,

                    Category = item.Category,

                    ProductModel = item.ProductModel,

                    Supplier = item.Supplier,

                    Size = item.Size,

                    Color = item.Color,

                    EntryDate = item.EntryDate,

                    LastChangeDate = item.LastChangeDate,
                });
            }

            return localProdList;
        }   

        public static List<Product> ToProducts(List<LocalProduct> list)
        {
            var prodList = new List<Product>();

            foreach (var item in list)
            {
                prodList.Add(new Product()
                {
                    Value = item.Value,

                    ImageFullPath = item.ImageFullPath,

                    ImageArray = item.ImageArray,

                    InventoryName = item.InventoryName,

                    InventoryManagerName = item.InventoryManagerName,

                    Location = item.Location,

                    Status = item.Status,

                    Category = item.Category,

                    ProductModel = item.ProductModel,

                    Supplier = item.Supplier,

                    Size = item.Size,

                    Color = item.Color,

                    EntryDate = item.EntryDate,

                    LastChangeDate = item.LastChangeDate,
                });
            }

            return prodList;
        }

        public static List<LocalInventory> ToLocalInventories(List<Picker2Items> list)
        {
            var localInventList = new List<LocalInventory>();

            foreach (var item in list)
            {
                localInventList.Add(new LocalInventory()
                {
                    Name = item.Name,

                    //UserName = item.UserName,

                    ImageFullPath = item.ImageFullPath
                });
            }

            return localInventList;
        }

        public static List<Picker2Items> ToInventories(List<LocalInventory> list)
        {
            var inventList = new List<Picker2Items>();

            foreach (var item in list)
            {
                inventList.Add(new Picker2Items()
                {
                    Name = item.Name,

                    //UserName = item.UserName,

                    ImageFullPath = item.ImageFullPath
                });
            }

            return inventList;
        }

        public static List<PickerItem> ToLocations(List<LocalLocation> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToStatus(List<LocalStatus> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToCategories(List<LocalCategory> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToModels(List<LocalModel> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToSuppliers(List<LocalSupplier> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToColors(List<LocalColor> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }

        public static List<PickerItem> ToSizes(List<LocalSize> list)
        {
            var outList = new List<PickerItem>();

            foreach (var item in list)
            {
                outList.Add(new PickerItem()
                {
                    Name = item.Name
                });
            }

            return outList;
        }
    }
}
