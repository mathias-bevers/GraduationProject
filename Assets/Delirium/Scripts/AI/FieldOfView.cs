using UnityEngine;

namespace Delirium.AI
{
	/// <summary>
	///     This class is used as the Eyes of the <see cref="RoamingEnemy" />.
	///     <para>Made by Mathias Bevers</para>
	/// </summary>
	public class FieldOfView : MonoBehaviour
	{
		[SerializeField] private float eyeHeight;
		[SerializeField] private float viewRadius;
		[SerializeField, Range(0.0f, 360.0f)] private float viewAngle;
		[SerializeField] private LayerMask targetMask;
		[SerializeField] private LayerMask obstacleMask;

		/// <summary>
		/// The origin is set by the transform.position and adding the eyeHeight.
		/// </summary>
		public Vector3 Origin => transform.position + Vector3.up * eyeHeight;
		
		/// <summary>
		/// Returns the viewRadius which is set in the inspector.
		/// </summary>
		public float ViewRadius => viewRadius;
		
		/// <summary>
		/// Returns the viewAngle which is set in the inspector.
		/// </summary>
		public float ViewAngle => viewAngle;

		/// <summary>
		///     This method tries to find the player in the given field of view.
		/// </summary>
		/// <returns>Returns the first player that is within the FOV</returns>
		public Player FindPlayer()
		{
			Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

			foreach (Collider target in targets)
			{
				Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

				if (Vector3.Angle(transform.forward, directionToTarget) > viewAngle / 2) { continue; }

				float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

				if (Physics.Raycast(Origin, directionToTarget, distanceToTarget, obstacleMask)) { continue; }

				var player = target.GetComponent<Player>();

				if (player == null) { continue; }

				return player;
			}

			return null;
		}

		/// <summary>
		///     Get the direction vector from an angle in degrees.
		/// </summary>
		/// <param name="angleInDegrees">The angle that has to be converted to a vector3</param>
		/// <param name="isGlobal">If this is set to false the transform's y angle is first added.</param>
		/// <returns>The direction in a Vector3</returns>
		public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal)
		{
			if (!isGlobal) { angleInDegrees += transform.eulerAngles.y; }

			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
		}
	}
}