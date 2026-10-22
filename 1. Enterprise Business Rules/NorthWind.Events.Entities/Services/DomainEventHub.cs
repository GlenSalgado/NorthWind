namespace NorthWind.Events.Entities.Services;

using NorthWind.Events.Entities.Interfaces;

internal class DomainEventHub<EventType>(
    IEnumerable<IDomainEventHandler<EventType>> eventHandlers)
    : IDomainEventHub<EventType>
    where EventType : IDomainEvent
{
    public async Task Raise(EventType eventTypeInstance)
    {
        foreach (var Handler in eventHandlers)
        {
            await Handler.Handle(eventTypeInstance);
        }
    }
}