using UnityEngine;

namespace Delirium
{
	[CreateAssetMenu(fileName = "Inventory item", menuName = "Delirium/Inventory item", order = 0)]
	public class InventoryItemData : ScriptableObject
	{
		[SerializeField] private new string name;
		[SerializeField] private Sprite sprite;
		
		public string Name => name;
		public Sprite Sprite => sprite;

		//TODO: Add more data fields
	}
}