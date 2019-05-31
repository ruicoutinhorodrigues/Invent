using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(DataContext context) : base(context) { }
    }
}
