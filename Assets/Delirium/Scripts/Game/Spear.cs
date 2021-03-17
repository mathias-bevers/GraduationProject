using Delirium.AI;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.Combat
{
	[RequireComponent(typeof(Animator))]
	public class Spear : MonoBehaviour
	{
		private static readonly int _attack = Animator.StringToHash("Attack");

		[SerializeField] private int damage;
		private Animator animator;
		private Health enemyHealth;

		private void Awake() { animator = GetComponent<Animator>(); }

		private void Update()
		{
			if (!Input.GetMouseButtonDown(0) || !animator.GetCurrentAnimatorStateInfo(0).IsName("SpearIdle") || MenuManager.Instance.IsAnyOpen) { return; }

			animator.SetTrigger(_attack);
			enemyHealth?.TakeDamage(damage);
		}

		private void OnTriggerEnter(Collider other)
		{
			//TODO: change to enemy AI
			enemyHealth = other.gameObject.GetComponent<EnemyAI>()?.Health;

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SpearAttack")) { return; }

			enemyHealth?.TakeDamage(damage);
		}

		private void OnTriggerExit(Collider other) { enemyHealth = null; }
	}
}