using System.Collections;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class Sanity : MonoBehaviour
	{
		public const int MAX_SANITY = 100;
		private const int SANITY_ADJUST_DELAY = 3;

		public int CurrentSanity { get; private set; } = 100;
		public bool IsHoldingTorch { get; set; } = false;

		private bool inLightZone;
		private float timer = 3.0f;

		private void Update()
		{
			if (IsHoldingTorch && !inLightZone) { return; }

			timer -= Time.deltaTime;

			if (timer > 0) { return; }

			if (inLightZone) { CurrentSanity += 2; }
			else { CurrentSanity -= 2; }

			CurrentSanity = Mathf.Clamp(CurrentSanity, 0, MAX_SANITY);
			
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
	}
}