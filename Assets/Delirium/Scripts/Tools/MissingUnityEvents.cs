//THIS IS AUTO GENERATED CODE, ANY CHANGES WILL BE OVERWRITTEN
using System;
using UnityEngine;

namespace Delirium 
{
#if ILWeavedEventsOn
	public static class TransformExtensions 
	{
		
		public static void BindSetPositionExecuting(this Transform obj, EventHandler<Vector3> handler)
	    {
	        obj.SetPositionExecuting += handler;
	    }
	
		public static void UnBindSetPositionExecuting(this Transform obj, EventHandler<Vector3> handler)
	    {
	        obj.SetPositionExecuting -= handler;
	    }
		
	}

	
#else
	public static class TransformExtensions 
	{
		
		public static void BindSetPositionExecuting(this Transform obj, EventHandler<Vector3> handler)
	    {
			Debug.LogWarning("Build symbol ILWeavedEventsOn not specified.");
			
	    }
	
		public static void UnBindSetPositionExecuting(this Transform obj, EventHandler<Vector3> handler)
	    {
			Debug.LogWarning("Build symbol ILWeavedEventsOn not specified.");
			
	    }
		
	}
#endif
}