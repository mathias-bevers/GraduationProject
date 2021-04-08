using System;
using UnityEngine;

namespace Delirium.AI
{
	/// <summary>
	///     This class is used to communicate with the animator which is attached to the <see cref="RoamingEnemy" /> game object.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class RoamingEnemyAnimation : MonoBehaviour
	{
		private static readonly int _hunt = Animator.StringToHash("Hunt");
		private static readonly int _attack = Animator.StringToHash("Attack");
		private static readonly int _roaming = Animator.StringToHash("Roaming");
		private static readonly int _searching = Animator.StringToHash("Search");

		private Animator animator;

		private void Start()
		{
			GetComponent<RoamingEnemy>().StateChangedEvent += OnStateChanged;

			animator = GetComponent<Animator>();
		}

		/// <summary>
		///     Set the correct animation triggers based on the current state of the attached <see cref="RoamingEnemy" />.
		/// </summary>
		/// <param name="state">The new state of the attached <see cref="RoamingEnemy" />.</param>
		private void OnStateChanged(RoamingEnemyState state)
		{
			ResetAllTriggers();

			switch (state)
			{
				case RoamingEnemyState.Roaming:
					animator.SetTrigger(_roaming);
					break;

				case RoamingEnemyState.TargetLock:
					animator.SetTrigger(_hunt);
					break;

				case RoamingEnemyState.Search:
					animator.SetTrigger(_searching);
					break;

				case RoamingEnemyState.MoveToLastPosition:
					animator.SetTrigger(_roaming);
					break;
				case RoamingEnemyState.Attack:
					animator.SetTrigger(_attack);
					break;

				default: throw new ArgumentOutOfRangeException(nameof(state), state, "This state does not exists");
			}
		}

		/// <summary>
		///     Reset all the current active triggers to inactive.
		/// </summary>
		private void ResetAllTriggers()
		{
			animator.ResetTrigger(_attack);
			animator.ResetTrigger(_hunt);
			animator.ResetTrigger(_roaming);
			animator.ResetTrigger(_searching);
		}
	}
}