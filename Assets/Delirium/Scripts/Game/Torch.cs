using System;
using System.Linq;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class Torch : MonoBehaviour
	{
		[SerializeField] private float maxLitTimeInSeconds;

		private bool hasBeenLit;
		private float litTimeLeft;
		private Player parentPlayer;

		private void Awake() { parentPlayer = SearchForParentPlayer(); }

		private void Update()
		{
			if (!gameObject.activeInHierarchy) { return; }

			litTimeLeft -= Time.deltaTime;
			EventCollection.Instance.TorchDecayEvent.Invoke(litTimeLeft / maxLitTimeInSeconds);

			if (litTimeLeft > 0.0f) { return; }

			hasBeenLit = false;
			parentPlayer.ToggleHeldItems(1);

			InventoryItemData torchData = parentPlayer.Inventory.Items.FirstOrDefault(x => x.Key.Name == "Torch").Key;
			parentPlayer.Inventory.RemoveItems(torchData);
		}

		private void OnEnable()
		{
			parentPlayer.Sanity.IsHoldingTorch = true;
			
			if (hasBeenLit) { return; }

			litTimeLeft = maxLitTimeInSeconds;
			hasBeenLit = true;
		}

		private void OnDisable() { parentPlayer.Sanity.IsHoldingTorch = false; }	

		private Player SearchForParentPlayer()
		{
			Transform t = transform;

			while (t.parent != null)
			{
				var player = t.parent.GetComponent<Player>();

				if (player != null) { return player; }

				t = t.parent;
			}

			Debug.LogError("Could not find parent player");
			return null;
		}
	}
}