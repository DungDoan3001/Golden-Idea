using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Entities;

namespace Web.Api.Services.FileService
{
    public interface IFileService
    {
        Task<IEnumerable<File>> AddRangeAsync(IEnumerable<File> entities);
    }
}