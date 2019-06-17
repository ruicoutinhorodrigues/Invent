using System.Collections.Generic;
using System.Linq;
using Invent.Web.Data.Entities;
using Invent.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Invent.Web.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
