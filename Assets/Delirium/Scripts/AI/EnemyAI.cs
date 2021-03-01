using UnityEngine;
using UnityEngine.AI;

namespace Delirium.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class EnemyAI : MonoBehaviour
	{
		public EnemyAIState State { get; private set; }
		private FieldOfView fieldOfView;

		private float searchTimer = 5.0f;
		private NavMeshAgent navMeshAgent;

		private Player targetPlayer;

		private void Awake()
		{
			EnemyManager.Instance.RegisterEnemy(this);
			navMeshAgent = GetComponent<NavMeshAgent>();
			fieldOfView = GetComponent<FieldOfView>();
		}

		public void UpdateState()
		{
			Player newTarget = fieldOfView.FindPlayer();

			if (newTarget == targetPlayer) { return; }

			if (newTarget != null)
			{
				State = EnemyAIState.TargetLock;
				targetPlayer = newTarget;
			}
		}
	}
}