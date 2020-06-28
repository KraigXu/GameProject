using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098C RID: 2444
	public class QuestPart_SendShuttleAway : QuestPart
	{
		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060039DA RID: 14810 RVA: 0x00133579 File Offset: 0x00131779
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x00133589 File Offset: 0x00131789
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x001335BE File Offset: 0x001317BE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x001335FC File Offset: 0x001317FC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap);
				this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center, randomPlayerHomeMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00133672 File Offset: 0x00131872
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x0400221A RID: 8730
		public string inSignal;

		// Token: 0x0400221B RID: 8731
		public Thing shuttle;

		// Token: 0x0400221C RID: 8732
		public bool dropEverything;
	}
}
