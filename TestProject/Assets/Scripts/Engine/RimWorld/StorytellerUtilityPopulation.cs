using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A25 RID: 2597
	public static class StorytellerUtilityPopulation
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x00144F07 File Offset: 0x00143107
		private static StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x00144F13 File Offset: 0x00143113
		public static float PopulationIntent
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulation, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003D71 RID: 15729 RVA: 0x00144F33 File Offset: 0x00143133
		public static float PopulationIntentForQuest
		{
			get
			{
				return StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, StorytellerUtilityPopulation.AdjustedPopulationIncludingQuests, Find.StoryWatcher.watcherPopAdaptation.AdaptDays);
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003D72 RID: 15730 RVA: 0x00144F53 File Offset: 0x00143153
		public static float AdjustedPopulation
		{
			get
			{
				return 0f + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count<Pawn>() + (float)PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Count<Pawn>() * StorytellerUtilityPopulation.PopulationValue_Prisoner + (float)QuestUtility.TotalBorrowedColonistCount();
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x00144F80 File Offset: 0x00143180
		public static float AdjustedPopulationIncludingQuests
		{
			get
			{
				float num = StorytellerUtilityPopulation.AdjustedPopulation;
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (!questsListForReading[i].Historical && questsListForReading[i].IncreasesPopulation)
					{
						num += 1f;
					}
				}
				return num;
			}
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x00144FD4 File Offset: 0x001431D4
		private static float CalculatePopulationIntent(StorytellerDef def, float curPop, float popAdaptation)
		{
			float num = def.populationIntentFactorFromPopCurve.Evaluate(curPop);
			if (num > 0f)
			{
				num *= def.populationIntentFactorFromPopAdaptDaysCurve.Evaluate(popAdaptation);
			}
			return num;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00145008 File Offset: 0x00143208
		public static string DebugReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Population intent: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntent.ToString("F2"));
			stringBuilder.AppendLine("Population intent for quest: ".PadRight(40) + StorytellerUtilityPopulation.PopulationIntentForQuest.ToString("F2"));
			stringBuilder.AppendLine("Chance random quest increases population: ".PadRight(40) + NaturalRandomQuestChooser.PopulationIncreasingQuestChance().ToStringPercent());
			stringBuilder.AppendLine("Adjusted population: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Adjusted population including quests: ".PadRight(40) + StorytellerUtilityPopulation.AdjustedPopulation.ToString("F1"));
			stringBuilder.AppendLine("Pop adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherPopAdaptation.AdaptDays.ToString("F2"));
			return stringBuilder.ToString();
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00145120 File Offset: 0x00143320
		[DebugOutput]
		public static void PopulationIntents()
		{
			List<float> list = new List<float>();
			for (int i = 0; i < 30; i++)
			{
				list.Add((float)i);
			}
			List<float> list2 = new List<float>();
			for (int j = 0; j < 40; j += 2)
			{
				list2.Add((float)j);
			}
			DebugTables.MakeTablesDialog<float, float>(list2, (float ds) => "d-" + ds.ToString("F0"), list, (float rv) => rv.ToString("F2"), (float ds, float p) => StorytellerUtilityPopulation.CalculatePopulationIntent(StorytellerUtilityPopulation.StorytellerDef, p, (float)((int)ds)).ToString("F2"), "pop");
		}

		// Token: 0x040023E0 RID: 9184
		private static float PopulationValue_Prisoner = 0.5f;
	}
}
