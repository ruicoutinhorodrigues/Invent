using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly DataContext context;

        public LocationRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
