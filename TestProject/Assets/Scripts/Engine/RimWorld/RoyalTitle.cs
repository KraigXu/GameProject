using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB2 RID: 2994
	public class RoyalTitle : IExposable
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600468C RID: 18060 RVA: 0x0017CEDD File Offset: 0x0017B0DD
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return Mathf.Max((180000 - (GenTicks.TicksGame - this.receivedTick)).TicksToDays(), 0f);
			}
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0017CF00 File Offset: 0x0017B100
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return GenTicks.TicksGame - this.receivedTick < 180000 && !pawn.IsQuestLodger();
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x0017CF20 File Offset: 0x0017B120
		public RoyalTitle()
		{
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x0017CF2F File Offset: 0x0017B12F
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x0017CF64 File Offset: 0x0017B164
		public void RoyalTitleTick(Pawn pawn)
		{
			if (pawn.IsHashIntervalTick(833) && this.conceited && pawn.Spawned && pawn.IsFreeColonist && (!pawn.IsQuestLodger() || pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				pawn.royalty.IssueDecree(false, null);
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x0017D01C File Offset: 0x0017B21C
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		// Token: 0x04002884 RID: 10372
		public Faction faction;

		// Token: 0x04002885 RID: 10373
		public RoyalTitleDef def;

		// Token: 0x04002886 RID: 10374
		public int receivedTick = -1;

		// Token: 0x04002887 RID: 10375
		public bool wasInherited;

		// Token: 0x04002888 RID: 10376
		public bool conceited;

		// Token: 0x04002889 RID: 10377
		private const int DecreeCheckInterval = 833;

		// Token: 0x0400288A RID: 10378
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
