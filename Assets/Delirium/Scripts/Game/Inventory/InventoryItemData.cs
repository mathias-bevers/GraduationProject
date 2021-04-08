using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This scriptable object holds the information,name and sprite, of an inventory item.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	[CreateAssetMenu(fileName = "Inventory item", menuName = "Delirium/Inventory item", order = 0)]
	public class InventoryItemData : ScriptableObject
	{
		[SerializeField] private new string name;
		[SerializeField] private Sprite sprite;

		/// <summary>
		///     Gets the name of the inventory item, which is set in the editor.
		/// </summary>
		public string Name => name;

		/// <summary>
		///     Gets the sprite of the inventory item, which is set in the editor.
		/// </summary>
		public Sprite Sprite => sprite;
	}
}