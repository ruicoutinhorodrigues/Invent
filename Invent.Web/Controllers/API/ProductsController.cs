using Invent.Web.Data;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Invent.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            //return Ok(this.productRepository.GetAllWithInventory());
            return Ok(this.productRepository.GetAll());
        }
    }
}
