using System.Collections.Generic;
using Delirium.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Delirium
{
	/// <summary>
	///     This class is used to display the correct inventory items to the UI elements.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class InventoryMenu : Menu
	{
		private const float RECIPE_UI_HEIGHT = 192.0f;

		[Header("UI element prefabs"), SerializeField]
		private GameObject inventoryItemUIPrefab;

		[SerializeField] private GameObject recipeUIPrefab;

		[Header("Prefab instance locations"), SerializeField]
		private Transform itemGrid;

		[SerializeField] private RectTransform recipeGrid;

		protected override void Start()
		{
			Opened += OnOpened;

			base.Start();
		}

		public override bool CanBeClosed() => true;
		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;

		public void OnOpened()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		/// <summary>
		/// Upa
		/// </summary>
		/// <param name="inventory"></param>
		public void UpdateUI(Inventory inventory)
		{
			foreach (Transform child in itemGrid.transform) { Destroy(child.gameObject); }

			foreach (KeyValuePair<InventoryItemData, int> kvp in inventory.Items)
			{
				GameObject inventoryItemUI = Instantiate(inventoryItemUIPrefab, itemGrid.transform, true);
				inventoryItemUI.transform.localScale = Vector3.one;

				inventoryItemUI.name = kvp.Key.Name;
				inventoryItemUI.transform.Find("AmountText").GetComponent<TextMeshProUGUI>().SetText(kvp.Value.ToString());
				inventoryItemUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(kvp.Key.Name);
				inventoryItemUI.GetComponentInChildren<Image>().sprite = kvp.Key.Sprite;

				inventoryItemUI.GetComponent<InventoryItemUI>()?.Initialize(inventory.ParentPlayer, kvp.Key);
			}

			foreach (Transform child in recipeGrid) { Destroy(child.gameObject); }

			float recipeGridSizeY = RECIPE_UI_HEIGHT + (inventory.UnlockedRecipes.Count - 1) * (RECIPE_UI_HEIGHT * 1.1f);
			recipeGrid.sizeDelta = new Vector2(recipeGrid.sizeDelta.x, recipeGridSizeY);

			foreach (CraftingRecipeData unlockedRecipe in inventory.UnlockedRecipes)
			{
				GameObject craftingRecipeObject = Instantiate(recipeUIPrefab, recipeGrid, true);
				craftingRecipeObject.transform.localScale = Vector3.one;
				craftingRecipeObject.name = unlockedRecipe.Result.Name;

				craftingRecipeObject.GetComponent<CraftingRecipeUI>().Initialize(unlockedRecipe, inventory);

				craftingRecipeObject.transform.Find("Title").GetComponent<TextMeshProUGUI>().SetText(unlockedRecipe.Result.Name);
				craftingRecipeObject.transform.Find("Sprite").GetComponent<Image>().sprite = unlockedRecipe.Result.Sprite;
				craftingRecipeObject.transform.Find("Needed Items").GetComponent<TextMeshProUGUI>().SetText(GenerateNeededItemsString(unlockedRecipe));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="craftingRecipe"></param>
		/// <returns></returns>
		private static string GenerateNeededItemsString(CraftingRecipeData craftingRecipe)
		{
			var generatedString = $"{craftingRecipe.NeededItems[0].Amount} {craftingRecipe.NeededItems[0].InventoryItemData.Name}";

			for (var i = 1; i < craftingRecipe.NeededItems.Length; i++) { generatedString += $" + {craftingRecipe.NeededItems[i].Amount} {craftingRecipe.NeededItems[i].InventoryItemData.Name}"; }

			return generatedString;
		}
	}
}