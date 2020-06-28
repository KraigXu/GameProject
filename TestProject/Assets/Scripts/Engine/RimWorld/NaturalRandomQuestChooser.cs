using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093A RID: 2362
	public static class NaturalRandomQuestChooser
	{
		// Token: 0x060037F2 RID: 14322 RVA: 0x0012C105 File Offset: 0x0012A305
		public static float PopulationIncreasingQuestChance()
		{
			return QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0012C118 File Offset: 0x0012A318
		public static QuestScriptDef ChooseNaturalRandomQuest(float points, IIncidentTarget target)
		{
			NaturalRandomQuestChooser.<>c__DisplayClass1_0 <>c__DisplayClass1_ = new NaturalRandomQuestChooser.<>c__DisplayClass1_0();
			<>c__DisplayClass1_.points = points;
			<>c__DisplayClass1_.target = target;
			bool flag = Rand.Chance(NaturalRandomQuestChooser.PopulationIncreasingQuestChance());
			QuestScriptDef result;
			if (<>c__DisplayClass1_.<ChooseNaturalRandomQuest>g__TryGetQuest|0(flag, out result))
			{
				return result;
			}
			QuestScriptDef result2;
			if (flag && <>c__DisplayClass1_.<ChooseNaturalRandomQuest>g__TryGetQuest|0(false, out result2))
			{
				return result2;
			}
			Log.Error("Couldn't find any random quest. points=" + <>c__DisplayClass1_.points, false);
			return null;
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x0012C180 File Offset: 0x0012A380
		public static float GetNaturalRandomSelectionWeight(QuestScriptDef quest, float points, StoryState storyState)
		{
			if (quest.rootSelectionWeight <= 0f || points < quest.rootMinPoints || StorytellerUtility.GetProgressScore(storyState.Target) < quest.rootMinProgressScore)
			{
				return 0f;
			}
			float num = quest.rootSelectionWeight;
			for (int i = 0; i < storyState.RecentRandomQuests.Count; i++)
			{
				if (storyState.RecentRandomQuests[i] == quest)
				{
					switch (i)
					{
					case 0:
						num *= 0.01f;
						break;
					case 1:
						num *= 0.3f;
						break;
					case 2:
						num *= 0.5f;
						break;
					case 3:
						num *= 0.7f;
						break;
					case 4:
						num *= 0.9f;
						break;
					}
				}
			}
			if (!quest.canGiveRoyalFavor && NaturalRandomQuestChooser.<GetNaturalRandomSelectionWeight>g__PlayerWantsRoyalFavorFromAnyFaction|2_0())
			{
				int num2 = (storyState.LastRoyalFavorQuestTick != -1) ? storyState.LastRoyalFavorQuestTick : 0;
				float x = (float)(Find.TickManager.TicksGame - num2) / 60000f;
				num *= QuestTuning.NonFavorQuestSelectionWeightFactorByDaysSinceFavorQuestCurve.Evaluate(x);
			}
			return num;
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0012C27C File Offset: 0x0012A47C
		public static float GetNaturalDecreeSelectionWeight(QuestScriptDef quest, StoryState storyState)
		{
			if (quest.decreeSelectionWeight <= 0f)
			{
				return 0f;
			}
			float num = quest.decreeSelectionWeight;
			for (int i = 0; i < storyState.RecentRandomDecrees.Count; i++)
			{
				if (storyState.RecentRandomDecrees[i] == quest)
				{
					switch (i)
					{
					case 0:
						num *= 0.01f;
						break;
					case 1:
						num *= 0.3f;
						break;
					case 2:
						num *= 0.5f;
						break;
					case 3:
						num *= 0.7f;
						break;
					case 4:
						num *= 0.9f;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x0012C318 File Offset: 0x0012A518
		public static float DebugTotalNaturalRandomSelectionWeight(QuestScriptDef quest, float points, IIncidentTarget target)
		{
			if (!quest.IsRootRandomSelected)
			{
				return 0f;
			}
			if (!quest.CanRun(points))
			{
				return 0f;
			}
			float naturalRandomSelectionWeight = NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(quest, points, target.StoryState);
			float num = QuestTuning.IncreasesPopQuestChanceByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
			return num * (quest.rootIncreasesPopulation ? naturalRandomSelectionWeight : 0f) + (1f - num) * ((!quest.rootIncreasesPopulation) ? naturalRandomSelectionWeight : 0f);
		}
	}
}
