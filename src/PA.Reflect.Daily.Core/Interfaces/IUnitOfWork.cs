using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Core.Interfaces;
public interface IUnitOfWork : IDisposable
{
  IRepository<User> UserRepository { get; }
  IRepository<Reflection> ReflectionRepository { get; }
  IFileStorageRepository FileStorageRepository { get; }
  void BeginTransaction();
  void Commit();
  void Rollback();
  Task<int> SaveChangesAsync();
  Task SaveAndCommitAsync();
}
