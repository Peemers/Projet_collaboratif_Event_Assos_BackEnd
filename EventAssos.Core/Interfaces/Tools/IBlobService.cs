namespace EventAssos.Core.Interfaces.Tools;

public interface IBlobService
{
  Task UploadFileAsync(string containerName, string fileName, Stream content);
}