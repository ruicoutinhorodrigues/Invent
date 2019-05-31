using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        private readonly DataContext context;

        public SupplierRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
