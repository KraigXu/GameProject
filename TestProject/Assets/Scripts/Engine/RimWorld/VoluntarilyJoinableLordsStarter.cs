using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAA RID: 2730
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x0600409C RID: 16540 RVA: 0x0015A27B File Offset: 0x0015847B
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0015A295 File Offset: 0x00158495
		public bool TryStartMarriageCeremony(Pawn firstFiance, Pawn secondFiance)
		{
			if (!GatheringDefOf.MarriageCeremony.CanExecute(firstFiance.Map, firstFiance, true))
			{
				return false;
			}
			GatheringDefOf.MarriageCeremony.Worker.TryExecute(firstFiance.Map, firstFiance);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0015A2D5 File Offset: 0x001584D5
		public bool TryStartReigningSpeech(Pawn pawn)
		{
			if (!GatheringDefOf.ThroneSpeech.CanExecute(pawn.Map, pawn, true))
			{
				return false;
			}
			GatheringDefOf.ThroneSpeech.Worker.TryExecute(pawn.Map, pawn);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0015A318 File Offset: 0x00158518
		public bool TryStartRandomGathering(bool forceStart = false)
		{
			VoluntarilyJoinableLordsStarter.tmpGatherings.Clear();
			foreach (GatheringDef gatheringDef in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				if (gatheringDef.IsRandomSelectable && gatheringDef.CanExecute(this.map, null, forceStart))
				{
					VoluntarilyJoinableLordsStarter.tmpGatherings.Add(gatheringDef);
				}
			}
			GatheringDef gatheringDef2;
			return VoluntarilyJoinableLordsStarter.tmpGatherings.TryRandomElementByWeight((GatheringDef def) => def.randomSelectionWeight, out gatheringDef2) && this.TryStartGathering(gatheringDef2);
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0015A3C8 File Offset: 0x001585C8
		public bool TryStartGathering(GatheringDef gatheringDef)
		{
			if (!gatheringDef.Worker.TryExecute(this.map, null))
			{
				return false;
			}
			this.lastLordStartTick = Find.TickManager.TicksGame;
			this.startRandomGatheringASAP = false;
			return true;
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0015A3F8 File Offset: 0x001585F8
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0015A400 File Offset: 0x00158600
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0015A428 File Offset: 0x00158628
		private void Tick_TryStartRandomGathering()
		{
			if (!this.map.IsPlayerHome)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if (Rand.MTBEventOccurs(40f, 60000f, 5000f))
				{
					this.startRandomGatheringASAP = true;
				}
				if (this.startRandomGatheringASAP && Find.TickManager.TicksGame - this.lastLordStartTick >= 600000)
				{
					this.TryStartRandomGathering(false);
				}
			}
		}

		// Token: 0x0400257A RID: 9594
		private Map map;

		// Token: 0x0400257B RID: 9595
		private int lastLordStartTick = -999999;

		// Token: 0x0400257C RID: 9596
		private bool startRandomGatheringASAP;

		// Token: 0x0400257D RID: 9597
		private const int CheckStartGatheringIntervalTicks = 5000;

		// Token: 0x0400257E RID: 9598
		private const float StartGatheringMTBDays = 40f;

		// Token: 0x0400257F RID: 9599
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
