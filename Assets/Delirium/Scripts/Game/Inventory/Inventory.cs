﻿using System.Collections.Generic;
using System.Linq;
using Delirium.Events;
using Delirium.Exceptions;
using Delirium.Tools;

namespace Delirium
{
	public class Inventory
	{
		public Dictionary<InventoryItemData, int> Items { get; } = new Dictionary<InventoryItemData, int>();
		public List<CraftingRecipeData> UnlockedRecipes { get; } = new List<CraftingRecipeData>();

		public void AddItem(InventoryItemData item)
		{
			if (!Items.ContainsKey(item))
			{
				Items.Add(item, 1);
				EventCollection.Instance.UpdateInventoryEvent?.Invoke(this);
				return;
			}

			if (Items[item] >= 10) { throw new AddingInventoryItemFailed($"Too many items of {item.Name} in inventory"); }

			Items[item]++;
			EventCollection.Instance.UpdateInventoryEvent?.Invoke(this);
		}

		public void RemoveItems(InventoryItemData item, int amount = 1)
		{
			if (!Items.ContainsKey(item)) { throw new RemovingInventoryItemFailed($"{item.Name} is not found in inventory"); }

			if (Items[item] < amount) { throw new RemovingInventoryItemFailed($"Not enough of {item.Name} in inventory"); }

			Items[item] -= amount;
			EventCollection.Instance.UpdateInventoryEvent?.Invoke(this);
		}

		public void CraftItem(CraftingRecipeData craftingRecipe)
		{
			try
			{
				foreach (CraftingRecipePair crp in craftingRecipe.NeededItems) { RemoveItems(crp.InventoryItemData, crp.Amount); }

				AddItem(craftingRecipe.Result);
			}
			catch (RemovingInventoryItemFailed exception) { MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup(exception.Message, PopupMenu.PopupLevel.Error); }
		}

		public bool CanBeCrafted(CraftingRecipeData craftingRecipe) => craftingRecipe.NeededItems.All(crp => Items.ContainsKey(crp.InventoryItemData) && Items[crp.InventoryItemData] >= crp.Amount);


		public void AddCraftingRecipe(CraftingRecipeData craftingRecipe)
		{
			if (UnlockedRecipes.Contains(craftingRecipe))
			{
				MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup("You already unlocked this crafting recipe", PopupMenu.PopupLevel.Waring);
				return;
			}
			
			UnlockedRecipes.Add(craftingRecipe);
			EventCollection.Instance.UpdateInventoryEvent?.Invoke(this);
		}
	}
}