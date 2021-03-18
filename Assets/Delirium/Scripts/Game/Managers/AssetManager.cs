using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class AssetManager : Singleton<AssetManager>
	{
		public CraftingRecipeData StartRecipe { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			StartRecipe = Resources.Load<CraftingRecipeData>("Torch_CraftingRecipe");
		}
	}
}