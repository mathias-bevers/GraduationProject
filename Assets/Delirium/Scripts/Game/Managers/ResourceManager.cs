using System.Collections.Generic;
using System.Linq;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class ResourceManager : Singleton<ResourceManager>
	{
		public GameObject FollowingEnemyPrefab { get; private set; }

		private List<CraftingRecipeData> craftingRecipes;

		protected override void Awake()
		{
			base.Awake();

			FollowingEnemyPrefab = Resources.Load<GameObject>("FollowingEnemy");

			craftingRecipes = Resources.LoadAll<CraftingRecipeData>("CraftingRecipes").ToList();
		}

		/// <summary>Returns the recipe for the given result name. Returns null when the recipe could not be found.</summary>
		/// <param name="resultName">The result name of the crafting recipe that is requested.</param>
		/// <returns>Crafting recipe with the given result name.</returns>
		public CraftingRecipeData GetRecipeByResultName(string resultName) => craftingRecipes.Find(recipe => recipe.Result.Name == resultName);
	}
}