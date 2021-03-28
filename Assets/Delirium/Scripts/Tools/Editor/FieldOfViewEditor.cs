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
			Handles.DrawWireArc(fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight, Vector3.up, Vector3.forward, 360.0f, fieldOfView.ViewRadius);

			Vector3 viewAngleA = fieldOfView.DirectionFormAngle(-fieldOfView.ViewAngle * 0.5f, false);
			Vector3 viewAngleB = fieldOfView.DirectionFormAngle(fieldOfView.ViewAngle * 0.5f, false);

			Handles.DrawLine(
				fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight,
				fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight + viewAngleA * fieldOfView.ViewRadius
			);
			Handles.DrawLine(
				fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight,
				fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight + viewAngleB * fieldOfView.ViewRadius
			);

			Player player = fieldOfView.FindPlayer();

			if (player == null) { return; }

			Handles.color = Color.red;
			Handles.DrawLine(fieldOfView.transform.position + Vector3.up * fieldOfView.EyeHeight, player.transform.position);
		}
	}
}