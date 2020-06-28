using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public static class ProfilerPairValidation
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x000C2A96 File Offset: 0x000C0C96
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000C2AAC File Offset: 0x000C0CAC
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
					return;
				}
			}
		}

		// Token: 0x040013A3 RID: 5027
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}
