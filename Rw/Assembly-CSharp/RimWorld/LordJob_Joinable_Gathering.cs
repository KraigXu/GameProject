using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200077D RID: 1917
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x00116F2A File Offset: 0x0011512A
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x00116F32 File Offset: 0x00115132
		public LordJob_Joinable_Gathering()
		{
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x00116F3A File Offset: 0x0011513A
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x06003211 RID: 12817
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		// Token: 0x06003212 RID: 12818 RVA: 0x00116F57 File Offset: 0x00115157
		protected virtual bool ShouldBeCalledOff()
		{
			return !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x00116F78 File Offset: 0x00115178
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			if (!GatheringsUtility.ShouldPawnKeepGathering(p, this.gatheringDef))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && this.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x00116FE4 File Offset: 0x001151E4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x0011702C File Offset: 0x0011522C
		private bool IsGatheringAboutToEnd()
		{
			return this.timeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x00117043 File Offset: 0x00115243
		private bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		// Token: 0x04001B3F RID: 6975
		protected IntVec3 spot;

		// Token: 0x04001B40 RID: 6976
		protected Pawn organizer;

		// Token: 0x04001B41 RID: 6977
		protected GatheringDef gatheringDef;

		// Token: 0x04001B42 RID: 6978
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
