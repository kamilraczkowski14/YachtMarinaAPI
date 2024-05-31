using Azure.Storage;
using Azure.Storage.Blobs;
using YachtMarinaAPI.Dtos;

namespace YachtMarinaAPI.Services
{
    public class FileService
    {

        private readonly BlobContainerClient _filesContainer;

        public FileService(IConfiguration configuration)
        {
            var credential = new StorageSharedKeyCredential(configuration["AzureStorage:StorageAccount"],
                configuration["AzureStorage:Key"]);
            var blobUri = $"https://{configuration["AzureStorage:StorageAccount"]}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("files");
        }


        public async Task<BlobResponseDto> Upload(IFormFile blob)
        {
            BlobResponseDto response = new BlobResponseDto();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            if (await client.ExistsAsync())
            {
               // response.Status = $"Plik o nazwie {blob.FileName} już istnieje w magazynie.";
                //response.Error = true;
                return null;
            }

            await using (Stream? data = blob.OpenReadStream())
            {
                client.Upload(data);
            }

            response.Status = $"Plik {blob.FileName} został przesłany pomyślnie";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
        public async Task<BlobDto?> Download(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;
                string contentType = content.Value.Details.ContentType;

                return new BlobDto
                {
                    Content = blobContent,
                    Name = name,
                    ContentType = contentType
                };

            }

            return null;
        }


        public async Task<BlobResponseDto> Delete(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            await file.DeleteAsync();

            return new BlobResponseDto
            {
                Error = false,
                Status = $"Plik: {blobFilename} został usunięty"
            };
        }

        public async Task<List<BlobDto>> List()
        {
            List<BlobDto> files = new List<BlobDto>();

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            return files;

        }
    }
}
