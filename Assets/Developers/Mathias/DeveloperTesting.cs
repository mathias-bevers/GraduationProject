using System;
using Delirium;
using UnityEngine;

namespace Testing
{
	public class DeveloperTesting : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				GameManager.Instance.Player.Health.TakeDamage(10);
			}
		}
	}
}