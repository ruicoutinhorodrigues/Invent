using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ManufacturersController(IProductRepository productRepository, IManufacturerRepository manufacturerRepository)
        {
            this.productRepository = productRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        // GET: Manufacturers
        public IActionResult Index()
        {
            return View(this.manufacturerRepository.GetAll());
        }

        // GET: Manufacturers/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Manufacturer manufacturer, string inventoryId, string returnUrl)
        {
            if (await this.manufacturerRepository.ExistsNameAsync(manufacturer.Name))
            {
                ViewBag.AlreadyExists = "This Status already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/ProductModels/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }

            else if (ModelState.IsValid)
            {
                await this.manufacturerRepository.CreateAsync(manufacturer);

                if (inventoryId == null)
                {
                    return RedirectToAction(nameof(Index), "Choices");
                }

                return RedirectToAction(nameof(Create), "Products", new { inventoryId, returnUrl });
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await this.manufacturerRepository.GetByIdAsync(id.Value);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.manufacturerRepository.UpdateAsync(manufacturer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.manufacturerRepository.ExistsAsync(manufacturer.Id))
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
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await this.manufacturerRepository.GetByIdAsync(id.Value);

            if (manufacturer == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.ProductModel.ManufacturerId == id.Value))
            {
                return View();
            }

            await this.manufacturerRepository.DeleteAsync(manufacturer);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
