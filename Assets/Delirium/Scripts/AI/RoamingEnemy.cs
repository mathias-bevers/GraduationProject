using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Delirium.AI
{
	/// <summary>
	///     This class is used to control the behaviour of a NavMeshAgent that can roam through the map.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class RoamingEnemy : MonoBehaviour
	{
		private const float ATTACK_ANIMATION_DURATION = 1.5f;
		private const float ATTACK_DISTANCE = 1.9f;
		private const float RUNNING_SPEED = 15.0f;
		private const float WALKING_SPEED = 3.5f;

		[SerializeField] private int damage;
		[SerializeField] private List<Vector3> idlePathPoints;

		/// <summary>
		/// The points of the path that the NavMeshAgent should follow, these points are defined in the inspector.
		/// </summary>
		public List<Vector3> IdlePathPoints => idlePathPoints;
		
		/// <summary>
		/// Health system of the RoamingEnemy.
		/// </summary>
		public Health Health { get; } = new Health(25);

		private RoamingEnemyState State
		{
			get => state;
			set
			{
				if (value != state) { StateChangedEvent?.Invoke(value); }

				state = value;
			}
		}

		private Collider attackTrigger;
		private FieldOfView fieldOfView;
		private float searchTimer = 5.0f;
		private float attackTimer = ATTACK_ANIMATION_DURATION;
		private Image healthBarImage;
		private int pathPointIndex;
		private NavMeshAgent navMeshAgent;
		private Player target;
		private RoamingEnemyState state = RoamingEnemyState.Roaming;
		private Vector3 lastKnownTargetPosition;

		private void Awake()
		{
			EnemyManager.Instance.RegisterRoamingEnemy(this);
			navMeshAgent = GetComponent<NavMeshAgent>();
			fieldOfView = GetComponent<FieldOfView>();
			attackTrigger = GetComponent<Collider>();

			healthBarImage = GetComponentInChildren<Image>();
		}

		private void Start()
		{
			Health.HealthChangedEvent += health =>
			{
				if (healthBarImage == null) { throw new NullReferenceException("Health bar image could not be found"); }

				healthBarImage.fillAmount = health.Health01;
			};

			Health.DiedEvent += () =>
			{
				EnemyManager.Instance.UnregisterEnemy(this);
				Destroy(gameObject);
			};
		}

		private void Update()
		{
			//The update method perform the actions that have to be taken based on the current state.
			if (target != null && !target.IsAlive) { return; }

			switch (State)
			{
				case RoamingEnemyState.Roaming:
					healthBarImage.gameObject.SetActive(false);
					attackTrigger.enabled = false;

					navMeshAgent.speed = WALKING_SPEED;

					navMeshAgent.destination = IdlePathPoints[pathPointIndex % IdlePathPoints.Count];
					if (HasArrived()) { pathPointIndex++; }

					break;

				case RoamingEnemyState.TargetLock:
					healthBarImage.gameObject.SetActive(true);
					attackTrigger.enabled = false;

					navMeshAgent.speed = RUNNING_SPEED;
					navMeshAgent.destination = target.transform.position;
					if (Vector3.Distance(transform.position, target.transform.position) <= ATTACK_DISTANCE) { State = RoamingEnemyState.Attack; }

					break;

				case RoamingEnemyState.Search:
					float rotation = Mathf.Sin(Time.time) * 90.0f;
					transform.rotation = Quaternion.Euler(Vector3.up * rotation);

					searchTimer -= Time.deltaTime;
					if (searchTimer <= 0.0f)
					{
						State = RoamingEnemyState.Roaming;
						lastKnownTargetPosition = Vector3.zero;
						searchTimer = 5.0f;
					}

					break;

				case RoamingEnemyState.MoveToLastPosition:
					navMeshAgent.destination = lastKnownTargetPosition;
					if (!navMeshAgent.hasPath) { State = RoamingEnemyState.Search; }

					break;

				case RoamingEnemyState.Attack:
					attackTrigger.enabled = true;
					navMeshAgent.destination = transform.position;

					attackTimer -= Time.deltaTime;
					if (attackTimer <= 0)
					{
						UpdateState();
						attackTimer = ATTACK_ANIMATION_DURATION;
					}

					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (State != RoamingEnemyState.Attack) { return; }

			Health collisionHealth = other.gameObject.GetComponent<Player>()?.Health;
			collisionHealth?.TakeDamage(damage);
		}

		private void OnValidate()
		{
			//Setting the start point to the position of the roaming enemy. This makes defining a path in the editor lot easier.
			if (IdlePathPoints.Count <= 0) { return; }

			IdlePathPoints[0] = transform.position;
		}

		/// <summary>
		///     This event is invoked when the state is set and different from the previous state.
		/// </summary>
		public event Action<RoamingEnemyState> StateChangedEvent;

		/// <summary>
		///     Updates the state based on whether a target is found. If an target is found is based on <see cref="FieldOfView" />.
		/// </summary>
		public void UpdateState()
		{
			if (State == RoamingEnemyState.Attack && attackTimer > 0) { return; }

			Player newTarget = fieldOfView.FindPlayer();

			if (newTarget != null)
			{
				State = RoamingEnemyState.TargetLock;
				target = newTarget;
				lastKnownTargetPosition = target.transform.position;
				return;
			}

			if (lastKnownTargetPosition == Vector3.zero)
			{
				State = RoamingEnemyState.Roaming;
				return;
			}

			if (State == RoamingEnemyState.Search) { return; }

			State = RoamingEnemyState.MoveToLastPosition;
		}

		/// <summary>
		///     Checks if the NavMeshAgent has arrived on its destination, using only the agent's hasPath can lead to some weird behaviour.
		///     This method gives it some extra checks whether it has arrived on its destination.
		/// </summary>
		/// <returns>Returns true if the NavMeshAgent has a arrived on its destination.</returns>
		private bool HasArrived()
		{
			if (navMeshAgent.pathPending) { return false; }

			if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)) { return false; }

			return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0.0f;
		}
	}
}