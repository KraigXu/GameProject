using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		
		public override void QuestPartTick()
		{
			if (this.requiredCount > 0 && Find.TickManager.TicksAbs % 500 == 0)
			{
				int num = 0;
				List<Pawn> allPawnsSpawned = this.mapParent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].def == this.race && allPawnsSpawned[i].Faction == null)
					{
						num++;
					}
				}
				if (num < this.requiredCount)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalPawnsNotAvailable));
				}
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		
		public ThingDef race;

		
		public int requiredCount;

		
		public MapParent mapParent;

		
		public string inSignalDecrement;

		
		public string outSignalPawnsNotAvailable;

		
		private const int CheckInterval = 500;
	}
}
