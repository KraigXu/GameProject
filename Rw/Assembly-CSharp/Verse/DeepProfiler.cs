using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000425 RID: 1061
	public static class DeepProfiler
	{
		// Token: 0x06001FBF RID: 8127 RVA: 0x000C2224 File Offset: 0x000C0424
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000C2294 File Offset: 0x000C0494
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000C22B2 File Offset: 0x000C04B2
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x0400139B RID: 5019
		public static volatile bool enabled = true;

		// Token: 0x0400139C RID: 5020
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x0400139D RID: 5021
		private static readonly object DeepProfilersLock = new object();
	}
}
