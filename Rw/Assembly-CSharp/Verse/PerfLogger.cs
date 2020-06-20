using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000466 RID: 1126
	public static class PerfLogger
	{
		// Token: 0x06002150 RID: 8528 RVA: 0x000CC4DB File Offset: 0x000CA6DB
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000CC4F7 File Offset: 0x000CA6F7
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "", false);
			PerfLogger.Reset();
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000CC51C File Offset: 0x000CA71C
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000CC5AE File Offset: 0x000CA7AE
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000CC5BC File Offset: 0x000CA7BC
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000CC5CA File Offset: 0x000CA7CA
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04001464 RID: 5220
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04001465 RID: 5221
		private static long start;

		// Token: 0x04001466 RID: 5222
		private static long current;

		// Token: 0x04001467 RID: 5223
		private static int indent;
	}
}
