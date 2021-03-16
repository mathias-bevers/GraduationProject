using System;
using Delirium.Events;
using Delirium.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Delirium
{
	public class GeneralHudMenu : Menu
	{
		[SerializeField] private GameObject healthBar;
		[SerializeField] private GameObject sanityBar;
		[SerializeField] private TextMeshProUGUI pickupText;

		private void Awake() { IsHUD = true; }

		protected override void Start()
		{
			base.Start();

			Health playerHealth = GameManager.Instance.Player.Health;
			playerHealth.HealthChangedEvent += OnHealthChanged;
			OnHealthChanged(playerHealth);

			OnSanityChanged(GameManager.Instance.Player.Sanity);
			EventCollection.Instance.SanityChangedEvent.AddListener(OnSanityChanged);
			
			EventCollection.Instance.ItemHoverEvent.AddListener(OnItemHoverEnter);
			EventCollection.Instance.ItemHoverExitEvent.AddListener(() => pickupText.gameObject.SetActive(false));
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		public override void Open()
		{
			base.Open();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void OnHealthChanged(Health health)
		{
			var fill = healthBar.transform.Find("Fill").GetComponent<Image>();
			fill.fillAmount = health.Health01;

			var text = healthBar.GetComponentInChildren<TextMeshProUGUI>();
			text.SetText(health.CurrentHealth.ToString());
		}

		private void OnSanityChanged(Sanity sanity)
		{
			var fill = sanityBar.transform.Find("Fill").GetComponent<Image>();
			fill.fillAmount = (float) sanity.CurrentSanity / Sanity.MAX_SANITY;

			var text = sanityBar.GetComponentInChildren<TextMeshProUGUI>();
			text.SetText(sanity.CurrentSanity.ToString());
		}

		private void OnItemHoverEnter(ScriptableObject data)
		{
			pickupText.gameObject.SetActive(true);

			switch (data)
			{
				case InventoryItemData item:
					pickupText.SetText($"Press <color=red>E</color> to pick up {item.Name}");
					break;
				case CraftingRecipeData craftingRecipe:
					pickupText.SetText($"Press <color=red>E</color> to pick up {craftingRecipe.Result.Name} blueprint");
					break;
				default: throw new NotSupportedException();
			}
		}
	}
}