using System;
using Delirium.Events;
using Delirium.Lore;
using Delirium.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Delirium
{
	public class GeneralHUDMenu : Menu
	{
		[SerializeField] private GameObject healthBar;
		[SerializeField] private GameObject sanityBar;
		[SerializeField] private Image torchDurabilityBar;
		[SerializeField] private TextMeshProUGUI pickupText;

		public GameObject TorchDurabilityBar => torchDurabilityBar.gameObject;

		private void Awake() { IsHUD = true; }

		protected override void Start()
		{
			Opened += OnOpened;

			base.Start();

			Health playerHealth = GameManager.Instance.Player.Health;
			playerHealth.HealthChangedEvent += OnHealthChanged;
			OnHealthChanged(playerHealth);

			OnSanityChanged(GameManager.Instance.Player.Sanity);
			EventCollection.Instance.SanityChangedEvent.AddListener(OnSanityChanged);

			EventCollection.Instance.EnteredInteractionZoneEvent.AddListener(OnEnteredInteractionZone);
			EventCollection.Instance.ItemHoverEvent.AddListener(OnItemHoverEnter);
			EventCollection.Instance.DisableInteractTextEvent.AddListener(() => pickupText.gameObject.SetActive(false));

			EventCollection.Instance.TorchDecayEvent.AddListener(fillAmount => torchDurabilityBar.fillAmount = fillAmount);
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		public void OnOpened()
		{
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
					pickupText.SetText(item.Name != "Tongue" ? $"Press <color=red>E</color> to pick up {item.Name}" : "Press <color=red>E</color> to cut out your tongue");
					break;

				case CraftingRecipeData craftingRecipe:
					pickupText.SetText($"Press <color=red>E</color> to pick up {craftingRecipe.Result.Name} blueprint");
					break;

				case LoreScrollData loreScrollData:
					pickupText.SetText($"Press <color=red>E</color> to pick up note #{loreScrollData.Number}");
					break;

				default: throw new NotSupportedException();
			}
		}

		private void OnEnteredInteractionZone(ZoneHandler.InteractionZone zoneType)
		{
			pickupText.gameObject.SetActive(true);

			switch (zoneType)
			{
				case ZoneHandler.InteractionZone.None:
					pickupText.gameObject.SetActive(false);
					break;

				case ZoneHandler.InteractionZone.Campfire:
					pickupText.SetText("Press <color=red>E</color> to light the campfire");
					break;

				case ZoneHandler.InteractionZone.Ritual:
					pickupText.SetText("Press <color=red>E</color> to perform the ritual");
					break;

				default: throw new ArgumentOutOfRangeException(nameof(zoneType), zoneType, null);
			}
		}
	}
}