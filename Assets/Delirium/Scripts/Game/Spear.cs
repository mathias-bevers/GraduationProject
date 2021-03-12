using Delirium.Testing;
using UnityEngine;

namespace Delirium.Combat
{
	[RequireComponent(typeof(Animator))]
	public class Spear : MonoBehaviour
	{
		private static readonly int _attack = Animator.StringToHash("Attack");

		[SerializeField] private int damage;
		private Animator animator;
		private Health touchingHealth;

		private void Awake() { animator = GetComponent<Animator>(); }

		private void Update()
		{
			if (!Input.GetMouseButtonDown(0) || !animator.GetCurrentAnimatorStateInfo(0).IsName("SpearIdle")) { return; }

			animator.SetTrigger(_attack);
			touchingHealth?.TakeDamage(damage);
		}

		private void OnTriggerEnter(Collider other)
		{
			//TODO: change to enemy AI
			touchingHealth = other.gameObject.GetComponent<Dummy>()?.Health;

			if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SpearAttack")) { return; }

			touchingHealth?.TakeDamage(damage);
		}

		private void OnTriggerExit(Collider other) { touchingHealth = null; }
	}
}