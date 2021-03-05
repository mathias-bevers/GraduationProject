using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class GameManager : Singleton<GameManager>
	{
		public Player Player { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			Player[] players = FindObjectsOfType<Player>();
			if (players.Length > 1) { Debug.LogError($"There are {players.Length} instances of player found. There should only be one in the scene."); }

			Player = players[0];
		}
	}
}