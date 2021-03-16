using System.Collections;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	public class Sanity : MonoBehaviour
	{
		public const int MAX_SANITY = 100;

		public int CurrentSanity { get; private set; } = 100;
		public bool IsHoldingTorch { get; set; } = false;

		private void Start()
		{
			StartCoroutine(UpdateSanity(-2));
		}

		private void OnTriggerEnter(Collider other)
		{
			var lightSource = other.GetComponent<Light>();
			if (lightSource == null) { return; }

			StopAllCoroutines();
			StartCoroutine(UpdateSanity(2));
		}

		private void OnTriggerExit(Collider other)
		{
			var lightSource = other.GetComponent<Light>();
			if (lightSource == null) { return; }

			StopAllCoroutines();
			StartCoroutine(UpdateSanity(-2));
		}

		private IEnumerator UpdateSanity(int adjustmentAmount)
		{
			if (IsHoldingTorch && adjustmentAmount < 0) { yield break; }

			while (true)
			{
				yield return new WaitForSeconds(3f);

				CurrentSanity += adjustmentAmount;
				CurrentSanity = Mathf.Clamp(CurrentSanity, 0, MAX_SANITY);
				EventCollection.Instance.SanityChangedEvent?.Invoke(this);
			}
		}
	}
}