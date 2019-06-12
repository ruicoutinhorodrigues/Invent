using Invent.Web.Data.Entities;
using System.Linq;

namespace Invent.Web.Data.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //IQueryable GetAllTeste();
    }
}
