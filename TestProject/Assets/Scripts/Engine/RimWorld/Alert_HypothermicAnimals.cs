using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDE RID: 3550
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005625 RID: 22053 RVA: 0x001C8D00 File Offset: 0x001C6F00
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

		// Token: 0x06005626 RID: 22054 RVA: 0x001C8D86 File Offset: 0x001C6F86
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x001C8D90 File Offset: 0x001C6F90
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

		// Token: 0x06005628 RID: 22056 RVA: 0x001C8E30 File Offset: 0x001C7030
		public override AlertReport GetReport()
		{
			if (!Prefs.DevMode)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.HypothermicAnimals);
		}

		// Token: 0x04002F1B RID: 12059
		private List<Pawn> hypothermicAnimalsResult = new List<Pawn>();
	}
}
