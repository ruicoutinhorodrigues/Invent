using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class StatusController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IStatusRepository statusRepository;

        public StatusController(IProductRepository productRepository, IStatusRepository statusRepository)
        {
            this.productRepository = productRepository;
            this.statusRepository = statusRepository;
        }

        // GET: Status
        public IActionResult Index()
        {
            return View(this.statusRepository.GetAll());
        }

        // GET: Status/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Status status, string inventoryId, string returnUrl)
        {
            if (await this.statusRepository.ExistsNameAsync(status.Name))
            {
                ViewBag.AlreadyExists = "This Status already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/Products/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }
            else if (ModelState.IsValid)
            {
                await this.statusRepository.CreateAsync(status);

                if (inventoryId == null)
                {
                    return RedirectToAction(nameof(Index), "Choices");
                }

                if (returnUrl.Split("/")[2] == "Edit")
                {
                    var id = returnUrl.Split("/")[3];

                    return RedirectToAction(nameof(Edit), "Products", new { id });
                }

                return RedirectToAction(nameof(Create), "Products", new { inventoryId });
            }
            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);

            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Status status)
        {
            if (id != status.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.statusRepository.UpdateAsync(status);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.statusRepository.ExistsAsync(status.Id))
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
            return View(status);
        }

        // GET: Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await this.statusRepository.GetByIdAsync(id.Value);

            if (status == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.StatusId == id.Value))
            {
                return View();
            }

            await this.statusRepository.DeleteAsync(status);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
