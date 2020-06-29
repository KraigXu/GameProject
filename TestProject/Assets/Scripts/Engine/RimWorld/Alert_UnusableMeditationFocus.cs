using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_UnusableMeditationFocus : Alert
	{
		
		
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnEntries.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.timetable != null && pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Meditate && pawn.psychicEntropy.IsCurrentlyMeditating && !MeditationFocusDefOf.Natural.CanPawnUse(pawn))
					{
						JobDriver_Meditate jobDriver_Meditate = pawn.jobs.curDriver as JobDriver_Meditate;
						if (jobDriver_Meditate != null && !(jobDriver_Meditate.Focus != null) && !(jobDriver_Meditate is JobDriver_Reign))
						{
							foreach (Thing thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, MeditationUtility.FocusObjectSearchRadius, false))
							{
								if (thing.def == ThingDefOf.Plant_TreeAnima || thing.def == ThingDefOf.AnimusStone || thing.def == ThingDefOf.NatureShrine_Small || thing.def == ThingDefOf.NatureShrine_Large)
								{
									this.targets.Add(pawn);
									this.pawnEntries.Add(pawn.LabelShort + " (" + thing.LabelShort + ")");
									break;
								}
							}
						}
					}
				}
				return this.targets;
			}
		}

		
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
		}

		
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		
		private List<string> pawnEntries = new List<string>();
	}
}
