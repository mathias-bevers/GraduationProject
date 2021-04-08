using Delirium.AbstractClasses;
using Delirium.Events;
using UnityEngine;

namespace Delirium.Lore
{
	/// <summary>
	///     This class is used to differentiate the Pickupable so the handlers, <see cref="PlayerHUDMenu" /> and <see cref="PickupHandler" />, can process the lore scroll differently.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class WorldLoreScroll : Pickupable
	{
		[SerializeField] private LoreScrollData data;

		/// <summary>
		///     Get all of the lore's data. Which is set in the editor.
		/// </summary>
		public LoreScrollData Data => data;

		private void OnMouseOver()
		{
			if (!InReach) { return; }

			EventCollection.Instance.ItemHoverEvent?.Invoke(data);
		}
	}
}