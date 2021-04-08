using Delirium.AI;
using UnityEditor;
using UnityEngine;

namespace Delirium.Editor
{
	[CustomEditor(typeof(FieldOfView))]
	public class FieldOfViewEditor : UnityEditor.Editor
	{
		private void OnSceneGUI()
		{
			var fieldOfView = target as FieldOfView;

			if (fieldOfView == null) { return; }

			Handles.color = Color.white;
			Handles.DrawWireArc(fieldOfView.Origin, Vector3.up, Vector3.forward, 360.0f, fieldOfView.ViewRadius);

			Vector3 viewAngleA = fieldOfView.DirectionFromAngle(-fieldOfView.ViewAngle * 0.5f, false);
			Vector3 viewAngleB = fieldOfView.DirectionFromAngle(fieldOfView.ViewAngle * 0.5f, false);

			Handles.DrawLine(fieldOfView.Origin, fieldOfView.Origin + viewAngleA * fieldOfView.ViewRadius);
			Handles.DrawLine(fieldOfView.Origin, fieldOfView.Origin + viewAngleB * fieldOfView.ViewRadius);

			Player player = fieldOfView.FindPlayer();

			if (player == null) { return; }

			Handles.color = Color.red;
			Handles.DrawLine(fieldOfView.Origin, player.transform.position);
		}
	}
}