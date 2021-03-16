using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private GameObject spear;
		[SerializeField] private GameObject torch;

		public Inventory Inventory { get; } = new Inventory();
		public Health Health { get; } = new Health(100);
		public Sanity Sanity { get; private set; }

		private int scrollIndex;

		private void Awake()
		{
			Inventory.UnlockedRecipes.Add(AssetManager.Instance.StartRecipe);
			Sanity = GetComponent<Sanity>();
		}

		private void Start() { EventCollection.Instance.UpdateInventoryEvent?.Invoke(Inventory); }

		private void Update()
		{
			scrollIndex += (int) (Input.GetAxis("Mouse ScrollWheel") * 10);

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