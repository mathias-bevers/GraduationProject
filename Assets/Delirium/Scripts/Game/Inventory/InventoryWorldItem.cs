using Delirium.AbstractClasses;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class InventoryWorldItem : Pickupable
	{
		[SerializeField] private InventoryItemData data;
		public InventoryItemData Data => data;

		private void OnMouseOver()
		{
			if (!InReach) { return; }

			EventCollection.Instance.ItemHoverEvent?.Invoke(Data);
		}
	}
}