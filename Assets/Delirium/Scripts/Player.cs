using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private float pickupReach = 1.5f;

		public Inventory Inventory { get; } = new Inventory();

		private Transform cameraTransform;

		private void Awake() { cameraTransform = GetComponentInChildren<Camera>().transform; }

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab)) { MenuManager.Instance.ToggleMenu<InventoryMenu>(); }

			if (Input.GetMouseButtonUp(0))
			{
				Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.green, 0.5f);
				if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickupReach)) { return; }
				
				var inventoryItem = hit.transform.gameObject.GetComponent<InventoryItem>();

				if (inventoryItem == null) { return; }

				Inventory.AddItem(inventoryItem.Data);
				Destroy(inventoryItem.gameObject);
			}
		}
	}
}