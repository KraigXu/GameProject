using System;
using Verse;

namespace RimWorld
{
	
	public sealed class StoryWatcher : IExposable
	{
		
		public void StoryWatcherTick()
		{
			this.watcherAdaptation.AdaptationWatcherTick();
			this.watcherPopAdaptation.PopAdaptationWatcherTick();
		}

		
		public void ExposeData()
		{
			Scribe_Deep.Look<StatsRecord>(ref this.statsRecord, "statsRecord", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_Adaptation>(ref this.watcherAdaptation, "watcherAdaptation", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher_PopAdaptation>(ref this.watcherPopAdaptation, "watcherPopAdaptation", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		
		public StatsRecord statsRecord = new StatsRecord();

		
		public StoryWatcher_Adaptation watcherAdaptation = new StoryWatcher_Adaptation();

		
		public StoryWatcher_PopAdaptation watcherPopAdaptation = new StoryWatcher_PopAdaptation();
	}
}
