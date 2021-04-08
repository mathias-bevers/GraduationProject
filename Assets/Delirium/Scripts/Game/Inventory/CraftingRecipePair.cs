using System;
using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This is a wrapper class to serialize a "Dictionary", this is not supported in Unity. This wrapper can hold a <see cref="InventoryItemData" /> as key and a int as value.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	[Serializable]
	public class CraftingRecipePair
	{
		[SerializeField] private InventoryItemData inventoryItemData;
		[SerializeField] private int amount;

		/// <summary>
		///     Key of this wrapper.
		/// </summary>
		public InventoryItemData InventoryItemData => inventoryItemData;

		/// <summary>
		///     Value of this wrapper.
		/// </summary>
		public int Amount => amount;
	}
}