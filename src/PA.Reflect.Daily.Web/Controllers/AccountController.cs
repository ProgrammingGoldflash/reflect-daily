using System.Security.Claims;
using Ardalis.GuardClauses;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PA.Reflect.Daily.Core.Aggregates;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.Core.Interfaces;

namespace PA.Reflect.Daily.Web.Controllers;
[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
  private const string VALIDATION = "validate";
  private const string VALIDATION_ENDPOINT = $"/account/{VALIDATION}";
  private readonly IUnitOfWork _unitOfWork;

  public AccountController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  [HttpGet("login")]
  public async Task Login()
  {
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        // Indicate here where Auth0 should redirect the user after a login.
        // Note that the resulting absolute Uri must be added to the
        // **Allowed Callback URLs** settings for the app.
        .WithRedirectUri(VALIDATION_ENDPOINT)
        .Build();

    await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
  }

  [Route("signup")]
  public async Task Signup()
  {
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithParameter("screen_hint", "signup")
        .WithRedirectUri(VALIDATION_ENDPOINT)
        .Build();

    await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
  }

  [Authorize]
  [HttpGet(VALIDATION)]
  public async Task<RedirectResult> ValidateAsync()
  {
    Guard.Against.Null(User.Identity);
    Guard.Against.NullOrEmpty(User.Identity.Name);

    var subjectClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    
    Guard.Against.NullOrEmpty(subjectClaim);

    var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(new UserBySubClaimSpec(subjectClaim));

    if(user == null)
      _unitOfWork.UserRepository.Add(new User(subjectClaim, User.Identity.Name) { Id = default });

    await _unitOfWork.SaveChangesAsync();

    return Redirect("/");
  }

  [Authorize]
  [HttpGet("logout")]
  public async Task Logout()
  {
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithParameter("screen_hint", "logout")
        .WithRedirectUri("/")
        .Build();

    await HttpContext.SignOutAsync(authenticationProperties);
  }
}
