using Delirium.Exceptions;
using UnityEngine;

namespace Delirium
{
	public class InventoryItem : MonoBehaviour
	{
		[SerializeField] private InventoryItemTemplate data;

		private void OnTriggerEnter(Collider other)
		{
			var player = other.GetComponent<Player>();
			
			if (player == null) { return; }

			try
			{
				player.Inventory.AddItem(data);
				Destroy(gameObject);
			}
			catch (AddingItemFailed e)
			{
				Debug.Log(e);
				throw;
			}
		}
	}
}