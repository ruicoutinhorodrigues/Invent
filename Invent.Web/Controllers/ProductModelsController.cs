using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class ProductModelsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IProductModelRepository productModelRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ProductModelsController(IProductRepository productRepository, IProductModelRepository productModelRepository, IManufacturerRepository manufacturerRepository)
        {
            this.productRepository = productRepository;
            this.productModelRepository = productModelRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        // GET: ProductModels
        public IActionResult Index()
        {
            return View(this.productModelRepository.GetAll());
            //var dataContext = _context.ProductModel.Include(p => p.Manufacturer);

            //return View(await dataContext.ToListAsync());
        }

        // GET: ProductModels/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            //ViewData["ManufacturerId"] = new SelectList(_context.Set<Manufacturer>(), "Id", "Name");
            ViewData["ManufacturerId"] = new SelectList(this.manufacturerRepository.GetAll(), "Id", "Name");

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: ProductModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ManufacturerId")] ProductModel productModel, string inventoryId)
        {
            if (await this.productModelRepository.ExistsNameAsync(productModel.Name))
            {
                ViewBag.AlreadyExists = "This Model already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/Products/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }
            else if (ModelState.IsValid)
            {
                await this.productModelRepository.CreateAsync(productModel);

                if (inventoryId == null)
                {
                    return RedirectToAction(nameof(Index), "Choices");
                }
                return RedirectToAction(nameof(Create), "Products", new { inventoryId });
            }

            ViewData["ManufacturerId"] = new SelectList(this.manufacturerRepository.GetAll(), "Id", "Name", productModel.ManufacturerId);
            return View(productModel);
        }

        // GET: ProductModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await this.productModelRepository.GetByIdAsync(id.Value);

            if (productModel == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerId"] = new SelectList(this.manufacturerRepository.GetAll(), "Id", "Name", productModel.ManufacturerId);
            return View(productModel);
        }

        // POST: ProductModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ManufacturerId")] ProductModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.productModelRepository.UpdateAsync(productModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productModelRepository.ExistsAsync(productModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Choices");
            }
            ViewData["ManufacturerId"] = new SelectList(this.manufacturerRepository.GetAll(), "Id", "Name", productModel.ManufacturerId);
            return View(productModel);
        }

        // GET: ProductModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await this.productModelRepository.GetByIdAsync(id.Value);

            if (productModel == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.ProductModelId == id.Value))
            {
                return View();
            }

            await this.productModelRepository.DeleteAsync(productModel);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
