using System.Collections;
using System.Collections.Generic;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.AI
{
	/// <summary>
	///     This manager keeps track of all the enemies in the scene and updates the state of the roaming enemies.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	public class EnemyManager : Singleton<EnemyManager>
	{
		private const float FOLLOWING_ENEMY_SPAWN_DISTANCE = 10.0f;

		private readonly List<RoamingEnemy> registeredEnemies = new List<RoamingEnemy>();

		[SerializeField] private GameObject hordeParent;
		private GameObject followingEnemyObject;

		private void Start() { StartCoroutine(UpdateEnemyStates(0.25f)); }

		/// <summary>
		///     Register a <see cref="RoamingEnemy" />, so its state is updated.
		/// </summary>
		/// <param name="roamingEnemy">The roaming enemy that has to be registered.</param>
		public void RegisterRoamingEnemy(RoamingEnemy roamingEnemy)
		{
			if (registeredEnemies.Contains(roamingEnemy))
			{
				Debug.Log("This enemy already exists");
				return;
			}

			registeredEnemies.Add(roamingEnemy);
		}

		/// <summary>
		///     Unregister a <see cref="RoamingEnemy" />, so its  state won't be updated anymore.
		/// </summary>
		/// <param name="roamingEnemy">The roaming enemy that has to be unregistered.</param>
		public void UnregisterEnemy(RoamingEnemy roamingEnemy)
		{
			if (!registeredEnemies.Contains(roamingEnemy))
			{
				Debug.Log("This enemy doesn't exists");
				return;
			}

			registeredEnemies.Remove(roamingEnemy);
		}

		/// <summary>
		///     Loops over all the registered <see cref="RoamingEnemy" /> and updates their states. Has a delay, updating every frame would be very expensive.
		/// </summary>
		/// <param name="delay">Time between updates.</param>
		private IEnumerator UpdateEnemyStates(float delay)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				foreach (RoamingEnemy registeredEnemy in registeredEnemies) { registeredEnemy.UpdateState(); }
			}
		}

		/// <summary> Spawn a <see cref="FollowingEnemy" /> directly in front of the player.</summary>
		/// <param name="playerToFollow">The player that has to be followed.</param>
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

		/// <summary>
		///     Enables the <see cref="hordeParent" /> game object initializes all of the <see cref="FollowingEnemy" /> in its children.
		/// </summary>
		/// <param name="invokingPlayerTransform">Player that needs to be followed</param>
		public void SpawnEnemyHorde(Transform invokingPlayerTransform)
		{
			hordeParent.SetActive(true);

			foreach (FollowingEnemy followingEnemy in hordeParent.GetComponentsInChildren<FollowingEnemy>()) { followingEnemy.Initialize(invokingPlayerTransform); }
		}

		/// <summary>
		///     Destroys the current <see cref="FollowingEnemy" /> game object.
		/// </summary>
		private IEnumerator DestroyFollowingEnemy()
		{
			yield return new WaitForSeconds(20.0f);
			Destroy(followingEnemyObject);
			followingEnemyObject = null;
		}
	}
}