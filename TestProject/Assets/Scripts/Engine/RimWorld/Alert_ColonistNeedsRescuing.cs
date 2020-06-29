using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		
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

		
		public static bool NeedsRescue(Pawn p)
		{
			return p.Downed && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker) && (p.jobs.jobQueue == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}

		
		public override string GetLabel()
		{
			if (this.ColonistsNeedingRescue.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ColonistsNeedingRescue)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
