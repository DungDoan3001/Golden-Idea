using static Web.Api.Services.ZipFile.ZipFileService;
using System;
using System.Threading.Tasks;

namespace Web.Api.Services.ZipFile
{
    public interface IZipFileService
    {
        Task<FileZip> ZipIdeasOfTopicExpired(Guid topicId);
        Task<FileZip> ZipDashboardData();
    }
}