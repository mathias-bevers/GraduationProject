using System.Collections;
using System.Collections.Generic;
using Delirium.Tools;
using UnityEngine;

namespace Delirium.AI
{
	public class EnemyManager : Singleton<EnemyManager>
	{
		private List<EnemyAI> registeredEnemies = new List<EnemyAI>();

		public void RegisterEnemy(EnemyAI enemy)
		{
			if (registeredEnemies.Contains(enemy))
			{
				Debug.Log("This enemy already exists");
				return;
			}
			
			registeredEnemies.Add(enemy);
		}

		private void Start()
		{
			StartCoroutine(UpdateEnemyStates(0.25f));
		}

		private IEnumerator UpdateEnemyStates(float delay)
		{
			while (true)
			{
				yield return new WaitForSeconds(delay);
				foreach (EnemyAI registeredEnemy in registeredEnemies)
				{
					registeredEnemy.UpdateState();
				}
			}
		}
	}
}