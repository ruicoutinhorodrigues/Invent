using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Invent.Web.Data;
using Invent.Web.Data.Entities;
using Invent.Web.Models;
using System.IO;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using Invent.Web.Data.Repositories;

namespace Invent.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;

        private readonly IProductRepository productRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly IProductModelRepository productModelRepository;
        private readonly IStatusRepository statusRepository;

        public ProductsController(DataContext context, UserManager<IdentityUser> userManager, 
                                  IProductRepository productRepository, 
                                  IInventoryRepository inventoryRepository, 
                                  ITicketRepository ticketRepository,
                                  ICategoryRepository categoryRepository,
                                  ILocationRepository locationRepository,
                                  ISupplierRepository supplierRepository,
                                  IProductModelRepository productModelRepository,
                                  IStatusRepository statusRepository
                                  )
        {
            _context = context;
            _userManager = userManager;

            this.productRepository = productRepository;
            this.inventoryRepository = inventoryRepository;
            this.ticketRepository = ticketRepository;
            this.categoryRepository = categoryRepository;
            this.locationRepository = locationRepository;
            this.supplierRepository = supplierRepository;
            this.productModelRepository = productModelRepository;
            this.statusRepository = statusRepository;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? inventoryId)
        {
            var inventory = await this.inventoryRepository.GetByIdAsync(inventoryId.Value);

            if (inventory == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            if (inventory.UserName == user.UserName || user.UserName == "rui.coutinho.rodrigues@gmail.com") //admin added in seed
            {
                var products = this.productRepository.GetAll()
                               .Where(p => p.InventoryId == inventoryId)
                               .Include(p => p.Category)
                               .Include(p => p.Inventory)
                               .Include(p => p.Location)
                               .Include(p => p.ProductModel)
                               .Include(p => p.Status)
                               .Include(p => p.Supplier);

                var numberOfOpenTickets = ticketRepository.GetAll().Where(t => t.InventoryId == inventoryId && t.ClosedIssue == false).Count();
              
                ViewBag.CurrentInventory = inventory.Name;

                ViewBag.InventoryUser = user.UserName;

                ViewBag.InventoryId = inventoryId;

                ViewBag.InventoryImage = inventory.ImageUrl;

                ViewBag.OpenTickets = numberOfOpenTickets;

                return View(await products.ToListAsync());
            }

            return RedirectToAction("Index", "Inventories");
        }

        // GET: Products/Create
        public IActionResult Create(int inventoryId, string returnUrl = null)
        {
            ViewData["CategoryId"] = new SelectList(this.categoryRepository.GetAll(), "Id", "Name");
            ViewData["LocationId"] = new SelectList(this.locationRepository.GetAll(), "Id", "Name");
            ViewData["ProductModelId"] = new SelectList(this.productModelRepository.GetAll(), "Id", "Name");
            ViewData["StatusId"] = new SelectList(this.statusRepository.GetAll(), "Id", "Name");
            ViewData["SupplierId"] = new SelectList(this.supplierRepository.GetAll(), "Id", "Name");
            ViewData["SizeId"] = new SelectList(_context.Sizes.OrderBy(sz => sz.Id), "Id", "Name", 5);
            ViewData["ColorId"] = new SelectList(_context.Colors.OrderBy(cl => cl.Id), "Id", "Name", 1);

            var newProdView = new ProductViewModel()
            {
                InventoryId = inventoryId
            };

            ViewBag.InventoryName = this.inventoryRepository.GetByIdAsync(inventoryId).Result.Name;

            if (returnUrl != null)
            {
                return RedirectToAction(nameof(Create), "ProductModels", new { inventoryId});
            }

            return View(newProdView);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReferenceCode,Description,Value,ImageUrl,ImageFile,EntryDate,LastChangeDate,InventoryId,LocationId,StatusId,CategoryId,ProductModelId,SupplierId, SizeId, ColorId")] ProductViewModel productView)
        {

            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (productView.ImageFile != null && productView.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await productView.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{file}";
                }

               
                var product = (Product)productView;

                product.ImageUrl = path;
                product.EntryDate = DateTime.Now;
                product.LastChangeDate = DateTime.Now;             

                var last = await this.productRepository.GetAll()
                    .LastOrDefaultAsync(p =>            
                    p.ReferenceCode.Substring(0, 14) == product.ReferenceCode.Substring(0, 14) 

                );

                if (last == null || last.ReferenceCode == null)
                {
                    product.ReferenceCode += 1;
                }
                else
                {
                    product.ReferenceCode = product.ReferenceCode + (Convert.ToInt32(last.ReferenceCode.Substring(14)) + 1);
                }

                await this.productRepository.CreateAsync(product);

                var inventoryId = productView.InventoryId;

                return RedirectToAction(nameof(Index), new { inventoryId });
            }

            ViewData["CategoryId"] = new SelectList(this.categoryRepository.GetAll(), "Id", "Name");
            ViewData["LocationId"] = new SelectList(this.locationRepository.GetAll(), "Id", "Name");
            ViewData["ProductModelId"] = new SelectList(this.productModelRepository.GetAll(), "Id", "Name");
            ViewData["StatusId"] = new SelectList(this.statusRepository.GetAll(), "Id", "Name");
            ViewData["SupplierId"] = new SelectList(this.supplierRepository.GetAll(), "Id", "Name");
           
            ViewData["SizeId"] = new SelectList(_context.Sizes.OrderBy(sz => sz.Id), "Id", "Name");
            ViewData["ColorId"] = new SelectList(_context.Colors.OrderBy(cl => cl.Id), "Id", "Name");
            return View(productView);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["LocationId"] = new SelectList(this.locationRepository.GetAll(), "Id", "Name", product.LocationId);          
            ViewData["StatusId"] = new SelectList(this.statusRepository.GetAll(), "Id", "Name", product.StatusId);
            
            var productView = new ProductViewModel
            {
                Id = product.Id,
                ReferenceCode = product.ReferenceCode,
                Description = product.Description,
                Value = product.Value,
                ImageUrl = product.ImageUrl,
                EntryDate = product.EntryDate,
                LastChangeDate = product.LastChangeDate,
                CategoryId = product.CategoryId,
                ProductModelId = product.ProductModelId,
                SupplierId = product.SupplierId,
                StatusId = product.StatusId,
                LocationId = product.LocationId,
                InventoryId = product.InventoryId
            };

            ViewBag.InventoryName = this.inventoryRepository.GetAll().FirstOrDefaultAsync(i => i.Id == product.InventoryId).Result.Name;
            ViewBag.ProductModel = this.productModelRepository.GetAll().FirstOrDefaultAsync(i => i.Id == product.ProductModelId).Result.Name;

            if (product.SizeId != null)
            {
                ViewBag.ProductSize = _context.Sizes.FirstOrDefaultAsync(i => i.Id == product.SizeId).Result.Name;
            }

            if ( product.ColorId != null)
            {
                ViewBag.ProductColor = _context.Colors.FirstOrDefaultAsync(i => i.Id == product.ColorId).Result.Name;
            }
            
            return View(productView);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReferenceCode,Description,Value,ImageUrl,ImageFile,EntryDate,LastChangeDate,InventoryId,LocationId,StatusId,CategoryId,ProductModelId,SupplierId")] ProductViewModel productView)
        {
            if (id != productView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var path = productView.ImageUrl;

                    if (productView.ImageFile != null && productView.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await productView.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{file}";
                    }

                    var product = (Product)productView;

                    product.ImageUrl = path;
                    product.LastChangeDate = DateTime.Now;

                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await this.productRepository.ExistsAsync(productView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { productView.InventoryId });
            }


            ViewData["CategoryId"] = new SelectList(this.categoryRepository.GetAll(), "Id", "Name");
            ViewData["LocationId"] = new SelectList(this.locationRepository.GetAll(), "Id", "Name");
            ViewData["ProductModelId"] = new SelectList(this.productModelRepository.GetAll(), "Id", "Name");
            ViewData["StatusId"] = new SelectList(this.statusRepository.GetAll(), "Id", "Name");
            ViewData["SupplierId"] = new SelectList(this.supplierRepository.GetAll(), "Id", "Name");

            return View(productView);
        }


        public IActionResult Excel(int inventoryId)
        {
            var products = this.productRepository.GetAll()
                .Where(p => p.InventoryId == inventoryId)
                .OrderByDescending(p => p.LastChangeDate)
                .OrderByDescending(p => p.EntryDate)
                .ToList();

            var comlumHeadrs = new string[]
            {
                "Reference code",
                "Value",
                "Entry date",
                "Last change",
                "Location",
                "Status",
                "Category",
                "Model",
                "Supplier"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("Current inventory");

                worksheet.Cells[1, 1].Value = "Inventory: " + this.inventoryRepository.GetAll().FirstOrDefault(i => i.Id == inventoryId).Name;

                using (var cells = worksheet.Cells[3, 1, 3, 9]) 
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[3, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                var j = 4;
                foreach (var product in products)
                {
                    worksheet.Cells["A" + j].Value = product.ReferenceCode;
                    worksheet.Cells["B" + j].Value = product.Value;
                    worksheet.Cells["C" + j].Value = product.EntryDate.ToShortDateString();
                    worksheet.Cells["D" + j].Value = product.LastChangeDate.ToShortDateString();
                    worksheet.Cells["E" + j].Value = this.locationRepository.GetAll().FirstOrDefault(p => p.Id == product.LocationId).Name;
                    worksheet.Cells["F" + j].Value = this.statusRepository.GetAll().FirstOrDefault(p => p.Id == product.StatusId).Name;
                    worksheet.Cells["G" + j].Value = this.categoryRepository.GetAll().FirstOrDefault(p => p.Id == product.CategoryId).Name;
                    worksheet.Cells["H" + j].Value = this.productModelRepository.GetAll().FirstOrDefault(p => p.Id == product.ProductModelId).Name;
                    worksheet.Cells["I" + j].Value = this.supplierRepository.GetAll().FirstOrDefault(p => p.Id == product.SupplierId).Name;

                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Inventory.xlsx");
        }
    }
}
