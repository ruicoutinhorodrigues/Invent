using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invent.Web.Data.Repositories;
using Invent.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
        

        public ChoicesController(
                                 ICategoryRepository categoryRepository, 
                                 ILocationRepository locationRepository,
                                 IManufacturerRepository manufacturerRepository, 
                                 IStatusRepository statusRepository,
                                 IProductModelRepository productmodelRepository, 
                                 ISupplierRepository supplierRepository
                                )
        {
            
            this.categoryRepository = categoryRepository;
            this.locationRepository = locationRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.statusRepository = statusRepository;
            this.productmodelRepository = productmodelRepository;
            this.supplierRepository = supplierRepository;
        }



        public IActionResult Index()
        {

            var choices = new ChoicesViewModel()
            {
                Categories = this.categoryRepository.GetAll().ToList(),
                Locations = this.locationRepository.GetAll().ToList(),
                Manufacturers = this.manufacturerRepository.GetAll().ToList(),
                Status = this.statusRepository.GetAll().ToList(),
                ProductModels = this.productmodelRepository.GetAll().ToList(),
                Suppliers = this.supplierRepository.GetAll().ToList()
            };


            return View(choices);
        }
    }
}