using MediatR;

namespace PA.Reflect.Daily.Core;

public abstract class DomainEventBase : INotification
{
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
