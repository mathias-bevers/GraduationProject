using System;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delirium
{
	public class EndMenu : Menu
	{
		private static readonly int _startEnding = Animator.StringToHash("StartEnding");

		[SerializeField] private Animation player;
		[SerializeField] private Animation ferry;
		[SerializeField] private Button backToMainMenuButton;

		private Animator animator;

		protected override void Start()
		{
			animator = GetComponent<Animator>();
			backToMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));

			Opened += OnOpened;
			base.Start();
		}

		public override bool CanBeOpened() => !MenuManager.Instance.IsAnyOpen;
		public override bool CanBeClosed() => true;

		private void OnOpened() { animator.SetTrigger(_startEnding); }

		//This is called by the animation event.
		public void AlertInvokers(string message)
		{
			switch (message)
			{
				case "fadeEnd":
					player.Play();
					ferry.Play();
					break;
				
				case "enableMouse":
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					break;
			}
		}
	}
}