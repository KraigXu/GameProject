using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD8 RID: 3544
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x0600560D RID: 22029 RVA: 0x001C8790 File Offset: 0x001C6990
		private List<Pawn> SickPawns
		{
			get
			{
				this.sickPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
					{
						Hediff hediff = pawn.health.hediffSet.hediffs[i];
						if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && !hediff.FullyImmune())
						{
							this.sickPawnsResult.Add(pawn);
							break;
						}
					}
				}
				return this.sickPawnsResult;
			}
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x001C8850 File Offset: 0x001C6A50
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x0600560F RID: 22031 RVA: 0x001C8864 File Offset: 0x001C6A64
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn pawn in this.SickPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.CurStage.lifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return "PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(stringBuilder.ToString());
			}
			return "PawnsWithLifeThreateningDiseaseDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x001C8990 File Offset: 0x001C6B90
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		// Token: 0x04002F12 RID: 12050
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
