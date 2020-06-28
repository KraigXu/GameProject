using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x02000472 RID: 1138
	public static class ProfilerThreadCheck
	{
		// Token: 0x060021A7 RID: 8615 RVA: 0x000CD054 File Offset: 0x000CB254
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000CD054 File Offset: 0x000CB254
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
