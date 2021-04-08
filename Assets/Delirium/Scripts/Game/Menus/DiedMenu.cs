using Delirium.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delirium
{
	/// <summary>
	///     This class is used to Handle the death splash screen's UI elements and animator.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class DiedMenu : Menu
	{
		private static readonly int _playerDied = Animator.StringToHash("PlayerDied");

		[SerializeField] private Button continueButton;
		private Animator animator;

		protected override void Start()
		{
			Opened += OnOpened;

			base.Start();
			animator = Content.GetComponent<Animator>();
		}

		public override bool CanBeOpened() => true;
		public override bool CanBeClosed() => true;

		public void OnOpened()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Died")) { return; }

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			animator.SetTrigger(_playerDied);
			continueButton.onClick.AddListener(() => SceneManager.LoadScene(0));
		}
	}
}