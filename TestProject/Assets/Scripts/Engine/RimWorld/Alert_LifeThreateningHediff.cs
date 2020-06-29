using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		
		
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

		
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		
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

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
