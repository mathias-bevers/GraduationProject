using Delirium.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Delirium
{
	public class PauseMenu : Menu
	{
		[SerializeField] private Button continueButton;
		[SerializeField] private Button quitButton;

		protected override void Start()
		{
			base.Start();
			quitButton.onClick.AddListener(Application.Quit);

			continueButton.onClick.AddListener(
				() =>
				{
					MenuManager.Instance.OpenMenu<GeneralHudMenu>();
					Close();
				}
			);
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		public override void Open()
		{
			if (!CanBeOpened()) { return; }

			base.Open();

			Time.timeScale = 0.0f;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public override void Close()
		{
			if (!CanBeClosed()) { return; }
			
			base.Close();
			Time.timeScale = 1.0f;
			
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}