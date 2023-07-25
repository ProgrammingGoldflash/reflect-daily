using Ardalis.Specification;
using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Core.Aggregates;

public class UserBySubClaimSpec : Specification<User>, ISingleResultSpecification
{
  public UserBySubClaimSpec(string subClaim)
  {
    Query
        .Where(user => user.Sub == subClaim);
  }
}
