using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Web.Interfaces;

public interface IUserService
{
  Task<User> GetCurrentUserAsync();
}
