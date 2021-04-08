namespace Delirium.AI
{
	
	/// <summary> All of the states the roaming enemy can be in. </summary>
	public enum RoamingEnemyState
	{
		/// <summary>Enemy has not detected a target and is walking between predefined points.</summary>
		Roaming,
		/// <summary>Enemy has detected a target and is running towards it.</summary>
		TargetLock,
		/// <summary>Enemy is within the attack range of the player and stands still to attack.</summary>
		Attack,
		/// <summary>Enemy has arrived on the last known position of the target and will look around if it can find the target again.</summary>
		Search,
		/// <summary>Enemy had detected a target, but has lost it. So its moving to the last know target position.</summary>
		MoveToLastPosition,
	}
}