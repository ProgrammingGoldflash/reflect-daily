namespace PA.Reflect.Daily.Core.Interfaces;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<IEntityBase> entitiesWithEvents);
}
