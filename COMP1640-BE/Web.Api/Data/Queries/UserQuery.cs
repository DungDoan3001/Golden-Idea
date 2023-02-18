using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks;
using Web.Api.Data.Context;

namespace Web.Api.Data.Queries
{
    public class UserQuery : BaseQuery<Entities.User>
    {
        public UserQuery(AppDbContext context) : base(context) 
        {
            
        }

        //public async Task<IEnumerable<Entities.User>> GetAll()
        //{
        //    return await dbSet.Include<Entities.UserRole>
        //}
    }
}
