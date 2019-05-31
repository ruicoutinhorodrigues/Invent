using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly DataContext context;

        public InventoryRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
