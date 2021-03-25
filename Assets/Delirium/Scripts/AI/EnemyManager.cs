using System.Collections;
using System.Collections.Generic;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.AI
{
	public class EnemyManager : Singleton<EnemyManager>
	{
		private const float FOLLOWING_ENEMY_SPAWN_DISTANCE = 10.0f;
		
		private readonly List<RoamingEnemy> registeredEnemies = new List<RoamingEnemy>();
		private GameObject followingEnemyObject;

		private void Start() { StartCoroutine(UpdateEnemyStates(0.25f)); }

		public void RegisterEnemy(RoamingEnemy roamingEnemy)
		{
			if (registeredEnemies.Contains(roamingEnemy))
			{
				Debug.Log("This enemy already exists");
				return;
			}

			registeredEnemies.Add(roamingEnemy);
		}

		public void UnregisterEnemy(RoamingEnemy roamingEnemy)
		{
			if (!registeredEnemies.Contains(roamingEnemy))
			{
				Debug.Log("This enemy doesn't exists");
				return;
			}

			registeredEnemies.Remove(roamingEnemy);
		}

		private IEnumerator UpdateEnemyStates(float delay)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				foreach (RoamingEnemy registeredEnemy in registeredEnemies) { registeredEnemy.UpdateState(); }
			}
		}

		public void SpawnFollowingEnemy(Player playerToFollow)
		{
			if (followingEnemyObject != null) { return; }

			followingEnemyObject = Instantiate(ResourcesManager.Instance.FollowingEnemyPrefab);

			Transform playerCameraTransform = playerToFollow.GetComponent<PlayerMovement>()?.CameraTransform;
			followingEnemyObject.transform.position = playerToFollow.transform.position + playerCameraTransform.forward * FOLLOWING_ENEMY_SPAWN_DISTANCE;
			followingEnemyObject.transform.LookAt(playerToFollow.transform);

			followingEnemyObject.GetComponent<FollowingEnemy>()?.Initialize(playerToFollow.transform);

			StartCoroutine(DestroyFollowingEnemy());
		}

		private IEnumerator DestroyFollowingEnemy()
		{
			yield return new WaitForSeconds(20.0f);
			Destroy(followingEnemyObject);
			followingEnemyObject = null;
		}
	}
}