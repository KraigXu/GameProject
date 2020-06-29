using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_ColonistNeedsTend : Alert
	{
		
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x001CA280 File Offset: 0x001C8480
		private List<Pawn> NeedingColonists
		{
			get
			{
				this.needingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.health.HasHediffsNeedingTendByPlayer(true))
					{
						Building_Bed building_Bed = pawn.CurrentBed();
						if ((building_Bed == null || !building_Bed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
						{
							this.needingColonistsResult.Add(pawn);
						}
					}
				}
				return this.needingColonistsResult;
			}
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistNeedsTreatmentDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		
		private List<Pawn> needingColonistsResult = new List<Pawn>();
	}
}
