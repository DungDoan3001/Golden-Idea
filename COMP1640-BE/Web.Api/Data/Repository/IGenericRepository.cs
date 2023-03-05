using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Api.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> All();
        bool Delete(Guid id);
        bool DeleteRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetById(Guid id);
        T Update(T entity);
    }
}