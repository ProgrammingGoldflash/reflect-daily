using System.Text;
using Ardalis.GuardClauses;
using PA.Reflect.Daily.Core.Aggregates;
using PA.Reflect.Daily.Core.Interfaces;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.ServiceLayer.Interfaces;

namespace PA.Reflect.Daily.ServiceLayer.Services;
public class ReflectionService : IReflectionService
{
  private readonly IUnitOfWork _unitOfWork;

  public ReflectionService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public async Task<Reflection> AddReflectionAsync(Reflection reflection)
  {
    _unitOfWork.BeginTransaction();

    Reflection entity = new();

    try
    {
      entity = _unitOfWork.ReflectionRepository.Add(reflection);

      if (reflection.Notes != null && reflection.Notes.Any())
      {
        foreach (var note in reflection.Notes)
        {
          await _unitOfWork.FileStorageRepository.StoreFileAsync(note.Filepath, Encoding.UTF8.GetBytes(note.MarkdownValue));
        }
      }
    }
    catch (Exception)
    {
      _unitOfWork.Rollback();
    }
    finally { await _unitOfWork.SaveAndCommitAsync(); }

    Guard.Against.Null(entity);

    return entity;
  }

  public async Task DeleteReflectionAsync(Reflection reflection)
  {
    _unitOfWork.ReflectionRepository.Delete(reflection);

    await _unitOfWork.SaveChangesAsync();
  }

  public async Task<Reflection?> GetReflectionAsync(int UserId, DateOnly date)
  {
    var entity = await _unitOfWork.ReflectionRepository.FirstOrDefaultAsync(new ReflectionByDate(UserId, date));

    if (entity?.Notes != null && entity.Notes.Any())
    {
      foreach (var note in entity.Notes)
      {
        var content = await _unitOfWork.FileStorageRepository.GetFileContentAsync(note.Filepath);
        note.MarkdownValue = Encoding.UTF8.GetString(content);
      }
    }

    return entity;
  }

  public async Task<IEnumerable<Reflection>> GetReflectionsAsync(int UserId)
  {
    var reflections = await _unitOfWork.ReflectionRepository.ListAsync(new ReflectionsByUserId(UserId));

    return reflections;
  }

  public async Task UpdateReflectionAsync(Reflection reflection)
  {
    _unitOfWork.BeginTransaction();

    try
    {
      _unitOfWork.ReflectionRepository.Update(reflection);

      if (reflection.Notes != null && reflection.Notes.Any())
      {
        foreach (var note in reflection.Notes)
        {
          await _unitOfWork.FileStorageRepository.StoreFileAsync(note.Filepath, Encoding.UTF8.GetBytes(note.MarkdownValue));
        }
      }
    }
    catch (Exception)
    {
      _unitOfWork.Rollback();
    }
    finally { _unitOfWork.Commit(); }
  }
}
