using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class RoyalTitle : IExposable
	{
		
		// (get) Token: 0x0600468C RID: 18060 RVA: 0x0017CEDD File Offset: 0x0017B0DD
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return Mathf.Max((180000 - (GenTicks.TicksGame - this.receivedTick)).TicksToDays(), 0f);
			}
		}

		
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return GenTicks.TicksGame - this.receivedTick < 180000 && !pawn.IsQuestLodger();
		}

		
		public RoyalTitle()
		{
		}

		
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
		}

		
		public void RoyalTitleTick(Pawn pawn)
		{
			if (pawn.IsHashIntervalTick(833) && this.conceited && pawn.Spawned && pawn.IsFreeColonist && (!pawn.IsQuestLodger() || pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				pawn.royalty.IssueDecree(false, null);
			}
		}

		
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		
		public Faction faction;

		
		public RoyalTitleDef def;

		
		public int receivedTick = -1;

		
		public bool wasInherited;

		
		public bool conceited;

		
		private const int DecreeCheckInterval = 833;

		
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
