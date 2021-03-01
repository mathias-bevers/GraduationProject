using Delirium.Interfaces;
using UnityEngine;

namespace Delirium
{
	public class WorldCraftingRecipe : MonoBehaviour, IHighlightable
	{
		[SerializeField] private CraftingRecipeData data;
		public CraftingRecipeData Data => data;

		public void Highlight() { }

		public void EndHighlight() { }
	}
}