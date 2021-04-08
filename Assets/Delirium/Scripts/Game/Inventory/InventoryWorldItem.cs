using Delirium.AbstractClasses;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This class is used to differentiate the Pickupable so the handlers, <see cref="PlayerHUDMenu"/> and <see cref="PickupHandler"/>, can process the inventory item differently.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class InventoryWorldItem : Pickupable
	{
		[SerializeField] private InventoryItemData data;
		
		/// <summary>
		/// Get all of the inventory item's information, which is set in the inventory.
		/// </summary>
		public InventoryItemData Data => data;

		private void OnMouseOver()
		{
			if (!InReach) { return; }

			EventCollection.Instance.ItemHoverEvent?.Invoke(Data);
		}
	}
}