using Delirium.Lore;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Delirium.Events
{
	#region Event classes
	
	/// <summary>
	///     This event is invoked in the <see cref="Sanity" /> class when the sanity value has changed, it has the sanity class as parameter.
	/// </summary>
	public class SanityChangedEvent : UnityEvent<Sanity> { }

	/// <summary>
	///     This event is invoked in the <see cref="InventoryWorldItem" /> or <see cref="WorldLoreScroll" /> class when the mouse hovers over them, it has a scriptable object as parameter.
	/// </summary>
	public class ItemHoverEvent : UnityEvent<ScriptableObject> { }

	/// <summary>
	///     This event is invoked in the <see cref="Torch" /> class when the torch is held and thus losing durability.
	/// </summary>
	public class TorchDecayEvent : UnityEvent<float> { }

	/// <summary>
	///     This event is invoked when a class need to show a popup, it needs a string which is the message that will be displayed
	///     and a <see cref="PopupMenu.PopupLevel" /> which determines the color of the notification.
	/// </summary>
	public class OpenPopupEvent : UnityEvent<string, PopupMenu.PopupLevel> { }

	/// <summary>
	///     This event is invoked in the <see cref="PickupHandler" /> class when the player has found a lore scroll, it has a <see cref="LoreScrollData" /> and a <see cref="Player" /> as parameter.
	/// </summary>
	public class LoreScrollFoundEvent : UnityEvent<LoreScrollData, Player> { }

	/// <summary>
	///     This event is invoked in the <see cref="ZoneHandler" /> class when a gameobject has entered one of the interaction zones.
	///     It has a <see cref="ZoneHandler.InteractionZone" /> as a parameter.
	/// </summary>
	public class EnteredInteractionZoneEvent : UnityEvent<ZoneHandler.InteractionZone> { }
	#endregion

	/// <summary>
	///     This class keeps track of all of the global events.
	///		<para>Made by: Mathias Bevers</para>
	/// </summary>
	public class EventCollection : Singleton<EventCollection>
	{
		public SanityChangedEvent SanityChangedEvent { get; } = new SanityChangedEvent();
		public ItemHoverEvent ItemHoverEvent { get; } = new ItemHoverEvent();
		public UnityEvent DisableInteractTextEvent { get; } = new UnityEvent();
		public TorchDecayEvent TorchDecayEvent { get; } = new TorchDecayEvent();
		public OpenPopupEvent OpenPopupEvent { get; } = new OpenPopupEvent();
		public LoreScrollFoundEvent LoreScrollFoundEvent { get; } = new LoreScrollFoundEvent();
		public EnteredInteractionZoneEvent EnteredInteractionZoneEvent { get; } = new EnteredInteractionZoneEvent();
	}
}