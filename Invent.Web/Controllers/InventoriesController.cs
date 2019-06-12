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
using Microsoft.AspNetCore.Identity.UI.Services;
using Invent.Web.Data.Repositories;

namespace Invent.Web.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IProductRepository productRepository;

        private readonly ApplicationDbContext _user_context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmailSender _emailSender;

        public InventoriesController(IInventoryRepository inventoryRepository, IProductRepository productRepository, ApplicationDbContext user_context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.inventoryRepository = inventoryRepository;
            this.productRepository = productRepository;

            _user_context = user_context;

            _userManager = userManager;
            _roleManager = roleManager;

            _emailSender = emailSender;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            ViewBag.CurrentUser = user.UserName;

            if (await _userManager.IsInRoleAsync(user, "Manager") && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                //return View(await _context.Inventory.Where(i => i.UserName == user.UserName).ToListAsync());

                //var thisManagerInventory = await _context.Inventory.Where(i => i.UserName == user.UserName).FirstOrDefaultAsync();
                var thisManagerInventory = await this.inventoryRepository.GetAll().Where(i => i.UserName == user.UserName).FirstOrDefaultAsync();

                return RedirectToAction(nameof(Index), "Products", new { inventoryId = thisManagerInventory.Id});
            }
            else
            {
                return View(this.inventoryRepository.GetAll().Where(i => i.NotAvailable != true));
            }

            
        }

        // GET: Inventories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = this.inventoryRepository.GetByIdAsync(id.Value);

            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            var availableUsers = _user_context.Users.ToList();

            //Each manager can have only one inventory. The admin can have as many as he wants.
            foreach (var manager in this.inventoryRepository.GetAll())
            {
                foreach (var user in _user_context.Users)
                {
                    if (manager.UserName == user.UserName && user.UserName != "rui.coutinho.rodrigues@gmail.com")
                    {
                        availableUsers.Remove(user);
                    }
                }
            }

            ViewData["UsersId"] = new SelectList(availableUsers, "Id", "UserName");

            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryViewModel inventoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (inventoryViewModel.ImageFile != null && inventoryViewModel.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Inventories",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await inventoryViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Inventories/{file}";
                }



                var thisUser = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId);

                if (thisUser != null)
                {
                    await this.inventoryRepository.CreateAsync(
                                        new Inventory
                                        {
                                            Name = inventoryViewModel.Name,
                                            UserName = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId).ToString(),
                                            DateOfCreation = DateTime.Now,
                                            ImageUrl = path
                                        });

                    if (!await _userManager.IsInRoleAsync(thisUser, "Admin"))
                    {
                        await AddToRole("Manager", thisUser.Email);

                        await _emailSender.SendEmailAsync(thisUser.Email, "Inventory Manager",
                            $"Welcome to our team.<br/><br/>" +
                            $"You are now the proud manager of the {inventoryViewModel.Name} inventory. We expect great things from you! :)<br/><br/>" +
                            $"Please remember: \"With great power comes great responsibility\" (spiderman)<br/><br/>" +
                            "Best regards, AMS");

                        
                    }

                    return RedirectToAction(nameof(Index));
                }             
            }

            var availableUsers = _user_context.Users.ToList();

            //Each manager can have only one inventory. The admin can have as many as he wants.
            foreach (var manager in this.inventoryRepository.GetAll())
            {
                foreach (var user in _user_context.Users)
                {
                    if (manager.UserName == user.UserName && user.UserName != "rui.coutinho.rodrigues@gmail.com")
                    {
                        availableUsers.Remove(user);
                    }
                }
            }
            ViewData["UsersId"] = new SelectList(availableUsers, "Id", "UserName");

            return View(inventoryViewModel);
        }

        public async Task AddToRole(string role, string email)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await _userManager.FindByEmailAsync(email);

            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task RemoveFromRole(string role, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await _userManager.RemoveFromRoleAsync(user, role);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return View("NotFound");
            }

            var inventory = await this.inventoryRepository.GetByIdAsync(id.Value);

            if (inventory == null)
            {
                //return NotFound();
                return View("NotFound");
            }

            var thisInventory = new InventoryViewModel()
            {
                Id = inventory.Id,
                Name = inventory.Name,
                UserName = inventory.UserName,
                DateOfCreation = inventory.DateOfCreation,
                ImageUrl = inventory.ImageUrl
            };

            var availableUsers = _user_context.Users.ToList();

            //Each manager can have only one inventory. The admin can have as many as he wants.
            foreach (var manager in this.inventoryRepository.GetAll())
            {
                foreach (var user in _user_context.Users)
                {
                    if (manager.UserName == user.UserName && manager.UserName != "rui.coutinho.rodrigues@gmail.com")
                    {
                        availableUsers.Remove(user);
                    }
                }
            }
            ViewData["UsersId"] = new SelectList(availableUsers, "Id", "UserName");

            return View(thisInventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,UsersId,ImageUrl,ImageFile,DateOfCreation")] InventoryViewModel inventoryViewModel)
        {
            if (id != inventoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var path = inventoryViewModel.ImageUrl;

                if (inventoryViewModel.ImageFile != null && inventoryViewModel.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Inventories",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await inventoryViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Inventories/{file}";
                }

                var thisUser = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId);

                if (thisUser != null)
                {
                    var oldUser = _user_context.Users.FirstOrDefault(u => u.UserName == inventoryViewModel.UserName);

                    if (!await _userManager.IsInRoleAsync(oldUser, "Admin"))
                    {
                        await RemoveFromRole("Manager", oldUser.Email);

                        await _emailSender.SendEmailAsync(oldUser.Email, "Inventory Manager",
                           $"It's time to say goodbye. :(<br/><br/>" +
                           $"You are no longer the manager of the {inventoryViewModel.Name} inventory. We were very fortunate to have you in our team.<br/><br/>" +
                           "Best regards, AMS");
                    }

                    var updatedInventory = new Inventory()
                    {
                        Id = inventoryViewModel.Id,
                        Name = inventoryViewModel.Name,
                        UserName = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId).ToString(),
                        DateOfCreation = DateTime.Now,
                        ImageUrl = path
                    };

                    try
                    {
                        await this.inventoryRepository.UpdateAsync(updatedInventory);

                        if (!await _userManager.IsInRoleAsync(thisUser, "Admin"))
                        {
                            await AddToRole("Manager", thisUser.Email);

                            await _emailSender.SendEmailAsync(thisUser.Email, "Inventory Manager",
                            $"Welcome to our team.<br/><br/>" +
                            $"You are now the proud manager of the {inventoryViewModel.Name} inventory. We expect great things from you! :)<br/><br/>" +
                            $"Please remember: \"With great power comes great responsibility\" (spiderman)<br/><br/>" +
                            "Best regards, AMS");
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await this.inventoryRepository.ExistsAsync(updatedInventory.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }     
                else
                {
                    var updatedInventory = new Inventory()
                    {
                        Id = inventoryViewModel.Id,
                        Name = inventoryViewModel.Name,
                        UserName = inventoryViewModel.UserName,
                        DateOfCreation = inventoryViewModel.DateOfCreation,
                        ImageUrl = path
                    };

                    try
                    {
                        await this.inventoryRepository.UpdateAsync(updatedInventory);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await this.inventoryRepository.ExistsAsync(updatedInventory.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            var availableUsers = _user_context.Users.ToList();

            //Each manager can have only one inventory. The admin can have as many as he wants.
            foreach (var manager in this.inventoryRepository.GetAll())
            {
                foreach (var user in _user_context.Users)
                {
                    if (manager.UserName == user.UserName && user.UserName != "rui.coutinho.rodrigues@gmail.com")
                    {
                        availableUsers.Remove(user);
                    }
                }
            }
            ViewData["UsersId"] = new SelectList(availableUsers, "Id", "UserName");

            return View(inventoryViewModel);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await this.inventoryRepository.GetByIdAsync(id.Value);

            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var inventory = await this.inventoryRepository.GetByIdAsync(id.Value);

            if (this.productRepository.GetAll().Any(p => p.InventoryId == id))
            {
                inventory.NotAvailable = true;
                await this.inventoryRepository.UpdateAsync(inventory);
            }
            else
            {
                await this.inventoryRepository.DeleteAsync(inventory);
            }

            var oldUser = _user_context.Users.FirstOrDefault(u => u.UserName == inventory.UserName);

            if (!await _userManager.IsInRoleAsync(oldUser, "Admin"))
            {
                await RemoveFromRole("Manager", oldUser.Email);

                await _emailSender.SendEmailAsync(oldUser.Email, "Inventory Manager",
                   $"It's time to say goodbye. :(<br/><br/>" +
                   $"You are no longer the manager of the {inventory.Name} inventory. We were very fortunate to have you in our team.<br/><br/>" +
                   "Best regards, AMS");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
