using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		public Inventory Inventory { get; } = new Inventory();
		public Health Health { get; } = new Health(100);

		private void Awake() { Inventory.UnlockedRecipes.Add(Resources.Load("RecipeTest0") as CraftingRecipeData); }

		private void Start()
		{
			EventCollection.Instance.UpdateInventoryEvent?.Invoke(Inventory);
			EventCollection.Instance.HealthChangedEvent?.Invoke(Health);
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Tab))
			{
				MenuManager.Instance.ToggleMenu<InventoryMenu>();
				MenuManager.Instance.ToggleMenu<GeneralHudMenu>();
			}

			if (Input.GetKeyUp(KeyCode.Escape))
			{
				MenuManager.Instance.CloseMenu<GeneralHudMenu>();
				MenuManager.Instance.OpenMenu<PauseMenu>();
			}
		}
	}
}