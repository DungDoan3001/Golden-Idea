using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Data.Context;
using Web.Api.Data.Repository;

namespace Web.Api.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> GetBaseRepo<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type) == false)
            {
                var repositoryInstance = new GenericRepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
