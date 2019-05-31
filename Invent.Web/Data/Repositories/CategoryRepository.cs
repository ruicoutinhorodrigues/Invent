using Invent.Web.Data.Entities;

namespace Invent.Web.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext context;

        public CategoryRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
