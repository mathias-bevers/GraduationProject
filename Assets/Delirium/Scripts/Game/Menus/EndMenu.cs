using System;
using Delirium.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Delirium
{
	/// <summary>
	/// This class is used the handle the ending animations, it listens to event markers which are called by the animator.
	/// <para>Made by: Mathias Bevers</para>
	/// </summary>
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

		/// <summary>
		/// This method handles the event markers, send by the animator.
		/// </summary>
		/// <param name="markerName">The name of the event marker that is called by the animator.</param>
		public void AlertInvokers(string markerName)
		{
			switch (markerName)
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