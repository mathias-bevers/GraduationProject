using Delirium.Tools;
using UnityEngine;

namespace Delirium
{
	public class ResourcesManager : Singleton<ResourcesManager>
	{
		public GameObject FollowingEnemyPrefab { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			
			FollowingEnemyPrefab = Resources.Load<GameObject>("FollowingEnemy");
		}
	}
}