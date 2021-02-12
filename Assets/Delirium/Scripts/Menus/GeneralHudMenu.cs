using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class GeneralHudMenu : Menu
	{
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
	}
}