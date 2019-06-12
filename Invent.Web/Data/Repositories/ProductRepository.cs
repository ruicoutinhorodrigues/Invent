using System.Collections.Generic;
using System.Linq;
using Invent.Web.Data.Entities;
using Invent.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Invent.Web.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        //public IQueryable GetAllTeste()
        //{
        //    var simpleProductList = new List<ProductOut>();

        //    foreach (var product in GetAll().ToList())
        //    {
        //        simpleProductList.Add(new ProductOut
        //        {
        //            ReferenceCode = product.ReferenceCode,
        //            Value = product.Value,
        //            ImageFullPath = product.ImageFullPath,
        //            InventoryName = product.Inventory.Name,
        //            Location = product.Location.Name,
        //            Status = product.Status.Name,
        //            ProductModel = product.ProductModel.Name,
        //            Supplier = product.Supplier.Name,
        //            Size = product.Size.Name,
        //            Color = product.Color.Name
        //        });
        //    }


        //    return (IQueryable)simpleProductList;
        //}
    }
}
