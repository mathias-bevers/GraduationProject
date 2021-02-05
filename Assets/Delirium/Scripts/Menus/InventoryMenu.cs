using System.Collections.Generic;
using UnityEngine.UI;
using Delirium.Tools;
using UnityEngine;
using TMPro;

namespace Delirium
{
	public class InventoryMenu : Menu
	{
		[SerializeField] private GameObject uiItemPrefab;

		public override bool CanBeOpened() => true;
		public override bool CanBeClosed() => true;

		public override void Open()
		{
			base.Open();
			
			//TODO: make this generic.
			DrawInventory(GameManager.Instance.Player.Inventory);
		}

		private void DrawInventory(Inventory inventory)
		{
			Transform grid = GetComponentInChildren<GridLayoutGroup>().transform;

			foreach (Transform child in grid) { Destroy(child.gameObject); }

			foreach (KeyValuePair<InventoryItemData, int> kvp in inventory.Items)
			{
				GameObject inventoryObject = Instantiate(uiItemPrefab, grid, true);

				inventoryObject.name = kvp.Key.Name;
				inventoryObject.GetComponentInChildren<TextMeshProUGUI>().SetText(kvp.Value.ToString());
				inventoryObject.GetComponentInChildren<Image>().sprite = kvp.Key.Sprite;
			}
		}
	}
}