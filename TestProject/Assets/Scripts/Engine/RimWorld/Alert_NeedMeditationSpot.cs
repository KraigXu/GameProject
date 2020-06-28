using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E07 RID: 3591
	public class Alert_NeedMeditationSpot : Alert
	{
		// Token: 0x17000F7C RID: 3964
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

		// Token: 0x060056C6 RID: 22214 RVA: 0x001CC664 File Offset: 0x001CA864
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x001CC697 File Offset: 0x001CA897
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x001CC6B8 File Offset: 0x001CA8B8
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04002F45 RID: 12101
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04002F46 RID: 12102
		private List<string> pawnNames = new List<string>();
	}
}
