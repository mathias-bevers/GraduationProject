using Delirium.Events;
using Delirium.Tools;

public class EventCollection : Singleton<EventCollection>
{
    public UpdateInventoryEvent UpdateInventoryEvent { get; } = new UpdateInventoryEvent();
}