using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
    {
        private readonly DataContext context;

        public ManufacturerRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
