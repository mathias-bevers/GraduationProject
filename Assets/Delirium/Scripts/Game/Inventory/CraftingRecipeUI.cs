using Delirium.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Delirium
{
	public class CraftingRecipeUI : MonoBehaviour, IPointerDownHandler
	{
		private CraftingRecipeData data;
		private Inventory holdingInventory;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!holdingInventory.CanBeCrafted(data))
			{
				MenuManager.Instance.GetMenu<PopupMenu>().ShowPopup($"You don't have enough items to craft {data.Result.Name}", PopupMenu.PopupLevel.Error);
				return;
			}

			holdingInventory.CraftItem(data);
		}

		public void Setup(CraftingRecipeData data, Inventory holdingInventory)
		{
			this.data = data;
			this.holdingInventory = holdingInventory;

			GetComponent<Button>().enabled = !holdingInventory.CanBeCrafted(data);
		}
	}
}