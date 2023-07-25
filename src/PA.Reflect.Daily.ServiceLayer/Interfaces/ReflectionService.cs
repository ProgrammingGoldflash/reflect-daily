using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.ServiceLayer.Interfaces;

public interface IReflectionService
{
  Task<Reflection> AddReflectionAsync(Reflection reflection);
  Task UpdateReflectionAsync(Reflection reflection);
  Task DeleteReflectionAsync(Reflection reflection);
  Task<Reflection?> GetReflectionAsync(int UserId, DateOnly date);
  Task<IEnumerable<Reflection>> GetReflectionsAsync(int UserId);
}
