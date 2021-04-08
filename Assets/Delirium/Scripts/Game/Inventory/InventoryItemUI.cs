using Delirium.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Delirium
{
	/// <summary>
	///     This class is used to Initialize with the right variables and when clicked if it is the Datura Potion heal <see cref="Health" /> and <see cref="Sanity" />.
	///		<para>Made by: Mathias Bevers</para>
	/// </summary>
	public class InventoryItemUI : MonoBehaviour, IPointerDownHandler
	{
		private const string DATURA_NAME = "Datura Potion";

		private InventoryItemData data;
		private Player player;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (data.Name != DATURA_NAME) { return; }

			player.Inventory.RemoveItems(data);
			player.Health.Heal(30);
			player.Sanity.RegenSanity(20);

			MenuManager.Instance.GetMenu<InventoryMenu>().UpdateUI(player.Inventory);
		}

		public void Initialize(Player player, InventoryItemData data)
		{
			this.player = player;
			this.data = data;
		}
	}
}