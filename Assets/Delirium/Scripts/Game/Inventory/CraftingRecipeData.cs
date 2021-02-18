using UnityEngine;

namespace Delirium
{
	[CreateAssetMenu(fileName = "Crafting recipe", menuName = "Delirium/Crafting Recipe", order = 0)]
	public class CraftingRecipeData : ScriptableObject
	{
		[SerializeField] private InventoryItemData result;
		[SerializeField] private CraftingRecipePair[] neededItems;

		public InventoryItemData Result => result;
		public CraftingRecipePair[] NeededItems => neededItems;
	}
}