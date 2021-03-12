﻿using Delirium.Tools;
using UnityEngine.Events;

namespace Delirium.Events
{
	public class UpdateInventoryEvent : UnityEvent<Inventory> { }

	public class HealthChangedEvent : UnityEvent<Health> { }

	public class SanityChangedEvent : UnityEvent<Sanity> { }

	public class EventCollection : Singleton<EventCollection>
	{
		public UpdateInventoryEvent UpdateInventoryEvent { get; } = new UpdateInventoryEvent();
		public HealthChangedEvent HealthChangedEvent { get; } = new HealthChangedEvent();
		public SanityChangedEvent SanityChangedEvent { get; } = new SanityChangedEvent();
	}
}