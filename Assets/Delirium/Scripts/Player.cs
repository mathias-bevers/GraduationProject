using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		public Inventory Inventory { get; } = new Inventory();

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab))
			{
				MenuManager.Instance.ToggleMenu<InventoryMenu>();
			}
		}
	}
}