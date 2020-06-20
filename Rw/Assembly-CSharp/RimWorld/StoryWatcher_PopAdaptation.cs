using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2C RID: 2604
	public class StoryWatcher_PopAdaptation : IExposable
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x00145A50 File Offset: 0x00143C50
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00145A58 File Offset: 0x00143C58
		public void Notify_PawnEvent(Pawn p, PopAdaptationEvent ev)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new object[]
				{
					"PopAdaptation event: ",
					ev,
					" - ",
					p
				}), false);
			}
			if (ev == PopAdaptationEvent.GainedColonist)
			{
				this.adaptDays = 0f;
			}
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00145AB8 File Offset: 0x00143CB8
		public void PopAdaptationWatcherTick()
		{
			if (Find.TickManager.TicksGame % 30000 == 171)
			{
				float num = 0.5f;
				this.adaptDays += num;
			}
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x00145AF0 File Offset: 0x00143CF0
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00145B08 File Offset: 0x00143D08
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x040023FB RID: 9211
		private float adaptDays;

		// Token: 0x040023FC RID: 9212
		private const int UpdateInterval = 30000;
	}
}
