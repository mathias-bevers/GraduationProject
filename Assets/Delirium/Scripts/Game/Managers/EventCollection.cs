using Delirium.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Delirium.Events
{
	#region Event classes
	public class UpdateInventoryEvent : UnityEvent<Inventory> { }

	public class SanityChangedEvent : UnityEvent<Sanity> { }

	public class ItemHoverEvent : UnityEvent<ScriptableObject> { }
	#endregion

	public class EventCollection : Singleton<EventCollection>
	{
		public UpdateInventoryEvent UpdateInventoryEvent { get; } = new UpdateInventoryEvent();
		public SanityChangedEvent SanityChangedEvent { get; } = new SanityChangedEvent();
		public ItemHoverEvent ItemHoverEvent { get; } = new ItemHoverEvent();
		public UnityEvent ItemHoverExitEvent { get; } = new UnityEvent();
	}
}