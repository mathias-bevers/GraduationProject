﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Delirium.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class RoamingEnemy : MonoBehaviour
	{
		private const float ATTACK_ANIMATION_DURATION = 1.5f;
		private const float ATTACK_DISTANCE = 1.9f;
		private const float RUNNING_SPEED = 15.0f;
		private const float WALKING_SPEED = 3.5f;

		[SerializeField] private int damage;
		public List<Vector3> idlePathPoints;

		public Health Health { get; } = new Health(25);

		private EnemyAIState State
		{
			get => state;
			set
			{
				if (value != state) { StateChangedEvent?.Invoke(value); }

				state = value;
			}
		}

		private Collider attackTrigger;
		private EnemyAIState state = EnemyAIState.Roaming;
		private FieldOfView fieldOfView;
		private float searchTimer = 5.0f;
		private float attackTimer = ATTACK_ANIMATION_DURATION;
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
			if (target != null && !target.IsAlive) { return; }

			switch (State)
			{
				case EnemyAIState.Roaming:
					healthBarImage.gameObject.SetActive(false);
					attackTrigger.enabled = false;

					navMeshAgent.speed = WALKING_SPEED;

					navMeshAgent.destination = idlePathPoints[pathPointIndex % idlePathPoints.Count];
					if (HasArrived()) { pathPointIndex++; }

					break;

				case EnemyAIState.TargetLock:
					healthBarImage.gameObject.SetActive(true);
					attackTrigger.enabled = false;

					navMeshAgent.speed = RUNNING_SPEED;
					navMeshAgent.destination = target.transform.position;
					if (Vector3.Distance(transform.position, target.transform.position) <= ATTACK_DISTANCE) { State = EnemyAIState.Attack; }

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
			if (State != EnemyAIState.Attack) { return; }

			Health collisionHealth = other.gameObject.GetComponent<Player>()?.Health;
			collisionHealth?.TakeDamage(damage);
		}

		private void OnValidate()
		{
			if (idlePathPoints.Count <= 0) { return; }

			idlePathPoints[0] = transform.position;
		}

		public event Action<EnemyAIState> StateChangedEvent;

		public void UpdateState()
		{
			if (State == EnemyAIState.Attack && attackTimer > 0) { return; }

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