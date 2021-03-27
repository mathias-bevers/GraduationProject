using Delirium;
using Delirium.Events;
using UnityEngine;

#if UNITY_EDITOR
namespace Testing
{
	public class DeveloperTesting : MonoBehaviour
	{
		[SerializeField] private LoreScrollData scrollToUnlock;
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1)) { GameManager.Instance.Player.Health.TakeDamage(10); }

			if (Input.GetKeyDown(KeyCode.F2)) { GameManager.Instance.Player.Sanity.DEVELOPERTEST(); }

			if (Input.GetKeyDown(KeyCode.F3)) { EventCollection.Instance.LoreScrollFoundEvent.Invoke(scrollToUnlock, GameManager.Instance.Player);}
		}
	}
}
#endif