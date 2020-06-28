using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098E RID: 2446
	public class QuestPart_SendShuttleAwayOnCleanup : QuestPart
	{
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0013370F File Offset: 0x0013190F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x0013371F File Offset: 0x0013191F
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00133740 File Offset: 0x00131940
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0013376B File Offset: 0x0013196B
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x0400221D RID: 8733
		public Thing shuttle;

		// Token: 0x0400221E RID: 8734
		public bool dropEverything;
	}
}
