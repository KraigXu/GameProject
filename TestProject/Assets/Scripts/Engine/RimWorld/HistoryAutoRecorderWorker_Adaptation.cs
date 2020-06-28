using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000935 RID: 2357
	public class HistoryAutoRecorderWorker_Adaptation : HistoryAutoRecorderWorker
	{
		// Token: 0x060037E3 RID: 14307 RVA: 0x0012BC04 File Offset: 0x00129E04
		public override float PullRecord()
		{
			return Find.StoryWatcher.watcherAdaptation.AdaptDays;
		}
	}
}
