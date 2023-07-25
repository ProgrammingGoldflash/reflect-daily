using Ardalis.GuardClauses;
using PA.Reflect.Daily.Core.Interfaces;

namespace PA.Reflect.Daily.Infrastructure.Data;
public class FileStorageRepository : IFileStorageRepository
{
  private readonly string _baseDirectory;

  public FileStorageRepository(string baseDirectory)
  {
    _baseDirectory = baseDirectory;
  }

  public async Task StoreFileAsync(string relativePath, byte[] content)
  {
    string fullPath = Path.Combine(_baseDirectory, relativePath);

    string? directory = Path.GetDirectoryName(fullPath);
    Guard.Against.NullOrEmpty(directory);

    if (!Directory.Exists(directory))
    {
      Directory.CreateDirectory(directory);
    }

    await File.WriteAllBytesAsync(fullPath, content);
  }

  public async Task<byte[]> GetFileContentAsync(string relativePath)
  {
    string fullPath = Path.Combine(_baseDirectory, relativePath);

    if (!File.Exists(fullPath))
    {
      throw new FileNotFoundException($"The file '{fullPath}' does not exist.");
    }

    return await File.ReadAllBytesAsync(fullPath);
  }
}
