using Delirium.Exceptions;
using UnityEngine;

namespace Delirium
{
	public class InventoryItem : MonoBehaviour
	{
		[SerializeField] private InventoryItemTemplate data;
		public InventoryItemTemplate Data => data;
	}
}