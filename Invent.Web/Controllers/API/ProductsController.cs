using Invent.Web.Data;
using Invent.Web.Data.Repositories;
using Invent.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Invent.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        private IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            //return Ok(this.productRepository.GetAllWithInventory());

            var simpleProductList = new List<ProductOut>();


            var completeProductList = this.productRepository.GetAll()
                              .Include(p => p.Category)
                              .Include(p => p.Inventory)
                              .Include(p => p.Location)
                              .Include(p => p.ProductModel)
                              .Include(p => p.Status)
                              .Include(p => p.Category)
                              .Include(p => p.Supplier)
                              .Include(p => p.Color)
                              .Include(p => p.Size);

            foreach (var product in completeProductList)
            {
                var newProd = new ProductOut()
                {
                    ReferenceCode = product.ReferenceCode,
                    Value = product.Value,
                    ImageFullPath = product.ImageFullPath,
                    InventoryName = product.Inventory.Name,
                    InventoryManagerName = product.Inventory.UserName,
                    Location = product.Location.Name,
                    Status = product.Status.Name,
                    Category = product.Category.Name,
                    ProductModel = product.ProductModel.Name,
                    Supplier = product.Supplier.Name,
                };

                //TO DELETE
                if (product.Color != null) newProd.Color = product.Color.Name;
                if (product.Size != null) newProd.Size = product.Size.Name;
                

                simpleProductList.Add(newProd);
            }


            return Ok(simpleProductList);
        }
    }
}
