using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_HypothermicAnimals : Alert
	{
		
		
		private List<Pawn> HypothermicAnimals
		{
			get
			{
				this.hypothermicAnimalsResult.Clear();
				List<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				for (int i = 0; i < allMaps_Spawned.Count; i++)
				{
					if (allMaps_Spawned[i].RaceProps.Animal && allMaps_Spawned[i].Faction == null && allMaps_Spawned[i].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null)
					{
						this.hypothermicAnimalsResult.Add(allMaps_Spawned[i]);
					}
				}
				return this.hypothermicAnimalsResult;
			}
		}

		
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Debug alert:\n\nThese wild animals are hypothermic. This may indicate a bug (but it may not, if the animals are trapped or in some other wierd but legitimate situation):");
			foreach (Pawn pawn in this.HypothermicAnimals)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"    ",
					pawn,
					" at ",
					pawn.Position
				}));
			}
			return stringBuilder.ToString();
		}

		
		public override AlertReport GetReport()
		{
			if (!Prefs.DevMode)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.HypothermicAnimals);
		}

		
		private List<Pawn> hypothermicAnimalsResult = new List<Pawn>();
	}
}
