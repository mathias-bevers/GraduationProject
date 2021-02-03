using System.Collections.Generic;
using Delirium.Exceptions;

namespace Delirium
{
	public class Inventory
	{
		public Dictionary<InventoryItemTemplate, int> Items { get; } = new Dictionary<InventoryItemTemplate, int>();

		public void AddItem(InventoryItemTemplate item)
		{
			if (!Items.ContainsKey(item))
			{
				Items.Add(item, 1);
				return;
			}

			if (Items[item] >= 10)
			{
				throw new AddingItemFailed($"Too many items of {item.name} in inventory");
			}

			Items[item]++;
		}

		public void RemoveItems(InventoryItemTemplate item) { }
	}
}