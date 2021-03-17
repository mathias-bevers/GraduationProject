using System;
using System.Linq;
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

		private Transform cameraTransform;

		private void Awake()
		{
			Inventory.UnlockedRecipes.Add(AssetManager.Instance.StartRecipe);
			Sanity = GetComponent<Sanity>();
			cameraTransform = GetComponentInChildren<Camera>().transform;
		}

		private void Start() { EventCollection.Instance.UpdateInventoryEvent?.Invoke(Inventory); }

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

			if (Input.GetKeyUp(KeyCode.Alpha1)) { ToggleHeldItems(1); }

			if (Input.GetKeyUp(KeyCode.Alpha2)) { ToggleHeldItems(2); }

			if (Input.GetKeyUp(KeyCode.Alpha3)) { ToggleHeldItems(3); }
		}

		public void ToggleHeldItems(int index)
		{
			void DisableTools()
			{
				foreach (Transform child in cameraTransform) { child.gameObject.SetActive(false); }
			}

			switch (index)
			{
				case 1:
					DisableTools();
					cameraTransform.GetChild(0).gameObject.SetActive(true);
					break;

				case 2:
					InventoryItemData torchData = Inventory.Items.FirstOrDefault(x => x.Key.Name == "Torch").Key;
					if (!Inventory.Items.ContainsKey(torchData) || Inventory.Items[torchData] == 0) { return; }

					DisableTools();
					cameraTransform.GetChild(1).gameObject.SetActive(true);
					break;

				case 3:
					InventoryItemData spearData = Inventory.Items.FirstOrDefault(x => x.Key.Name == "Spear").Key;
					if (!Inventory.Items.ContainsKey(spearData)) { return; }

					DisableTools();
					cameraTransform.GetChild(2).gameObject.SetActive(true);
					break;

				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}