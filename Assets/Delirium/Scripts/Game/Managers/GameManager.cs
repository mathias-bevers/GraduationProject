using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	/// <summary>
	///     This class keeps track of the player. Handles when the player has died.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	public class GameManager : Singleton<GameManager>
	{
		/// <summary>
		///     Get the only player that should exist in the scene.
		/// </summary>
		public Player Player { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			Player[] players = FindObjectsOfType<Player>();
			if (players.Length > 1) { Debug.LogError($"There are {players.Length} instances of player found. There should only be one in the scene."); }

			Player = players[0];

			Player.Health.DiedEvent += OnPlayerDeath;
		}

		private void OnPlayerDeath()
		{
			if (!Player.IsAlive) { return; }

			Player.IsAlive = false;
			MenuManager.Instance.CloseAllMenus();
			MenuManager.Instance.CloseMenu<PlayerHUDMenu>();
			MenuManager.Instance.OpenMenu<DiedMenu>();
		}
	}
}