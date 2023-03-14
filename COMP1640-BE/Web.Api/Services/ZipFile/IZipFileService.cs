using static Web.Api.Services.ZipFile.ZipFileService;
using System;
using System.Threading.Tasks;

namespace Web.Api.Services.ZipFile
{
    public interface IZipFileService
    {
        FileZip GetZippedFile(DateTime deliveryDate);
        Task<FileZip> ZipIdeasOfTopicExpired(Guid topicId);
    }
}