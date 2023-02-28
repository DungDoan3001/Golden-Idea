using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Services.UploadFileService
{
    public class UploadFileService : IUploadFileService
    {
        private readonly DropboxClient _dropboxClient;
        private readonly string token = "sl.BZoXUrF5ugELfJjtWRUhl6EuK2BftNoTO3kk6e1DAybwqSzPSXQ4-vXbmp4jvIkpY6fGsMP32zkzU9cGl1bocEulWRPv6Q4bEMhLNPcIde6s2PuUStqiDPfLe18E7aYXrMRiqtmNJ92n";

        public UploadFileService()
        {
            _dropboxClient = new DropboxClient(token);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string url;
            using (var stream = file.OpenReadStream())
            {
                var dropboxPath = "/GoldenIdea/" + file.FileName;
                var uploaded = await _dropboxClient.Files
                    .UploadAsync(path: dropboxPath, WriteMode.Overwrite.Instance, body: stream);

                var link = await _dropboxClient.Sharing.ListSharedLinksAsync(dropboxPath);
                if (link.Links.Count == 0)
                {
                    var result =
                           await _dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync(dropboxPath);
                    url = result.Url;
                }
                else
                {
                    url = link.Links[0].Url;
                }
            }
            return url;
        }
    }
}
