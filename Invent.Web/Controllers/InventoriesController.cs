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

namespace Invent.Web.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly ApplicationDbContext _user_context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmailSender _emailSender;

        public InventoriesController(DataContext context, ApplicationDbContext user_context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
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

            if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                //return View(await _context.Inventory.Where(i => i.UserName == user.UserName).ToListAsync());

                var thisManagerInventory = await _context.Inventory.Where(i => i.UserName == user.UserName).FirstOrDefaultAsync();

                return RedirectToAction(nameof(Index), "Products", new { inventoryId = thisManagerInventory.Id});
            }
            else
            {
                return View(await _context.Inventory.ToListAsync());
            }

            
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["UsersId"] = new SelectList(_user_context.Users, "Id", "UserName");

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
                    _context.Inventory.Add(
                                        new Inventory
                                        {
                                            Name = inventoryViewModel.Name,
                                            UserName = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId).ToString(),
                                            DateOfCreation = DateTime.Now,
                                            ImageUrl = path
                                        });
                    await _context.SaveChangesAsync();

                    await AddToRole("Manager", thisUser.Email);

                    await _emailSender.SendEmailAsync(thisUser.Email, "Inventory Manager",
                        $"Welcome to our team.<br/><br/>" +
                        $"You are now the proud manager of the {inventoryViewModel.Name} inventory. We expect great things from you! :)<br/><br/>" +
                        $"Please remember: \"With great power comes great responsibility\" (spiderman)<br/><br/>" +
                        "Best regards, AMS");



                    return RedirectToAction(nameof(Index));
                }             
            }

            ViewData["UsersId"] = new SelectList(_user_context.Users, "Id", "UserName");

            return View(inventoryViewModel);
        }

        public async Task AddToRole(string role, string email)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (!User.IsInRole(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            var thisInventory = new InventoryViewModel()
            {
                Id = inventory.Id,
                Name = inventory.Name,
                UserName = inventory.UserName,
                DateOfCreation = inventory.DateOfCreation,
                ImageUrl = inventory.ImageUrl
            };

            ViewData["UsersId"] = new SelectList(_user_context.Users, "Id", "UserName");

            return View(thisInventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UsersId,ImageFile,DateOfCreation")] InventoryViewModel inventoryViewModel)
        {
            if (id != inventoryViewModel.Id)
            {
                return NotFound();
            }

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

                    var updatedInventory = new Inventory()
                    {

                        Name = inventoryViewModel.Name,
                        UserName = _user_context.Users.FirstOrDefault(u => u.Id == inventoryViewModel.UsersId).ToString(),
                        DateOfCreation = DateTime.Now,
                        ImageUrl = path
                    };

                    try
                    {
                        _context.Update(updatedInventory);
                        await _context.SaveChangesAsync();

                        await AddToRole("Manager", thisUser.Email);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InventoryExists(updatedInventory.Id))
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
            return View(inventoryViewModel);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.Id == id);
        }
    }
}
