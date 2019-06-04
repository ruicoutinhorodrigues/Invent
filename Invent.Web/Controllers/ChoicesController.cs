using Invent.Web.Data;
using Invent.Web.Data.Repositories;
using Invent.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class ChoicesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IProductModelRepository productmodelRepository;
        private readonly ISupplierRepository supplierRepository;

        private readonly ApplicationDbContext user_context;

        private readonly UserManager<IdentityUser> userManager;

        private readonly IInventoryRepository inventoryRepository;

        public ChoicesController(
                                 ICategoryRepository categoryRepository,
                                 ILocationRepository locationRepository,
                                 IManufacturerRepository manufacturerRepository,
                                 IStatusRepository statusRepository,
                                 IProductModelRepository productmodelRepository,
                                 ISupplierRepository supplierRepository,
                                 ApplicationDbContext user_context,
                                 UserManager<IdentityUser> userManager,
                                 IInventoryRepository inventoryRepository
                                )
        {

            this.categoryRepository = categoryRepository;
            this.locationRepository = locationRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.statusRepository = statusRepository;
            this.productmodelRepository = productmodelRepository;
            this.supplierRepository = supplierRepository;

            this.user_context = user_context;
            this.userManager = userManager;

            this.inventoryRepository = inventoryRepository;
        }



        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var choices = new ChoicesViewModel()
            {
                Categories = this.categoryRepository.GetAll().ToList(),
                Locations = this.locationRepository.GetAll().ToList(),
                Manufacturers = this.manufacturerRepository.GetAll().ToList(),
                Status = this.statusRepository.GetAll().ToList(),
                ProductModels = this.productmodelRepository.GetAll().ToList(),
                Suppliers = this.supplierRepository.GetAll().ToList(),

                Users = this.user_context.Users.ToList()
            };


            return View(choices);
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (this.inventoryRepository.GetAll().Any(i => i.UserName == user.UserName))
            {
                ViewBag.UserId = userId;

                return View();
            }
            else
            {
                await userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> DeleteConfirmed(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            await userManager.DeleteAsync(user);

            await inventoryRepository.DeleteAsync(this.inventoryRepository.GetAll().FirstOrDefault(i => i.UserName == user.UserName));

            return RedirectToAction(nameof(Index));
        }
    }
}