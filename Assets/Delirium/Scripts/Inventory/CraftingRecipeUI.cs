using UnityEngine;
using UnityEngine.EventSystems;

namespace Delirium
{
	public class CraftingRecipeUI : MonoBehaviour, IPointerDownHandler
	{
		private CraftingRecipeData data;
		private Inventory holdingInventory;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!holdingInventory.CanBeCrafted(data)) { return; }

			holdingInventory.CraftItem(data);
		}

		public void Setup(CraftingRecipeData data, Inventory holdingInventory)
		{
			this.data = data;
			this.holdingInventory = holdingInventory;

			GameObject disableLayer = transform.Find("DisableLayer").gameObject;
			disableLayer.SetActive(!holdingInventory.CanBeCrafted(data));
		}
	}
}