using System;
using UnityEngine;

namespace Delirium
{
	[Serializable]
	public class CraftingRecipePair
	{
		[SerializeField] private InventoryItemData inventoryItemData;
		[SerializeField] private int amount;

		public InventoryItemData InventoryItemData => inventoryItemData;
		public int Amount => amount;
	}
}