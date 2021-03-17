using System.Linq;
using UnityEngine;

namespace Delirium
{
	public class Torch : MonoBehaviour
	{
		[SerializeField] private float maxLightTimeInSeconds;

		private bool hasBeenLit;
		private float litTimeLeft;
		private Player parentPlayer;

		private void Start() { parentPlayer = SearchForParentPlayer(); }

		private void Update()
		{
			if (!gameObject.activeInHierarchy) { return; }

			litTimeLeft -= Time.deltaTime;

			if (litTimeLeft > 0.0f) { return; }

			Debug.Log("Torch died!");

			hasBeenLit = false;
			parentPlayer.ToggleHeldItems(1);

			InventoryItemData torchData = parentPlayer.Inventory.Items.FirstOrDefault(x => x.Key.Name == "Torch").Key;
			parentPlayer.Inventory.RemoveItems(torchData);
		}

		private void OnEnable()
		{
			if (hasBeenLit) { return; }

			litTimeLeft = maxLightTimeInSeconds;
			hasBeenLit = true;
		}

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