using UnityEngine;
using UnityEngine.EventSystems;

namespace Delirium
{
	public class InventoryItemUI : MonoBehaviour, IPointerDownHandler
	{
		private const string DATURA_NAME = "Datura Potion";
		private Player player;

		private InventoryItemData data;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (data.Name != DATURA_NAME) { return; }

			player.Inventory.RemoveItems(data, 1);
			player.Health.Heal(30);
			player.Sanity.RegenSanity(20);
		}

		public void Initialize(Player player, InventoryItemData data)
		{
			this.player = player;
			this.data = data;
		}
	}
}