using Delirium;
using UnityEngine;

namespace Testing
{
	public class DeveloperTesting : MonoBehaviour
	{
		private float fps;

		private void Update()
		{
			fps = 1.0f / Time.deltaTime;

			if (Input.GetKeyDown(KeyCode.F1)) { GameManager.Instance.Player.Health.TakeDamage(10); }

			if (Input.GetKeyDown(KeyCode.F2)) { GameManager.Instance.Player.Sanity.DEVELOPERTEST(); }
		}

		private void OnGUI() { GUI.Label(new Rect(10, 10, 150, 50), fps.ToString("0.##")); }
	}
}