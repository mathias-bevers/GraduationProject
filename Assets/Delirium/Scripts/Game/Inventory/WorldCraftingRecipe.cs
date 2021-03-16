using System;
using Delirium.AbstractClasses;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class WorldCraftingRecipe : Pickupable
	{
		[SerializeField] private CraftingRecipeData data;
		public CraftingRecipeData Data => data;

		private void OnMouseOver()
		{
			if (!InReach) { return; }

			EventCollection.Instance.ItemHoverEvent?.Invoke(Data);
		}
	}
}