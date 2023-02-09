using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Context;

namespace Web.Api.Data.Queries
{
    public class BaseQuery<T> where T : class
    {
        protected DbSet<T> dbSet;

        public BaseQuery() { }

        public BaseQuery(AppDbContext dbContext)
        {
            dbSet = dbContext.Set<T>();
        }
    }
}
