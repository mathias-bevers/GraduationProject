using Delirium;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	[SerializeField] private float viewRadius;
	[SerializeField, Range(0.0f, 360.0f)] private float viewAngle;
	[SerializeField] private LayerMask targetMask;
	[SerializeField] private LayerMask obstacleMask;

	public float ViewRadius => viewRadius;
	public float ViewAngle => viewAngle;

	public Player FindPlayer()
	{
		Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		foreach (Collider target in targets)
		{
			Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

			if (Vector3.Angle(transform.forward, directionToTarget) > viewAngle / 2) { continue; }

			float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

			if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)) { continue; }

			var player = target.GetComponent<Player>();

			if (player == null) { continue; }

			return player;
		}

		return null;
	}

	public Vector3 DirectionFormAngle(float angleInDegrees, bool isGlobal)
	{
		if (!isGlobal) { angleInDegrees += transform.eulerAngles.y; }

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}