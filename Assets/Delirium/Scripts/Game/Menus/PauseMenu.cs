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
			Opened += OnOpened;
			Closed += OnClosed;

			base.Start();
			quitButton.onClick.AddListener(Application.Quit);

#if UNITY_EDITOR
			quitButton.onClick.AddListener(() => UnityEditor.EditorApplication.isPlaying = false);
#endif
			continueButton.onClick.AddListener(() => { Close(); });
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		public void OnOpened()
		{
			Time.timeScale = 0.0f;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public void OnClosed()
		{
			Time.timeScale = 1.0f;
			MenuManager.Instance.OpenMenu<PlayerHUDMenu>();
		}
	}
}