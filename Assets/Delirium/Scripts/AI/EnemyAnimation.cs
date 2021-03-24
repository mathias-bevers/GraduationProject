using System;
using UnityEngine;

namespace Delirium.AI
{
	[RequireComponent(typeof(Animator))]
	public class EnemyAnimation : MonoBehaviour
	{
		private static readonly int _hunt = Animator.StringToHash("Hunt");
		private static readonly int _attack = Animator.StringToHash("Attack");
		private static readonly int _roaming = Animator.StringToHash("Roaming");
		private static readonly int _searching = Animator.StringToHash("Search");

		private Animator animator;

		private void Start()
		{
			GetComponent<EnemyAI>().StateChangedEvent += OnStateChanged;

			animator = GetComponent<Animator>();
		}

		private void OnStateChanged(EnemyAIState state)
		{
			ResetAllTriggers();

			switch (state)
			{
				case EnemyAIState.Roaming:
					animator.SetTrigger(_roaming);
					break;

				case EnemyAIState.TargetLock:
					animator.SetTrigger(_hunt);
					break;

				case EnemyAIState.Search:
					animator.SetTrigger(_searching);
					break;

				case EnemyAIState.MoveToLastPosition:
					animator.SetTrigger(_roaming);
					break;
				case EnemyAIState.Attack:
					animator.SetTrigger(_attack);
					break;

				default: throw new ArgumentOutOfRangeException(nameof(state), state, "This state does not exists");
			}
		}

		private void ResetAllTriggers()
		{
			animator.ResetTrigger(_attack);
			animator.ResetTrigger(_hunt);
			animator.ResetTrigger(_roaming);
			animator.ResetTrigger(_searching);
		}
	}
}