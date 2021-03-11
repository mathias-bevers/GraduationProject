using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Delirium.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class EnemyAI : MonoBehaviour
	{
		public List<Vector3> idlePathPoints;

		public EnemyAIState State { get; private set; } = EnemyAIState.Idle;
		private FieldOfView fieldOfView;
		private float searchTimer = 5.0f;

		private int pathPointIndex;
		private NavMeshAgent navMeshAgent;
		private Player target;
		private Vector3 lastKnownTargetPosition = Vector3.zero;

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
				case EnemyAIState.Idle:
					navMeshAgent.destination = idlePathPoints[pathPointIndex % idlePathPoints.Count];
					if (HasArrived()) { pathPointIndex++; }

					break;

				case EnemyAIState.TargetLock:
					navMeshAgent.destination = target.transform.position;
					break;

				case EnemyAIState.Search:
					float rotation = Mathf.Sin(Time.time) * 90.0f;
					transform.rotation = Quaternion.Euler(Vector3.up * rotation);

					searchTimer -= Time.deltaTime;
					if (searchTimer <= 0.0f)
					{
						State = EnemyAIState.Idle;

						lastKnownTargetPosition = Vector3.zero;
						searchTimer = 5.0f;
					}

					break;

				case EnemyAIState.MoveToLastPosition:
					navMeshAgent.destination = lastKnownTargetPosition;
					if (!navMeshAgent.hasPath) { State = EnemyAIState.Search; }

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

			if (lastKnownTargetPosition == Vector3.zero)
			{
				State = EnemyAIState.Idle;
				return;
			}

			State = EnemyAIState.MoveToLastPosition;
		}

		private bool HasArrived()
		{
			if (navMeshAgent.pathPending) { return false; }

			if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) { return false; }

			return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0.0f;
		}
	}
}