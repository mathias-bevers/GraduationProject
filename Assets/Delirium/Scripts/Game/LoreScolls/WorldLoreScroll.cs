using Delirium.AbstractClasses;
using Delirium.Events;
using UnityEngine;

namespace Delirium.Lore
{
	public class WorldLoreScroll : Pickupable
	{
		[SerializeField] private LoreScrollData data;
		public LoreScrollData Data => data;

		private void OnMouseOver()
		{
			if (!InReach) { return; }

			EventCollection.Instance.ItemHoverEvent?.Invoke(data);
		}
	}
}