using System;
using Delirium.Lore;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This class should be initialized first, it loads all of the objects in the Resources folder.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class ResourceManager : Singleton<ResourceManager>
	{
		/// <summary>
		///     Get the following enemy prefab, which is loaded from the resources folder.
		/// </summary>
		public GameObject FollowingEnemyPrefab { get; private set; }

		private CraftingRecipeData[] craftingRecipes;

		private LoreScrollData[] loreScrolls;

		protected override void Awake()
		{
			base.Awake();

			FollowingEnemyPrefab = Resources.Load<GameObject>("FollowingEnemy");

			craftingRecipes = Resources.LoadAll<CraftingRecipeData>("CraftingRecipes");
			loreScrolls = Resources.LoadAll<LoreScrollData>("LoreScrolls");
		}

		/// <summary>Returns the recipe for the given result name. Returns null when the recipe could not be found.</summary>
		/// <param name="resultName">The result name of the crafting recipe that is requested.</param>
		/// <returns>Crafting recipe with the given result name.</returns>
		public CraftingRecipeData GetRecipeByResultName(string resultName) => Array.Find(craftingRecipes, recipe => recipe.Result.Name == resultName);

		/// <summary>Returns the data of the lore scroll with the given number. Returns null when the lore scroll data could not be found.</summary>
		/// <param name="number">The number of the lore scroll data that is requested</param>
		/// <returns>The date of the lore scroll with the given number</returns>
		public LoreScrollData GetLoreScrollByNumber(int number) => Array.Find(loreScrolls, scroll => scroll.Number == number);
	}
}