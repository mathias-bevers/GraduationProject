using Delirium.Interfaces;
using UnityEngine;

namespace Delirium
{
	public class InventoryWorldItem : MonoBehaviour, IHighlightable
	{
		[SerializeField] private InventoryItemData data;
		public InventoryItemData Data => data;

		public void Highlight() { }

		public void EndHighlight() { }
	}
}