using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		
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

		
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		
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

		
		private Map map;

		
		private int lastLordStartTick = -999999;

		
		private bool startRandomGatheringASAP;

		
		private const int CheckStartGatheringIntervalTicks = 5000;

		
		private const float StartGatheringMTBDays = 40f;

		
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
