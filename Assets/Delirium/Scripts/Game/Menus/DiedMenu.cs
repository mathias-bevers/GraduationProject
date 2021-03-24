using Delirium.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delirium
{
	public class DiedMenu : Menu
	{
		private static readonly int _playerDied = Animator.StringToHash("PlayerDied");

		[SerializeField] private Button continueButton;
		private Animator animator;
		
		public override bool CanBeOpened() => true;
		public override bool CanBeClosed() => true;

		protected override void Start()
		{
			base.Start();
			animator = Content.GetComponent<Animator>();
		}

		public override void Open()
		{
			base.Open();

			if (animator.GetCurrentAnimatorStateInfo(0).IsName("Died")) { return; }

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			
			animator.SetTrigger(_playerDied);
			continueButton.onClick.AddListener(() => SceneManager.LoadScene(0));
		}
	}
}