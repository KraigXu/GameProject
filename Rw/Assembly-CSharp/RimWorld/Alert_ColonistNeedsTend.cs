using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEB RID: 3563
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x0600565D RID: 22109 RVA: 0x001CA250 File Offset: 0x001C8450
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F65 RID: 3941
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

		// Token: 0x0600565F RID: 22111 RVA: 0x001CA310 File Offset: 0x001C8510
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.NeedingColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistNeedsTreatmentDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x001CA398 File Offset: 0x001C8598
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		// Token: 0x04002F2A RID: 12074
		private List<Pawn> needingColonistsResult = new List<Pawn>();
	}
}
