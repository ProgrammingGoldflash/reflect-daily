using System.ComponentModel.DataAnnotations.Schema;

namespace PA.Reflect.Daily.Core;

public interface IEntityBase {
  public IEnumerable<DomainEventBase> DomainEvents { get; }
  internal void ClearDomainEvents();
}

public abstract class EntityBase : IEntityBase
{
  public int Id { get; set; }

  private List<DomainEventBase> _domainEvents = new ();
  [NotMapped]
  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  public void ClearDomainEvents() => _domainEvents.Clear();
}
