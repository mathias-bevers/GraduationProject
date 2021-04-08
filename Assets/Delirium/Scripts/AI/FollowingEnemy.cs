using System.Collections;
using Delirium.Audio;
using UnityEngine;
using UnityEngine.AI;

namespace Delirium.AI
{
	/// <summary>
	///     This class is used to let a NavMeshAgent follow an transform.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class FollowingEnemy : MonoBehaviour
	{
		private const float CALCULATE_PATH_DELAY = 2.0f;

		private NavMeshAgent agent;
		private Transform transformToFollow;

		private void OnTriggerEnter(Collider other)
		{
			var player = other.gameObject.GetComponent<Player>();
			if (player) { player.Health.TakeDamage(50); }
		}

		/// <summary>
		///     The Initialize method is used because unity's Awake/Start can't take any arguments.
		///     This method set the transform that has to be followed, plays a JumpScare sound and Starts the <see cref="CalculatePath" /> coroutine.
		/// </summary>
		/// <param name="transformToFollow">The transform the FollowingEnemy has to follow for its entire life span.</param>
		public void Initialize(Transform transformToFollow)
		{
			agent = GetComponent<NavMeshAgent>();

			this.transformToFollow = transformToFollow;
			AudioManager.Instance.Play("Jumpscare_02");

			StartCoroutine(CalculatePath());
		}

		/// <summary>
		///     Sets the designation of the NavMeshAgent. This is done with a delay, other wise the FollowingEnemy would always stand still because it did not have a path.
		/// </summary>
		/// <returns></returns>
		private IEnumerator CalculatePath()
		{
			while (gameObject.activeInHierarchy)
			{
				yield return new WaitForSeconds(CALCULATE_PATH_DELAY);
				agent.destination = transformToFollow.position;
			}
		}
	}
}