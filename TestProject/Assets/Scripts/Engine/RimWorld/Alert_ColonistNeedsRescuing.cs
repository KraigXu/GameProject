using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD7 RID: 3543
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06005607 RID: 22023 RVA: 0x001C85DC File Offset: 0x001C67DC
		private List<Pawn> ColonistsNeedingRescue
		{
			get
			{
				this.colonistsNeedingRescueResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
					{
						this.colonistsNeedingRescueResult.Add(pawn);
					}
				}
				return this.colonistsNeedingRescueResult;
			}
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x001C864C File Offset: 0x001C684C
		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x001C86B9 File Offset: 0x001C68B9
		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x001C86E8 File Offset: 0x001C68E8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x001C8770 File Offset: 0x001C6970
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		// Token: 0x04002F11 RID: 12049
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
