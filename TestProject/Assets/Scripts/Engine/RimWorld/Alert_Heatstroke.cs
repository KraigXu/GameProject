using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_Heatstroke : Alert
	{
		
		public Alert_Heatstroke()
		{
			this.defaultLabel = "AlertHeatstroke".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		
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

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.HeatstrokePawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("AlertHeatstrokeDesc".Translate(), stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HeatstrokePawns);
		}

		
		private List<Pawn> heatstrokePawnsResult = new List<Pawn>();
	}
}
