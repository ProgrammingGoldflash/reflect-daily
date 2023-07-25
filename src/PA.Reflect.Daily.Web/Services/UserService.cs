using System.Security.Claims;
using Ardalis.GuardClauses;
using PA.Reflect.Daily.Core.Aggregates;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.Core.Interfaces;
using PA.Reflect.Daily.Web.Interfaces;

namespace PA.Reflect.Daily.Web.Services;

public class UserService : IUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IUnitOfWork _unitOfWork;

  public UserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
  {
    _httpContextAccessor = httpContextAccessor;
    _unitOfWork = unitOfWork;
  }

  public async Task<User> GetCurrentUserAsync()
  {
    Guard.Against.Null(_httpContextAccessor.HttpContext);

    var subjectClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    Guard.Against.NullOrEmpty(subjectClaim);

    var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(new UserBySubClaimSpec(subjectClaim));

    Guard.Against.Null(user);

    return user;
  }
}
