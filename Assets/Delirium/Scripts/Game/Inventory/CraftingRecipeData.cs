using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This ScriptableObject keeps track of the data that is needed for a Crafting Recipe. It holds the result and all of the items that are needed to craft the result.
	/// </summary>
	[CreateAssetMenu(fileName = "Crafting recipe", menuName = "Delirium/Crafting Recipe", order = 0)]
	public class CraftingRecipeData : ScriptableObject
	{
		[SerializeField] private InventoryItemData result;
		[SerializeField] private CraftingRecipePair[] neededItems;

		/// <summary>
		///     The <see cref="InventoryItemData" /> that is added to the <see cref="Inventory" /> when crafted.
		/// </summary>
		public InventoryItemData Result => result;

		/// <summary>
		///     Array of all the items and their amounts stored in an <see cref="CraftingRecipePair" /> needed to craft the result.
		/// </summary>
		public CraftingRecipePair[] NeededItems => neededItems;
	}
}