using UnityEngine;

namespace Delirium
{
	[CreateAssetMenu(fileName = "Inventory item", menuName = "Delirium/Inventory item", order = 0)]
	public class InventoryItemData : ScriptableObject
	{
		[SerializeField] private new string name;
		[SerializeField] private Sprite sprite;
		[SerializeField] private GameObject worldItem;
		
		public string Name => name;
		public Sprite Sprite => sprite;
		public GameObject WorldItem => worldItem;

		//TODO: Add more data fields
	}
}