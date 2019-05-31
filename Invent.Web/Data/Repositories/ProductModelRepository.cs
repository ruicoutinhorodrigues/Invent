using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class ProductModelRepository : GenericRepository<ProductModel>, IProductModelRepository
    {
        private readonly DataContext context;

        public ProductModelRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
