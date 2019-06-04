using Invent.Web.Data;
using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        // GET: Categories
        public IActionResult Index()
        {
            return View(this.categoryRepository.GetAll());
        }

        // GET: Categories/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category, string inventoryId)
        {
            if (await this.categoryRepository.ExistsNameAsync(category.Name))
            {
                ViewBag.AlreadyExists = "This Category already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/Products/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }

            else if (ModelState.IsValid)
            {
                await this.categoryRepository.CreateAsync(category);

                if (inventoryId == null)
                {
                    return RedirectToAction(nameof(Index), "Choices");
                }
                return RedirectToAction(nameof(Create), "Products", new { inventoryId });
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await this.categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.categoryRepository.UpdateAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.categoryRepository.ExistsAsync(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await this.categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.CategoryId == id.Value))
            {
                return View();
            }

            await this.categoryRepository.DeleteAsync(category);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
