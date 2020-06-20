using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEE RID: 3566
	public class Alert_Heatstroke : Alert
	{
		// Token: 0x06005669 RID: 22121 RVA: 0x001CA629 File Offset: 0x001C8829
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x0600566A RID: 22122 RVA: 0x001CA658 File Offset: 0x001C8858
		private List<Pawn> HeatstrokePawns
		{
			get
			{
				this.heatstrokePawnsResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.health != null && !pawn.RaceProps.Animal && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, true) != null)
					{
						this.heatstrokePawnsResult.Add(pawn);
					}
				}
				return this.heatstrokePawnsResult;
			}
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x001CA6D4 File Offset: 0x001C88D4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x001CA764 File Offset: 0x001C8964
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		// Token: 0x04002F2D RID: 12077
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();
	}
}
