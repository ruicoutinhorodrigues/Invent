using Invent.Common.Helpers;
using Invent.Web.Data;
using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Invent.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Invent.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductModelRepository productModelRepository;
        private readonly ISupplierRepository supplierRepository;


        private readonly UserManager<IdentityUser> userManager;
        private readonly DataContext context;

        public ProductsController(IProductRepository productRepository, 
                                  IInventoryRepository inventoryRepository,
                                  IStatusRepository statusRepository, 
                                  ILocationRepository locationRepository,
                                  ICategoryRepository categoryRepository,
                                  IProductModelRepository productModelRepository,
                                  ISupplierRepository supplierRepository,
                                  UserManager<IdentityUser> userManager, 
                                  DataContext contex)
        {
            this.productRepository = productRepository;
            this.inventoryRepository = inventoryRepository;
            this.statusRepository = statusRepository;
            this.locationRepository = locationRepository;
            this.categoryRepository = categoryRepository;
            this.productModelRepository = productModelRepository;
            this.supplierRepository = supplierRepository;
            this.userManager = userManager;
            this.context = contex;
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
                    EntryDate = product.EntryDate,
                    LastChangeDate = product.LastChangeDate
                };

                //TO DELETE
                if (product.Color != null) newProd.Color = product.Color.Name;
                if (product.Size != null) newProd.Size = product.Size.Name;              

                simpleProductList.Add(newProd);
            }

            return Ok(simpleProductList);
        }

        [HttpGet("Inventories")]
        public IActionResult GetInventories()
        {
            return Ok(this.inventoryRepository.GetAll());
        }

        [HttpGet("Status")]
        public IActionResult GetStatus()
        {
            return Ok(this.statusRepository.GetAll());
        }

        [HttpGet("Locations")]
        public IActionResult GetLocations()
        {
            return Ok(this.locationRepository.GetAll());
        }

        [HttpGet("Categories")]
        public IActionResult GetCategories()
        {
            return Ok(this.categoryRepository.GetAll());
        }

        [HttpGet("ProductModels")]
        public IActionResult GetProductModels()
        {
            return Ok(this.productModelRepository.GetAll());
        }

        [HttpGet("Suppliers")]
        public IActionResult GetSuppliers()
        {
            return Ok(this.supplierRepository.GetAll());
        }

        [HttpGet("Colors")]
        public IActionResult GetColors()
        {
            return Ok(this.context.Colors);
        }

        [HttpGet("Sizes")]
        public IActionResult GetSizes()
        {
            return Ok(this.context.Sizes);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Common.Models.Product productOut)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userManager.FindByEmailAsync(productOut.InventoryManagerName);

            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var imageUrl = string.Empty;
            if (productOut.ImageArray != null && productOut.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(productOut.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Products";
                var fullPath = $"~/images/Products/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }


            //Get the new index

            var newIndex = 1;

            var allProducts =  await this.productRepository.GetAll().ToListAsync();

            var contains = allProducts.Exists(p => p.ReferenceCode.Substring(0,16) == productOut.ReferenceCode);

            if (contains)
            {
                var lastProd = (await this.productRepository.GetAll().ToListAsync()).FindLast(p => p.ReferenceCode.Substring(0,16) == productOut.ReferenceCode);

                var lastIndex = Int32.Parse(lastProd.ReferenceCode.Substring(16));

                newIndex = lastIndex  + 1;               
            }


            var entityProduct = new Product
            {
                ReferenceCode = productOut.ReferenceCode + newIndex,
                Value = productOut.Value,
                ImageUrl = imageUrl,
                EntryDate = DateTime.Now,
                InventoryId = (await this.context.Inventory.FirstOrDefaultAsync(i => i.Name == productOut.InventoryName)).Id,
                LocationId = (await this.context.Locations.FirstOrDefaultAsync(l => l.Name == productOut.Location)).Id,
                StatusId = (await this.context.Status.FirstOrDefaultAsync(st => st.Name == productOut.Status)).Id,
                CategoryId = (await this.context.Categories.FirstOrDefaultAsync(ct => ct.Name == productOut.Category)).Id,
                ProductModelId = (await this.context.ProductModel.FirstOrDefaultAsync(m => m.Name == productOut.ProductModel)).Id,
                SupplierId = (await this.context.Suppliers.FirstOrDefaultAsync(sp => sp.Name == productOut.Supplier)).Id,
                ColorId = (await this.context.Colors.FirstOrDefaultAsync(cl => cl.Name == productOut.Color)).Id,
                SizeId = (await this.context.Sizes.FirstOrDefaultAsync(sz => sz.Name == productOut.Size)).Id,
                LastChangeDate = DateTime.Now
            };

            await this.productRepository.CreateAsync(entityProduct);

            var newProduct = await this.context.Products.FirstOrDefaultAsync(p => p.ReferenceCode == entityProduct.ReferenceCode);

            var newProdOut = new ProductOut()
            {
                ReferenceCode = newProduct.ReferenceCode,
                Value = newProduct.Value,
                ImageFullPath = newProduct.ImageFullPath,
                InventoryName = newProduct.Inventory.Name,
                InventoryManagerName = newProduct.Inventory.UserName,
                Location = newProduct.Location.Name,
                Status = newProduct.Status.Name,
                Category = newProduct.Category.Name,
                ProductModel = newProduct.ProductModel.Name,
                Supplier = newProduct.Supplier.Name,
                Color = newProduct.Color.Name,
                Size = newProduct.Size.Name,
                EntryDate = newProduct.EntryDate,
                LastChangeDate = newProduct.LastChangeDate
            };

            return Ok(newProdOut);
        }

        [HttpPut("{referenceCode}")]
        public async Task<IActionResult> PutProduct([FromRoute] string referenceCode, [FromBody] ProductOut productOut)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            //var oldProduct = await this.productRepository.GetByIdAsync(id);
            var oldProduct = await this.productRepository.GetAll().FirstOrDefaultAsync(p => p.ReferenceCode == referenceCode);


            if (oldProduct == null)
            {
                return this.BadRequest("Product ReferenceCode doesn't exists.");
            }

            //TODO: Upload images

            //oldProduct.ReferenceCode = productOut.ReferenceCode;
            oldProduct.Value = productOut.Value;
            //oldProduct.ImageUrl = productOut.ImageFullPath.Substring(30);
            oldProduct.LastChangeDate = DateTime.Now;
            //oldProduct.InventoryId = (await this.context.Inventory.FirstOrDefaultAsync(i => i.Name == productOut.InventoryName)).Id;

            var location = await this.context.Locations.FirstOrDefaultAsync(l => l.Name == productOut.Location);
            oldProduct.LocationId = location.Id;

            var status = await this.context.Status.FirstOrDefaultAsync(st => st.Name == productOut.Status);
            oldProduct.StatusId = status.Id;
            //oldProduct.CategoryId = (await this.context.Categories.FirstOrDefaultAsync(i => i.Name == productOut.Category)).Id;
            //oldProduct.ProductModelId = (await this.context.ProductModel.FirstOrDefaultAsync(i => i.Name == productOut.ProductModel)).Id;
            //oldProduct.SupplierId = (await this.context.Suppliers.FirstOrDefaultAsync(i => i.Name == productOut.Supplier)).Id;
            //oldProduct.ColorId = (await this.context.Colors.FirstOrDefaultAsync(i => i.Name == productOut.Color)).Id;
            //oldProduct.SizeId = (await this.context.Sizes.FirstOrDefaultAsync(i => i.Name == productOut.Size)).Id;
            oldProduct.LastChangeDate = DateTime.Now;

            await this.productRepository.UpdateAsync(oldProduct);

            var updatedProduct = await this.context.Products.FirstOrDefaultAsync(p => p.ReferenceCode == oldProduct.ReferenceCode);

            var updatedProdOut = new ProductOut()
            {
                ReferenceCode = updatedProduct.ReferenceCode,
                Value = updatedProduct.Value,
                ImageFullPath = updatedProduct.ImageFullPath,
                InventoryName = productOut.InventoryName,
                InventoryManagerName = productOut.InventoryManagerName,
                Location = location.Name,
                Status = status.Name,
                Category = productOut.Category,
                ProductModel = productOut.ProductModel,
                Supplier = productOut.Supplier,
                Color = productOut.Color,
                Size = productOut.Size,
                LastChangeDate = updatedProduct.LastChangeDate
            };

            return Ok(updatedProdOut);
        }
    }
}
