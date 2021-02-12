using System.Collections.Generic;
using Delirium.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Delirium
{
	public class InventoryMenu : Menu
	{
		private const float RECIPE_UI_HEIGHT = 192.0f;

		[Header("UI element prefabs"), SerializeField]
		private GameObject inventoryItemUIPrefab;

		[SerializeField] private GameObject recipeUIPrefab;

		[Header("Prefab instance locations"), SerializeField]
		private Transform itemGrid;

		[SerializeField] private RectTransform recipeGrid;

		private void Awake() { EventCollection.Instance.UpdateInventoryEvent.AddListener(OnUpdateInventory); }

		public override bool CanBeClosed() => true;
		public override bool CanBeOpened() => true;

		public override void Open()
		{
			base.Open();

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		private void OnUpdateInventory(Inventory inventory)
		{
			foreach (Transform child in itemGrid.transform) { Destroy(child.gameObject); }

			foreach (KeyValuePair<InventoryItemData, int> kvp in inventory.Items)
			{
				if(kvp.Value <= 0) {continue;}

				GameObject inventoryItemUI = Instantiate(inventoryItemUIPrefab, itemGrid.transform, true);
				inventoryItemUI.transform.localScale = Vector3.one;

				inventoryItemUI.name = kvp.Key.Name;
				inventoryItemUI.GetComponentInChildren<TextMeshProUGUI>().SetText(kvp.Value.ToString());
				inventoryItemUI.GetComponentInChildren<Image>().sprite = kvp.Key.Sprite;
			}

			foreach (Transform child in recipeGrid) { Destroy(child.gameObject); }

			float recipeGridSizeY = RECIPE_UI_HEIGHT + (inventory.UnlockedRecipes.Count - 1) * (RECIPE_UI_HEIGHT * 1.1f);
			recipeGrid.sizeDelta = new Vector2(recipeGrid.sizeDelta.x, recipeGridSizeY);

			foreach (CraftingRecipeData unlockedRecipe in inventory.UnlockedRecipes)
			{
				GameObject craftingRecipeObject = Instantiate(recipeUIPrefab, recipeGrid, true);
				craftingRecipeObject.transform.localScale = Vector3.one;
				craftingRecipeObject.name = unlockedRecipe.Result.Name;

				craftingRecipeObject.GetComponent<CraftingRecipeUI>().Setup(unlockedRecipe, inventory);

				craftingRecipeObject.transform.Find("Title").GetComponent<TextMeshProUGUI>().SetText(unlockedRecipe.Result.Name);
				craftingRecipeObject.transform.Find("Sprite").GetComponent<Image>().sprite = unlockedRecipe.Result.Sprite;
			}
		}
	}
}