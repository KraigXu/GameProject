using System;
using System.Diagnostics;

namespace Verse
{
	
	public static class ProfilerThreadCheck
	{
		
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
