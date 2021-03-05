using Delirium.Events;
using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class Health
	{
		private readonly int maxHealth;
		
		public int CurrentHealth { get; private set; }
		public float Health01 => (float)CurrentHealth / maxHealth;
		
		public Health(int maxHealth)
		{
			this.maxHealth = maxHealth;
			CurrentHealth = this.maxHealth;
		}

		public void TakeDamage(int amount)
		{
			CurrentHealth -= amount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
			EventCollection.Instance.HealthChangedEvent.Invoke(this);

			if (CurrentHealth > 0) { return; }

			//TODO: implement some death thingy.
			Debug.Log("You died!");
			
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
			EventCollection.Instance.HealthChangedEvent.Invoke(this);
		}
	}
}