﻿using System;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
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
		public event Action<Health> HealthChangedEvent;
		public event Action DiedEvent;

		public void TakeDamage(int amount)
		{
			CurrentHealth -= amount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
			HealthChangedEvent?.Invoke(this);

			if (CurrentHealth > 0) { return; }

			DiedEvent?.Invoke();
		}

		public void Heal(int amount)
		{
			if (CurrentHealth == maxHealth)
			{
				MenuManager.Instance.GetMenu<PopupMenu>().ShowPopup("Healing will have no effect", PopupMenu.PopupLevel.Waring);
				return;
			}

			CurrentHealth += amount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
			MenuManager.Instance.GetMenu<PopupMenu>().ShowPopup($"Healed to {CurrentHealth} HP", PopupMenu.PopupLevel.Info);
			HealthChangedEvent?.Invoke(this);
		}
	}
}