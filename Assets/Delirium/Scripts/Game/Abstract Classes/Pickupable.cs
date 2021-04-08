using Delirium.Events;
using UnityEngine;

namespace Delirium.AbstractClasses
{
	/// <summary>
	///     This class is used to identify a item that can be picked up. And to invoke some of the basic events.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	public abstract class Pickupable : MonoBehaviour
	{
		/// <summary>
		/// 
		/// </summary>
		public bool InReach { get; set; }

		private void OnDestroy() { EventCollection.Instance.DisableInteractTextEvent?.Invoke(); }

		private void OnMouseExit() { EventCollection.Instance.DisableInteractTextEvent?.Invoke(); }
	}
}