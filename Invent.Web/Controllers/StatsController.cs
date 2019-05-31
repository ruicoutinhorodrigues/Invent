using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invent.Web.Data;
using Invent.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invent.Web.Controllers
{
    public class StatsController : Controller
    {

        private readonly DataContext _context;

        readonly StatViewModel lstModel = new StatViewModel();

        public StatsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int inventoryId)
        {
            var listProductsFromInventory = _context.Products.Where(p => p.InventoryId == inventoryId);

            if (listProductsFromInventory.Count() == 0)
            {
                return RedirectToAction("Index", "Products", new { inventoryId });
            }
            else
            {
                foreach (var product in listProductsFromInventory)
                {

                    // Locations chart

                    if (lstModel.VLocations == null)
                    {
                        lstModel.VLocations = new List<VLocation>();
                    }

                    if (lstModel.VLocations.Any(l => l.DimensionId == product.LocationId))
                    {
                        lstModel.VLocations.FirstOrDefault(l => l.DimensionId == product.LocationId).Quantity++;
                    }
                    else
                    {
                        lstModel.VLocations.Add(new VLocation()
                        {
                            DimensionId = product.LocationId,
                            Dimension = _context.Locations.FirstOrDefault(l => l.Id == product.LocationId).Name,
                            Quantity = 1
                        });
                    }


                    // Categories chart

                    if (lstModel.VCategories == null)
                    {
                        lstModel.VCategories = new List<VCategory>();
                    }

                    if (lstModel.VCategories.Any(l => l.DimensionId == product.CategoryId))
                    {
                        lstModel.VCategories.FirstOrDefault(l => l.DimensionId == product.CategoryId).Quantity++;
                    }
                    else
                    {
                        lstModel.VCategories.Add(new VCategory()
                        {
                            DimensionId = product.CategoryId,
                            Dimension = _context.Categories.FirstOrDefault(l => l.Id == product.CategoryId).Name,
                            Quantity = 1
                        });
                    }


                    // Suppliers chart

                    if (lstModel.VSuppliers == null)
                    {
                        lstModel.VSuppliers = new List<VSupplier>();
                    }

                    if (lstModel.VSuppliers.Any(l => l.DimensionId == product.SupplierId))
                    {
                        lstModel.VSuppliers.FirstOrDefault(l => l.DimensionId == product.SupplierId).Quantity++;
                    }
                    else
                    {
                        lstModel.VSuppliers.Add(new VSupplier()
                        {
                            DimensionId = product.SupplierId,
                            Dimension = _context.Suppliers.FirstOrDefault(l => l.Id == product.SupplierId).Name,
                            Quantity = 1
                        });
                    }


                    // Status chart

                    if (lstModel.VStatus == null)
                    {
                        lstModel.VStatus = new List<VStatus>();
                    }

                    if (lstModel.VStatus.Any(l => l.DimensionId == product.StatusId))
                    {
                        lstModel.VStatus.FirstOrDefault(l => l.DimensionId == product.StatusId).Quantity++;
                    }
                    else
                    {
                        lstModel.VStatus.Add(new VStatus()
                        {
                            DimensionId = product.StatusId,
                            Dimension = _context.Status.FirstOrDefault(l => l.Id == product.StatusId).Name,
                            Quantity = 1
                        });
                    }

                    //Last 6 months entries
                    if (lstModel.ValueMonth == null)
                    {
                        lstModel.ValueMonth = new List<ValueMonth>();
                    }

                    for (int i = -5; i <= 0; i++)
                    {
                        if (product.EntryDate.Month == DateTime.Now.AddMonths(i).Month)
                        {
                            if (!lstModel.ValueMonth.Any(p => p.Month.Month == product.EntryDate.Month))
                            {
                                lstModel.ValueMonth.Add(new ValueMonth()
                                {
                                    TotalValue = product.Value,
                                    Month = product.EntryDate
                                });
                            }
                            else
                            {
                                lstModel.ValueMonth.FirstOrDefault(p => p.Month.Month == product.EntryDate.Month).TotalValue += product.Value;
                            }
                        }
                    }
                }

                ViewBag.InventoryId = inventoryId;

                return View(lstModel);
            }
        }
    }
}