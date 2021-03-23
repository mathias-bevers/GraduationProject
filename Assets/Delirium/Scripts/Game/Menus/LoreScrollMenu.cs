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

		public override void Open()
		{
			base.Open();

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public override void Close()
		{
			base.Close();

			MenuManager.Instance.OpenMenu<GeneralHudMenu>();
		}
	}
}