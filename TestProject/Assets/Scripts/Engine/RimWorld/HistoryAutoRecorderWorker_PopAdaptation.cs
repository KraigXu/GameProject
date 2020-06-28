using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000936 RID: 2358
	public class HistoryAutoRecorderWorker_PopAdaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x060037E5 RID: 14309 RVA: 0x0012BC15 File Offset: 0x00129E15
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherPopAdaptation.AdaptDays;
		}
	}
}
