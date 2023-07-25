using Ardalis.Specification;

namespace PA.Reflect.Daily.Core.Interfaces;

public interface IRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
  T Add(T entity);
  IEnumerable<T> AddRange(IEnumerable<T> entities);
  void Update(T entity);
  void UpdateRange(IEnumerable<T> entities);
  void Delete(T entity);
  void DeleteRange(IEnumerable<T> entities);
}
