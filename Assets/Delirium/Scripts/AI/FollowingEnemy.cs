using System.Collections;
using Delirium.Audio;
using UnityEngine;
using UnityEngine.AI;

namespace Delirium.AI
{
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

		public void Initialize(Transform transformToFollow)
		{
			agent = GetComponent<NavMeshAgent>();

			this.transformToFollow = transformToFollow;
			AudioManager.Instance.Play("Jumpscare_02");

			StartCoroutine(CalculatePath());
		}

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