using UnityEngine;
using UnityEngine.AI;

namespace Delirium.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class FollowingEnemy : MonoBehaviour
	{
		private NavMeshAgent agent;
		private Transform transformToFollow;

		private void Update() { agent.destination = transformToFollow.position; }

		private void OnTriggerEnter(Collider other)
		{
			var player = other.gameObject.GetComponent<Player>();

			if (player) { player.Health.TakeDamage(50); }
		}

		public void Initialize(Transform transformToFollow)
		{
			Destroy(gameObject, 20.0f);

			agent = GetComponent<NavMeshAgent>();

			this.transformToFollow = transformToFollow;
		}
	}
}