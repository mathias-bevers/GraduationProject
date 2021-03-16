using UnityEngine;

namespace Delirium
{
	[CreateAssetMenu(fileName = "Inventory item", menuName = "Delirium/Inventory item", order = 0)]
	public class InventoryItemData : ScriptableObject
	{
		[SerializeField] private new string name;
		[SerializeField] private Sprite sprite;
		[SerializeField] private GameObject worldItem;
		[SerializeField] private bool canHold;
		
		public string Name => name;
		public Sprite Sprite => sprite;
		public GameObject WorldItem => worldItem;
		public bool CanHold => canHold;

		//TODO: Add more data fields
	}
}