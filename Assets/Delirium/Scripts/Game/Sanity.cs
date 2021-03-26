using Delirium.AI;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	[RequireComponent(typeof(Player))]
	public class Sanity : MonoBehaviour
	{
		public const int MAX_SANITY = 100;
		private const int SANITY_ADJUST_DELAY = 3;

		public int CurrentSanity { get; private set; } = 100;
		public bool IsHoldingTorch { get; set; } = false;
		private bool inLightZone;
		private float timer = 3.0f;

		private Player parentPlayer;

		private void Update()
		{
			if (IsHoldingTorch && !inLightZone) { return; }

			timer -= Time.deltaTime;

			if (timer > 0) { return; }

			if (inLightZone) { CurrentSanity += 2; }
			else { CurrentSanity -= 2; }

			CurrentSanity = Mathf.Clamp(CurrentSanity, 0, MAX_SANITY);

			if (CurrentSanity < 20) { parentPlayer.Health.TakeDamage(1); }

			if (CurrentSanity < 10) { /*EnemyManager.Instance.SpawnFollowingEnemy(parentPlayer);*/ }

			EventCollection.Instance.SanityChangedEvent?.Invoke(this);
			timer = SANITY_ADJUST_DELAY;
		}

		private void OnTriggerEnter(Collider other)
		{
			var lightSource = other.GetComponent<Light>();
			if (lightSource == null) { return; }

			inLightZone = true;
		}

		private void OnTriggerExit(Collider other)
		{
			var lightSource = other.GetComponent<Light>();
			if (lightSource == null) { return; }

			inLightZone = false;
		}

		public void RegisterPlayer(Player player) { parentPlayer = player; }

		//TODO: Remove when done testing.
		public void DEVELOPERTEST()
		{
			CurrentSanity = 20;
			EventCollection.Instance.SanityChangedEvent?.Invoke(this);
		}
	}
}