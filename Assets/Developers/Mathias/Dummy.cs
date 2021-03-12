using UnityEngine;
using UnityEngine.UI;

namespace Delirium.Testing
{
	public class Dummy : MonoBehaviour
	{
		[SerializeField] private Image healthBar;

		public Health Health { get; } = new Health(25);

		private void Start()
		{
			Health.HealthChangedEvent += health =>
			{
				healthBar.fillAmount = health.Health01;
				Debug.Log($"Dummy has taken damage, current health: {health.CurrentHealth}");
			};

			Health.DiedEvent += () => { Destroy(gameObject); };
		}
	}
}