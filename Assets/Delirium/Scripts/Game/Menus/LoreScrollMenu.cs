using Delirium.Tools;
using TMPro;
using UnityEngine;

namespace Delirium
{
	public class LoreScrollMenu : Menu
	{
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private TextMeshProUGUI text;

		protected override void Start()
		{
			Opened += OnOpened;
			Closed += () => { MenuManager.Instance.OpenMenu<GeneralHudMenu>(); };

			base.Start();

			Content.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(Close);
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		public void SetScrollText(LoreScrollData loreScroll)
		{
			title.SetText($"Note #{loreScroll.Number}");
			text.SetText(loreScroll.Text);
		}

		private void OnOpened()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}