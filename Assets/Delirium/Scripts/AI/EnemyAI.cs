using System;
using System.ComponentModel;
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
		private Player target;
		private Vector3 lastKnownTargetPosition;

		private void Awake()
		{
			EnemyManager.Instance.RegisterEnemy(this);
			navMeshAgent = GetComponent<NavMeshAgent>();
			fieldOfView = GetComponent<FieldOfView>();
		}

		private void Update()
		{
			switch (State)
			{
				case EnemyAIState.Idle: break;

				case EnemyAIState.TargetLock:
					navMeshAgent.destination = target.transform.position;
					break;

				case EnemyAIState.Search:
					float rotation = Mathf.Sin(Time.deltaTime * 180.0f);
					transform.rotation = Quaternion.Euler(Vector3.up * rotation);

					searchTimer -= Time.deltaTime;
					if (searchTimer <= 0.0f)
					{
						State = EnemyAIState.Idle;
						searchTimer = 5.0f;
					}

					break;

				case EnemyAIState.MoveToLastPosition:
					navMeshAgent.destination = lastKnownTargetPosition;
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		public void UpdateState()
		{
			Player newTarget = fieldOfView.FindPlayer();

			if (newTarget != null)
			{
				State = EnemyAIState.TargetLock;
				target = newTarget;
				lastKnownTargetPosition = target.transform.position;
				return;
			}

			State = EnemyAIState.MoveToLastPosition;
		}
	}
}