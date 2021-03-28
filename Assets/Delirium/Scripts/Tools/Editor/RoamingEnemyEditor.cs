using Delirium.AI;
using UnityEditor;
using UnityEngine;

namespace Delirium.Editor 
{
	[CustomEditor(typeof(RoamingEnemy))]
	public class RoamingEnemyEditor : UnityEditor.Editor
	{
		private void OnSceneGUI()
		{
			var enemyAI = target as RoamingEnemy;

			if (enemyAI == null || enemyAI.idlePathPoints == null) { return; }
	Handles.color = Color.magenta;

			for (var i = 0; i < enemyAI.idlePathPoints.Count; i++)
			{
				if (i == enemyAI.idlePathPoints.Count - 1)
				{
					Handles.SphereHandleCap(0, enemyAI.idlePathPoints[i], Quaternion.identity, 0.25f, EventType.Repaint);
					Handles.DrawLine(enemyAI.idlePathPoints[i], enemyAI.idlePathPoints[0]);
					continue;
				}

				Handles.SphereHandleCap(0, enemyAI.idlePathPoints[i], Quaternion.identity, 0.25f, EventType.Repaint);
				Handles.DrawLine(enemyAI.idlePathPoints[i], enemyAI.idlePathPoints[i + 1]);
			}
		}
		
	}
}