using System.Threading.Tasks;
using Web.Api.Data.Repository;

namespace Web.Api.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
        void Detach<T>(T entity) where T : class;
        void Dispose();
        IGenericRepository<T> GetBaseRepo<T>() where T : class;
    }
}