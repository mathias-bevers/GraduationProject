using Delirium.Events;
using UnityEngine;

namespace Delirium.AbstractClasses
{
	public abstract class Pickupable : MonoBehaviour
	{
		public bool InReach { get; set; }
		
		private void OnDestroy() { EventCollection.Instance.DisableInteractTextEvent?.Invoke(); }

		private void OnMouseExit() { EventCollection.Instance.DisableInteractTextEvent?.Invoke(); }
	}
}