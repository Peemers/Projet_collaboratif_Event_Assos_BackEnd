using EventAssos.Core.Interfaces.Tools;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace EventAssos.Infrastructure.AzureTools;

public class BlobStorageService(IConfiguration config) : IBlobService
{
  private readonly BlobServiceClient _blobServiceClient = new (config["AzureBlobStorage"]);
  public async Task UploadFileAsync(string containerName, string fileName, Stream content)
  {
    var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
    await containerClient.CreateIfNotExistsAsync();
    var blocCLient = containerClient.GetBlobClient(fileName);
    await blocCLient.UploadAsync(content, true);
  }
}