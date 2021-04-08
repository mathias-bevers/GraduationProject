using Delirium.AI;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.Combat
{
	/// <summary>
	///     This class is used to communicate with the spear's animator and deal damage to hit objects.
	///     <para>Made by: Mathias Bevers</para>
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class Spear : MonoBehaviour
	{
		private static readonly int _attack = Animator.StringToHash("Attack");

		[SerializeField] private int damage;
		private Animator animator;
		private Health enemyHealth;

		private void Awake() { animator = GetComponent<Animator>(); }

		/// <summary>
		///     Check whether the attack animation can be played, if so Play it and try to deal damage if the spear has already collided.
		/// </summary>
		private void Update()
		{
			if (!Input.GetMouseButtonDown(0) || !animator.GetCurrentAnimatorStateInfo(0).IsName("SpearIdle") || MenuManager.Instance.IsAnyOpen) { return; }

			animator.SetTrigger(_attack);


			try { enemyHealth?.TakeDamage(damage); }
			catch (MissingReferenceException) { enemyHealth = null; }
		}

		/// <summary>
		///     When collided with another object, try to get the health of the <see cref="RoamingEnemy" />. And if the spear is in the attach state, try to deal damage.
		/// </summary>
		private void OnTriggerEnter(Collider other)
		{
			enemyHealth = other.gameObject.GetComponent<RoamingEnemy>()?.Health;

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SpearAttack")) { return; }

			enemyHealth?.TakeDamage(damage);
		}

		private void OnTriggerExit(Collider other) { enemyHealth = null; }
	}
}