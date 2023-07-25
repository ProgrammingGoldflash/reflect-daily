namespace PA.Reflect.Daily.Core.Interfaces;

public interface IFileStorageRepository
{
  Task StoreFileAsync(string relativePath, byte[] content);
  Task<byte[]> GetFileContentAsync(string relativePath);
}
