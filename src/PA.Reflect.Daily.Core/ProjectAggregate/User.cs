using System.Security.Principal;
using Ardalis.GuardClauses;
using PA.Reflect.Daily.Core.Interfaces;

namespace PA.Reflect.Daily.Core.ProjectAggregate;
public class User : EntityBase, IAggregateRoot
{
  public string Sub { get; set; } = string.Empty;
  public string Username { get; private set; } = string.Empty;
  public string? ProfilePictureUri { get; set; }

  public User() {}

  public User(string sub, string username, string? profilePictureUri = "")
  {
    Sub = sub;
    Username = username;
    ProfilePictureUri = profilePictureUri;
  }

  private readonly List<Reflection> _reflections = new();
  public IEnumerable<Reflection> Reflections => _reflections.AsReadOnly();

  public void UpdateName(string newName)
  {
    Username = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}
