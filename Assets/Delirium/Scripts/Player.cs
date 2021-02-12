using System;
using Delirium.Exceptions;
using Delirium.Interfaces;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private float pickupReach = 1.5f;

		public Inventory Inventory { get; } = new Inventory();
		private IHighlightable currentHighlightable;

		private Transform cameraTransform;

		private void Awake()
		{
			cameraTransform = GetComponentInChildren<Camera>().transform;
			Inventory.UnlockedRecipes.Add(Resources.Load("RecipeTest0") as CraftingRecipeData);
		}

		private void Start() { EventCollection.Instance.UpdateInventoryEvent?.Invoke(Inventory); }

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab))
			{
				MenuManager.Instance.ToggleMenu<InventoryMenu>();
				MenuManager.Instance.ToggleMenu<GeneralHudMenu>();
			}

			if (MenuManager.Instance.IsAnyOpen) { return; }

			RaycastCheck();
		}

		private void RaycastCheck()
		{
			if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupReach))
			{
				currentHighlightable?.EndHighlight();
				currentHighlightable = null;
				return;
			}

			var highlightable = hit.transform.gameObject.GetComponent<IHighlightable>();

			if (highlightable != null)
			{
				highlightable.Highlight();
				currentHighlightable = highlightable;
			}
			else
			{
				currentHighlightable?.EndHighlight();
				currentHighlightable = null;
			}

			if (!Input.GetMouseButtonUp(0)) { return; }

			var inventoryItem = hit.transform.gameObject.GetComponent<InventoryWorldItem>();

			if (inventoryItem == null) { return; }

			try
			{
				Inventory.AddItem(inventoryItem.Data);
				MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup($"Picked up {inventoryItem.Data.Name}", PopupMenu.PopupLevel.Info);
				Destroy(inventoryItem.gameObject);
			}
			catch (AddingInventoryItemFailed e) { MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup(e.Message, PopupMenu.PopupLevel.Waring); }
		}
	}
}