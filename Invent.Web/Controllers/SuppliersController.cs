using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ISupplierRepository supplierRepository;

        public SuppliersController(IProductRepository productRepository, ISupplierRepository supplierRepository)
        {
            this.productRepository = productRepository;
            this.supplierRepository = supplierRepository;
        }

        // GET: Suppliers
        public IActionResult Index()
        {
            return View(this.supplierRepository.GetAll());
        }

        // GET: Suppliers/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,PostCode,Town,Country,Phone,Email,Website,NIPC,DateOfCreation")] Supplier supplier, string inventoryId)
        {
            if (await this.supplierRepository.ExistsNameAsync(supplier.Name))
            {
                ViewBag.AlreadyExists = "This Supplier already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/Products/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }

            else if (ModelState.IsValid)
            {
                await this.supplierRepository.CreateAsync(supplier);

                if (inventoryId == null)
                {
                    return RedirectToAction(nameof(Index), "Choices");
                }
                return RedirectToAction(nameof(Create), "Products", new { inventoryId });
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await this.supplierRepository.GetByIdAsync(id.Value);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,PostCode,Town,Country,Phone,Email,Website,NIPC,DateOfCreation")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.supplierRepository.UpdateAsync(supplier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.supplierRepository.ExistsAsync(supplier.Id))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await this.supplierRepository.GetByIdAsync(id.Value);

            if (supplier == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.SupplierId == id.Value))
            {
                return View();
            }

            await this.supplierRepository.DeleteAsync(supplier);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
