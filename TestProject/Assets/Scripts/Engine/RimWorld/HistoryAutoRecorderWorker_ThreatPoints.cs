using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000934 RID: 2356
	public class HistoryAutoRecorderWorker_ThreatPoints : HistoryAutoRecorderWorker
	{
		// Token: 0x060037E1 RID: 14305 RVA: 0x0012BBE5 File Offset: 0x00129DE5
		public override float PullRecord()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return 0f;
			}
			return StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap) / 10f;
		}
	}
}
