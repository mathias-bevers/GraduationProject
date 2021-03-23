using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Delirium.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class EnemyAI : MonoBehaviour
	{
		private const float ATTACK_DISTANCE = 1.3f;

		[SerializeField] private int damage;
		public List<Vector3> idlePathPoints;

		public Health Health { get; } = new Health(25);

		private EnemyAIState State
		{
			get => state;
			set
			{
				state = value;
				StateChangedEvent?.Invoke(State);
			}
		}


		private EnemyAIState state = EnemyAIState.Roaming;
		private FieldOfView fieldOfView;
		private float searchTimer = 5.0f;
		private Image healthBarImage;
		private int pathPointIndex;
		private NavMeshAgent navMeshAgent;
		private Player target;
		private Vector3 lastKnownTargetPosition;

		private void Awake()
		{
			EnemyManager.Instance.RegisterEnemy(this);
			navMeshAgent = GetComponent<NavMeshAgent>();
			fieldOfView = GetComponent<FieldOfView>();

			healthBarImage = GetComponentInChildren<Image>();
		}

		private void Start()
		{
			Health.HealthChangedEvent += health => { healthBarImage.fillAmount = health.Health01; };
			Health.DiedEvent += () =>
			{
				EnemyManager.Instance.UnregisterEnemy(this);
				Destroy(gameObject);
			};
		}

		private void Update()
		{
			switch (State)
			{
				case EnemyAIState.Roaming:
					navMeshAgent.destination = idlePathPoints[pathPointIndex % idlePathPoints.Count];
					if (HasArrived()) { pathPointIndex++; }

					break;

				case EnemyAIState.TargetLock:
					navMeshAgent.destination = target.transform.position;

					float distance = Vector3.Distance(transform.position, target.transform.position);
					if (distance <= ATTACK_DISTANCE) { State = EnemyAIState.Attack; }

					break;

				case EnemyAIState.Search:
					float rotation = Mathf.Sin(Time.time) * 90.0f;
					transform.rotation = Quaternion.Euler(Vector3.up * rotation);

					searchTimer -= Time.deltaTime;
					if (searchTimer <= 0.0f)
					{
						State = EnemyAIState.Roaming;
						lastKnownTargetPosition = Vector3.zero;
						searchTimer = 5.0f;
					}

					break;

				case EnemyAIState.MoveToLastPosition:
					navMeshAgent.destination = lastKnownTargetPosition;
					if (!navMeshAgent.hasPath) { State = EnemyAIState.Search; }

					break;

				case EnemyAIState.Attack:
					navMeshAgent.destination = transform.position;
					if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Z_Attack")) { UpdateState(); }

					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			Health collisionHealth = other.gameObject.GetComponent<Player>()?.Health;
			collisionHealth?.TakeDamage(damage);
		}

		public event Action<EnemyAIState> StateChangedEvent;

		public void UpdateState()
		{
			Player newTarget = fieldOfView.FindPlayer();

			if (newTarget != null && State != EnemyAIState.Attack)
			{
				State = EnemyAIState.TargetLock;
				target = newTarget;
				lastKnownTargetPosition = target.transform.position;
				return;
			}

			if (lastKnownTargetPosition == Vector3.zero)
			{
				State = EnemyAIState.Roaming;
				return;
			}

			if (State == EnemyAIState.Search) { return; }

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