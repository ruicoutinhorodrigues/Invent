using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        private readonly DataContext context;

        public StatusRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
