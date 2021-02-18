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

		private void Awake()
		{
			EventCollection.Instance.HealthChangedEvent.AddListener(OnHealthChanged);
		}

		protected override void Start()
		{
			IsHUD = true;
			base.Start();
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
	}
}