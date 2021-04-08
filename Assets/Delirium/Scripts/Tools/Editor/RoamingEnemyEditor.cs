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

			if (enemyAI == null || enemyAI.IdlePathPoints == null) { return; }

			Handles.color = Color.magenta;

			for (var i = 0; i < enemyAI.IdlePathPoints.Count; i++)
			{
				if (i == enemyAI.IdlePathPoints.Count - 1)
				{
					Handles.SphereHandleCap(0, enemyAI.IdlePathPoints[i], Quaternion.identity, 0.25f, EventType.Repaint);
					Handles.DrawLine(enemyAI.IdlePathPoints[i], enemyAI.IdlePathPoints[0]);
					continue;
				}

				Handles.SphereHandleCap(0, enemyAI.IdlePathPoints[i], Quaternion.identity, 0.25f, EventType.Repaint);
				Handles.DrawLine(enemyAI.IdlePathPoints[i], enemyAI.IdlePathPoints[i + 1]);
			}
		}
	}
}