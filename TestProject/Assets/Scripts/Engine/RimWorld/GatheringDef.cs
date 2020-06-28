using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008C4 RID: 2244
	public class GatheringDef : Def
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600360C RID: 13836 RVA: 0x0012581B File Offset: 0x00123A1B
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x0012582A File Offset: 0x00123A2A
		public GatheringWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (GatheringWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x0012585C File Offset: 0x00123A5C
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		// Token: 0x04001E40 RID: 7744
		public Type workerClass = typeof(GatheringWorker);

		// Token: 0x04001E41 RID: 7745
		public DutyDef duty;

		// Token: 0x04001E42 RID: 7746
		public float randomSelectionWeight;

		// Token: 0x04001E43 RID: 7747
		public bool respectTimetable = true;

		// Token: 0x04001E44 RID: 7748
		public List<ThingDef> gatherSpotDefs;

		// Token: 0x04001E45 RID: 7749
		[MustTranslate]
		public string letterTitle;

		// Token: 0x04001E46 RID: 7750
		[MustTranslate]
		public string letterText;

		// Token: 0x04001E47 RID: 7751
		[MustTranslate]
		public string calledOffMessage;

		// Token: 0x04001E48 RID: 7752
		[MustTranslate]
		public string finishedMessage;

		// Token: 0x04001E49 RID: 7753
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		// Token: 0x04001E4A RID: 7754
		private GatheringWorker worker;
	}
}
