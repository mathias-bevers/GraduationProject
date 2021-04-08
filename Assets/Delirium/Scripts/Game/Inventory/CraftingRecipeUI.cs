using Delirium.Events;
using Delirium.Exceptions;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Delirium
{
	/// <summary>
	///     This class is used to Initialize with the right variables and tries to craft the item set with initialization when it is clicked.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class CraftingRecipeUI : MonoBehaviour, IPointerDownHandler
	{
		private CraftingRecipeData data;
		private Inventory holdingInventory;

		public void OnPointerDown(PointerEventData eventData)
		{
			try
			{
				holdingInventory.CraftItem(data);
				MenuManager.Instance.GetMenu<InventoryMenu>().UpdateUI(holdingInventory);
			}
			catch (CraftingFailedException exception) { EventCollection.Instance.OpenPopupEvent.Invoke(exception.Message, PopupMenu.PopupLevel.Error); }
		}

		/// <summary>
		///     Set the variables to the right values.
		/// </summary>
		/// <param name="data">The crafting recipe that has to be crafted when clicked.</param>
		/// <param name="holdingInventory">The <see cref="Inventory" /> that needs to be checked if the result can be crafted and if so, added to.</param>
		public void Initialize(CraftingRecipeData data, Inventory holdingInventory)
		{
			this.data = data;
			this.holdingInventory = holdingInventory;
		}
	}
}