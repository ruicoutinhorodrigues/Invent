using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class LocationsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ILocationRepository locationRepository;

        public LocationsController(IProductRepository productRepository, ILocationRepository locationRepository)
        {
            this.productRepository = productRepository;
            this.locationRepository = locationRepository;
        }

        // GET: Locations
        public IActionResult Index()
        {
            return View(this.locationRepository.GetAll());
        }

        // GET: Locations/Create
        public IActionResult Create(string returnUrl, string inventoryId)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.InventoryId = inventoryId;

            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Location location, string inventoryId, string returnUrl)
        {

            if (await this.locationRepository.ExistsNameAsync(location.Name))
            {
                ViewBag.AlreadyExists = "This Location already exists in your system.";

                if (inventoryId != null)
                {
                    ViewBag.ReturnUrl = "/Products/Create?inventoryId=" + inventoryId;
                    ViewBag.InventoryId = inventoryId;
                }
            }

            else if (ModelState.IsValid)
            {
                await this.locationRepository.CreateAsync(location);

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

            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await this.locationRepository.GetByIdAsync(id.Value);

            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.locationRepository.UpdateAsync(location);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.locationRepository.ExistsAsync(location.Id))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await this.locationRepository.GetByIdAsync(id.Value);

            if (location == null)
            {
                return NotFound();
            }

            if (await this.productRepository.GetAll().AnyAsync(p => p.LocationId == id.Value))
            {
                return View();
            }

            await this.locationRepository.DeleteAsync(location);

            return RedirectToAction(nameof(Index), "Choices");
        }
    }
}
