using System;

namespace RimWorld
{
	// Token: 0x02000937 RID: 2359
	public class HistoryAutoRecorderWorker_PopIntent : HistoryAutoRecorderWorker
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x0012BC26 File Offset: 0x00129E26
		public override float PullRecord()
		{
			return StorytellerUtilityPopulation.PopulationIntent * 10f;
		}
	}
}
