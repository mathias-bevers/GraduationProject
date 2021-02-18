using Delirium.Exceptions;
using Delirium.Interfaces;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Player))]
	public class PickupHandler : MonoBehaviour
	{
		[SerializeField] private float pickupReach = 1.5f;
		private IHighlightable currentHighlightable;

		private Player player;
		private Transform cameraTransform;

		public void Start()
		{
			player = GetComponent<Player>();
			cameraTransform = GetComponentInChildren<Camera>().transform;
		}

		private void Update()
		{
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

			if (highlightable == null)
			{
				currentHighlightable?.EndHighlight();
				currentHighlightable = null;
				return;
			}

			currentHighlightable = highlightable;
			highlightable.Highlight();

			if (!Input.GetMouseButtonDown(0)) { return; }

			if (highlightable.GetType() == typeof(InventoryWorldItem))
			{
				var inventoryItem = highlightable as InventoryWorldItem;

				try
				{
					player.Inventory.AddItem(inventoryItem.Data);
					MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup($"Picked up {inventoryItem.Data.Name}", PopupMenu.PopupLevel.Info);
					Destroy(inventoryItem.gameObject);
					currentHighlightable = null;
				}
				catch (AddingInventoryItemFailed e) { MenuManager.Instance.GetMenu<PopupMenu>()?.ShowPopup(e.Message, PopupMenu.PopupLevel.Waring); }
			}

			if (highlightable.GetType() == typeof(WorldCraftingRecipe))
			{
				var craftingRecipe = highlightable as WorldCraftingRecipe;
				
				player.Inventory.AddCraftingRecipe(craftingRecipe.Data);
				Destroy(craftingRecipe.gameObject);
				currentHighlightable = null;
			}
		}
	}
}