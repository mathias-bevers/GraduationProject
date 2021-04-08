using System;
using System.Linq;
using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Player : MonoBehaviour
	{
		public bool IsAlive { get; set; } = true;
		public Inventory Inventory { get; private set; }
		public Health Health { get; } = new Health(100);
		public Sanity Sanity { get; private set; }
		
		private Transform cameraTransform;

		private void Awake()
		{
			cameraTransform = GetComponentInChildren<Camera>().transform;

			Inventory = new Inventory(this);

			Sanity = GetComponent<Sanity>();
			Sanity.RegisterPlayer(this);
		}

		private void Start()
		{
			ToggleHeldItems(1);
		}

		private void Update()
		{
			if (!IsAlive) { return; }

			if (Input.GetKeyUp(KeyCode.Tab))
			{
				if (MenuManager.Instance.GetMenu<InventoryMenu>().IsOpen)
				{
					MenuManager.Instance.CloseMenu<InventoryMenu>();
					MenuManager.Instance.OpenMenu<PlayerHUDMenu>();
					return;
				}
				
				MenuManager.Instance.CloseMenu<PlayerHUDMenu>();
				var inventoryMenu = MenuManager.Instance.OpenMenu<InventoryMenu>();
				inventoryMenu.UpdateUI(Inventory);
			}

			if (Input.GetKeyUp(KeyCode.Escape))
			{
				if (MenuManager.Instance.GetMenu<PauseMenu>().IsOpen)
				{
					MenuManager.Instance.CloseMenu<PauseMenu>();
					MenuManager.Instance.OpenMenu<PlayerHUDMenu>();
					return;
				}
				
				MenuManager.Instance.CloseMenu<PlayerHUDMenu>();
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
				MenuManager.Instance.GetMenu<PlayerHUDMenu>()?.TorchDurabilityBar.SetActive(false);
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

					if (torchData == null) { return; }

					if (!Inventory.Items.ContainsKey(torchData) || Inventory.Items[torchData] == 0) { return; }

					DisableTools();
					MenuManager.Instance.GetMenu<PlayerHUDMenu>()?.TorchDurabilityBar.SetActive(true);
					cameraTransform.GetChild(1).gameObject.SetActive(true);
					break;

				case 3:
					InventoryItemData spearData = Inventory.Items.FirstOrDefault(x => x.Key.Name == "Spear").Key;

					if (spearData == null) { return; }

					if (!Inventory.Items.ContainsKey(spearData)) { return; }

					DisableTools();
					cameraTransform.GetChild(2).gameObject.SetActive(true);
					break;

				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}