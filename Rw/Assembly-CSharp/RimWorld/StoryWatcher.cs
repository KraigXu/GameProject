using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A28 RID: 2600
	public sealed class StoryWatcher : IExposable
	{
		// Token: 0x06003D89 RID: 15753 RVA: 0x00145732 File Offset: 0x00143932
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0014574C File Offset: 0x0014394C
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x040023EE RID: 9198
		public StatsRecord statsRecord = new StatsRecord();

		// Token: 0x040023EF RID: 9199
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		// Token: 0x040023F0 RID: 9200
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
