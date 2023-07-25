using Ardalis.Specification;
using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Core.Aggregates;
public class ReflectionsByUserId : Specification<Reflection>
{
  public ReflectionsByUserId(int userId)
  {
    Query
      .Where(c => c.UserId == userId);
  }
}

public class ReflectionByDate : Specification<Reflection>
{
  public ReflectionByDate(int userId, DateOnly date)
  {
    Query
      .Where(c => c.Date == date && c.UserId == userId)
      .Include(r => r.Notes);
  }
}
