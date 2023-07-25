using Ardalis.Specification;

namespace PA.Reflect.Daily.Core.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
