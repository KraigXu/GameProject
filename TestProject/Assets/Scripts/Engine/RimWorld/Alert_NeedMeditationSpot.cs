using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_NeedMeditationSpot : Alert
	{
		
		// (get) Token: 0x060056C5 RID: 22213 RVA: 0x001CC584 File Offset: 0x001CA784
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnNames.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					bool flag = false;
					for (int i = 0; i < pawn.timetable.times.Count; i++)
					{
						if (pawn.timetable.times[i] == TimeAssignmentDefOf.Meditate)
						{
							flag = true;
							break;
						}
					}
					if ((pawn.HasPsylink || flag) && !MeditationUtility.AllMeditationSpotCandidates(pawn, false).Any<LocalTargetInfo>())
					{
						this.targets.Add(pawn);
						this.pawnNames.Add(pawn.LabelShort);
					}
				}
				return this.targets;
			}
		}

		
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
		}

		
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
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

		
		private List<string> pawnNames = new List<string>();
	}
}
