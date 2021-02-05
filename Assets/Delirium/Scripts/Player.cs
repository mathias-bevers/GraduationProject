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

		private void Awake() { cameraTransform = GetComponentInChildren<Camera>().transform; }

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab)) { MenuManager.Instance.ToggleMenu<InventoryMenu>(); }

			RaycastCheck();
		}

		private void RaycastCheck()
		{
			Debug.DrawRay(cameraTransform.position, cameraTransform.forward * pickupReach, Color.green);
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

			var inventoryItem = hit.transform.gameObject.GetComponent<InventoryItemBehaviour>();

			if (inventoryItem == null) { return; }

			try
			{
				Inventory.AddItem(inventoryItem.Data);
				Destroy(inventoryItem.gameObject);
			}
			catch (AddingItemFailed) { MenuManager.Instance.GetMenu<GeneralHudMenu>()?.ShowPopup($"Reached max capacity of {inventoryItem.Data.Name}", GeneralHudMenu.PopupLevel.Waring); }
		}
	}
}