using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_Exhaustion : Alert
	{
		
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		
		private List<Pawn> ExhaustedColonists
		{
			get
			{
				this.exhaustedColonistsResult.Clear();
				List<Pawn> allMaps_FreeColonistsSpawned = PawnsFinder.AllMaps_FreeColonistsSpawned;
				for (int i = 0; i < allMaps_FreeColonistsSpawned.Count; i++)
				{
					if (allMaps_FreeColonistsSpawned[i].needs.rest != null && allMaps_FreeColonistsSpawned[i].needs.rest.CurCategory == RestCategory.Exhausted)
					{
						this.exhaustedColonistsResult.Add(allMaps_FreeColonistsSpawned[i]);
					}
				}
				return this.exhaustedColonistsResult;
			}
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ExhaustionDesc".Translate(stringBuilder.ToString());
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}

		
		private List<Pawn> exhaustedColonistsResult = new List<Pawn>();
	}
}
