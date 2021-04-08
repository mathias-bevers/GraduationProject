using System.Collections.Generic;
using System.Linq;
using Delirium.Events;
using Delirium.Exceptions;

namespace Delirium
{
	/// <summary>
	///     <para>This class is used to keep track of the items and recipes the player currently has.</para>
	///     <para>
	///         It can add <see cref="AddItem" />, remove <see cref="RemoveItems" /> items, and add crafting recipes <see cref="AddCraftingRecipe" />.
	///         It can craft <see cref="CraftItem" />, and check if the item can be crafted <see cref="CanBeCrafted" />.
	///     </para>
	///     <para>Created by: Mathias Bevers</para>
	/// </summary>
	public class Inventory
	{
		/// <summary>
		///     <para> This class is used to keep track of the items and recipes the player currently has. This constructor is used to set the player which the inventory is attached to.</para>
		///     <para>It can add <see cref="AddItem" />, remove <see cref="RemoveItems" /> items, and add crafting recipes <see cref="AddCraftingRecipe" />.</para>
		///     <para>It can craft <see cref="CraftItem" />, and check if the item can be crafted <see cref="CanBeCrafted" />.</para>
		/// </summary>
		/// <param name="parentPlayer"></param>
		public Inventory(Player parentPlayer) { ParentPlayer = parentPlayer; }

		/// <summary>
		///     All of the items the player currently has and how much of them.
		/// </summary>
		public Dictionary<InventoryItemData, int> Items { get; } = new Dictionary<InventoryItemData, int>();

		/// <summary>
		///     All of the crafting recipes the player has unlocked and thus will be displayed in the UI.
		/// </summary>
		public List<CraftingRecipeData> UnlockedRecipes { get; } = new List<CraftingRecipeData>();

		/// <summary>
		///     The player this inventory is attached to.
		/// </summary>
		public Player ParentPlayer { get; }

		/// <summary>
		///     Try to add an item to the <see cref="Items" /> dictionary. It can only be added if the player is carrying less than 10 items of that type,
		///     otherwise the <see cref="AddingInventoryItemFailed" /> exception is thrown.
		/// </summary>
		/// <param name="item">The item that has to be attempted to add to the inventory</param>
		/// <exception cref="AddingInventoryItemFailed">This exception is thrown when the player is already carrying 10 of the item type.</exception>
		public void AddItem(InventoryItemData item)
		{
			if (!Items.ContainsKey(item))
			{
				Items.Add(item, 1);
				EventCollection.Instance.OpenPopupEvent?.Invoke($"Added {item.Name} to inventory", PopupMenu.PopupLevel.Info);
				return;
			}

			if (Items[item] >= 10) { throw new AddingInventoryItemFailed($"Too many items of {item.Name} in inventory"); }

			Items[item]++;
			EventCollection.Instance.OpenPopupEvent?.Invoke($"Added {item.Name} to inventory", PopupMenu.PopupLevel.Info);
		}

		/// <summary>
		///     Try to remove items from the <see cref="Items" /> dictionary.
		///     It can only be removed when the item type already exists in the dictionary or it has at least the amount that has to be removed.
		///     Otherwise the <see cref="RemovingInventoryItemFailed" /> exception is thrown.
		/// </summary>
		/// <param name="item">The item(s) that have to be attempted to remove from the inventory</param>
		/// <param name="amount">The amount of items that have to be attempted to remove.</param>
		/// <exception cref="RemovingInventoryItemFailed">This exception is thrown when the dictionary doesn't have the item type or its amount is less then the requested remove amount.</exception>
		public void RemoveItems(InventoryItemData item, int amount = 1)
		{
			if (!Items.ContainsKey(item)) { throw new RemovingInventoryItemFailed($"{item.Name} is not found in inventory"); }

			if (Items[item] < amount) { throw new RemovingInventoryItemFailed($"Not enough of {item.Name} in inventory"); }

			Items[item] -= amount;

			if (Items[item] == 0) { Items.Remove(item); }
		}

		/// <summary>
		///     Try to craft an item, determines if it can be crafting using the <see cref="CanBeCrafted" /> method.
		///     If this is not the case the <see cref="CraftingFailedException" /> exception is thrown.
		/// </summary>
		/// <param name="craftingRecipe">The recipe that is attempted to be crafted.</param>
		/// <exception cref="CraftingFailedException">This exception is thrown when the <see cref="CanBeCrafted" /> returns false.</exception>
		public void CraftItem(CraftingRecipeData craftingRecipe)
		{
			if (!CanBeCrafted(craftingRecipe)) { throw new CraftingFailedException($"You don't have enough items to craft {craftingRecipe.Result.Name}"); }

			foreach (CraftingRecipePair crp in craftingRecipe.NeededItems) { RemoveItems(crp.InventoryItemData, crp.Amount); }

			AddItem(craftingRecipe.Result);
		}

		/// <summary>
		///     Try to add a crafting recipe to the <see cref="UnlockedRecipes" /> list,
		///     when the list already contains this crafting recipe the <see cref="AddingCraftingRecipeFailed" /> exception in thrown.
		/// </summary>
		/// <param name="craftingRecipe">The crafting recipe that is attempted to add the the list.</param>
		/// <exception cref="AddingCraftingRecipeFailed">This exception is thrown when the crafting recipe already exists in the <see cref="UnlockedRecipes" /> list.</exception>
		public void AddCraftingRecipe(CraftingRecipeData craftingRecipe)
		{
			if (UnlockedRecipes.Contains(craftingRecipe)) { throw new AddingCraftingRecipeFailed($"You already unlocked the crafting recipe for {craftingRecipe.Result.Name}"); }

			UnlockedRecipes.Add(craftingRecipe);
			EventCollection.Instance.OpenPopupEvent?.Invoke($"You unlocked the {craftingRecipe.Result.Name} crafting recipe", PopupMenu.PopupLevel.Info);
		}

		/// <summary>
		///     Checks is the crafting recipe can be crafted, by checking if the <see cref="Items" /> dictionary contains the needed items and at least the needed amount of these items.
		/// </summary>
		/// <param name="craftingRecipe">The crafting recipe that need to be checked if can be crafted.</param>
		/// <returns>True if all of the items exists and have at least the needed amount.</returns>
		private bool CanBeCrafted(CraftingRecipeData craftingRecipe) => craftingRecipe.NeededItems.All(crp => Items.ContainsKey(crp.InventoryItemData) && Items[crp.InventoryItemData] >= crp.Amount);

		/// <summary>
		///     The Inventory system is based on Scriptable objects, the scriptable object form could in some cases not be accessible.
		///     So this method allows to find the scriptable object form by the item name.
		/// </summary>
		/// <param name="name">The name of the item you want to get.</param>
		/// <returns>The item in its scriptable object form.</returns>
		public InventoryItemData GetItemKeyByName(string name) => Items.FirstOrDefault(kvp => kvp.Key.Name == name).Key;

		/// <summary>
		///     The Inventory system is based on Scriptable objects, the scriptable object form could in some cases not be accessible.
		///     So this method allows to find the amount of an item by the item name.
		/// </summary>
		/// <param name="name">The name of the item you want the amount of.</param>
		/// <returns>The amount of the items found in the inventory as an int.</returns>
		public int GetItemCountByName(string name) => Items.FirstOrDefault(keyValuePair => keyValuePair.Key.Name == name).Value;
	}
}