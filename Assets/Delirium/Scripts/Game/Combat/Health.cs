using System;
using Delirium.Audio;
using Delirium.Events;
using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     Basic health class which keeps track of the health value and supports healing and taking damage.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	public class Health
	{
		private readonly int maxHealth;

		public Health(int maxHealth)
		{
			this.maxHealth = maxHealth;
			CurrentHealth = this.maxHealth;

			HealthChangedEvent?.Invoke(this);
		}

		public int CurrentHealth { get; private set; }
		public float Health01 => (float) CurrentHealth / maxHealth;

		/// <summary>
		///     This event is invoked in the <see cref="TakeDamage" /> and <see cref="Heal" /> method when the health value has been changed with the Health class as parameter.
		/// </summary>
		public event Action<Health> HealthChangedEvent;

		/// <summary>
		///     This event is invoked in the <see cref="TakeDamage" /> method when the health value is 0.
		/// </summary>
		public event Action DiedEvent;

		/// <summary>
		///     Decrease the health value by the given amount, it is automatically clamped between 0 and <see cref="maxHealth" />.
		///     When damage is applied it immediately checks if the health value is 0, meaning the attached game object is dead.
		///     When the game object has died, the <see cref="DiedEvent" /> is invoked.
		/// </summary>
		/// <param name="amount">The amount the health value should be decreased with.</param>
		public void TakeDamage(int amount)
		{
			CurrentHealth -= amount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
			HealthChangedEvent?.Invoke(this);

			AudioManager.Instance.Play("TakeDamage");

			if (CurrentHealth > 0) { return; }

			DiedEvent?.Invoke();
		}

		/// <summary>
		///     Increase the health value with the given amount, it is automatically clamped between 0 and <see cref="maxHealth" />.
		/// </summary>
		/// <param name="amount">The amount the health value should be increased with.</param>
		public void Heal(int amount)
		{
			CurrentHealth += amount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
			EventCollection.Instance.OpenPopupEvent.Invoke($"Healed to {CurrentHealth} HP", PopupMenu.PopupLevel.Info);
			HealthChangedEvent?.Invoke(this);
		}
	}
}