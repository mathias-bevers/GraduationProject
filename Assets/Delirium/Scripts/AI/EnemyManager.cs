using System;
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
		[SerializeField] private GameObject hordeParent;
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
			//DONE: the enemy is moving in between init frame and first update frame. If you need to set the position, if possible, give the position as an argument in the instantiate method.

			if (followingEnemyObject != null) { return; }
			
			Transform playerCameraTransform = playerToFollow.GetComponent<PlayerMovement>()?.CameraTransform;
			Vector3 pos = playerToFollow.transform.position + playerCameraTransform.forward * FOLLOWING_ENEMY_SPAWN_DISTANCE;

			followingEnemyObject = Instantiate(ResourceManager.Instance.FollowingEnemyPrefab, pos, Quaternion.identity);

			followingEnemyObject.transform.LookAt(playerToFollow.transform);

			followingEnemyObject.GetComponent<FollowingEnemy>()?.Initialize(playerToFollow.transform);

			StartCoroutine(DestroyFollowingEnemy());
		}

		public void SpawnEnemyHorde(Transform invokingPlayerTransform)
		{
			foreach (FollowingEnemy followingEnemy in hordeParent.GetComponentsInChildren<FollowingEnemy>()) { followingEnemy.Initialize(invokingPlayerTransform); }

			hordeParent.SetActive(true);
		}

		private IEnumerator DestroyFollowingEnemy()
		{
			yield return new WaitForSeconds(20.0f);
			Destroy(followingEnemyObject);
			followingEnemyObject = null;
		}
	}
}