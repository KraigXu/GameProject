using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E08 RID: 3592
	public class Alert_UnusableMeditationFocus : Alert
	{
		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x060056C9 RID: 22217 RVA: 0x001CC6D4 File Offset: 0x001CA8D4
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

		// Token: 0x060056CA RID: 22218 RVA: 0x001CC890 File Offset: 0x001CAA90
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x001CC8C3 File Offset: 0x001CAAC3
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x001CC8E4 File Offset: 0x001CAAE4
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04002F47 RID: 12103
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04002F48 RID: 12104
		private List<string> pawnEntries = new List<string>();
	}
}
