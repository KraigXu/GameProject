using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001140 RID: 4416
	public class QuestNode_GetRandomNegativeGameCondition : QuestNode
	{
		// Token: 0x0600671E RID: 26398 RVA: 0x0024183C File Offset: 0x0023FA3C
		public static void ResetStaticData()
		{
			QuestNode_GetRandomNegativeGameCondition.options = new List<QuestNode_GetRandomNegativeGameCondition.Option>
			{
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.VolcanicWinter, new FloatRange(10f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.WeatherController, new FloatRange(5f, 20f), 0.4f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.HeatWave, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ColdSnap, new FloatRange(4f, 8f), 1f, 1),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.ToxicFallout, new FloatRange(5f, 20f), 0.8f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicSuppression, new FloatRange(4f, 8f), 1.5f, 2),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.EMIField, new FloatRange(4f, 8f), 1.8f, 3),
				new QuestNode_GetRandomNegativeGameCondition.Option(GameConditionDefOf.PsychicDrone, new FloatRange(4f, 8f), 2f, 3)
			};
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x0024197B File Offset: 0x0023FB7B
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null && this.DoWork(slate);
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x00241995 File Offset: 0x0023FB95
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x002419A4 File Offset: 0x0023FBA4
		private bool DoWork(Slate slate)
		{
			QuestNode_GetRandomNegativeGameCondition.Option option;
			if (!(from x in QuestNode_GetRandomNegativeGameCondition.options
			where (!QuestGen.Working || x.challengeRating == QuestGen.quest.challengeRating) && this.PossibleNow(x.gameCondition, slate)
			select x).TryRandomElement(out option))
			{
				return false;
			}
			int var = (int)(option.durationDaysRange.RandomInRange * 60000f);
			slate.Set<GameConditionDef>(this.storeGameConditionAs.GetValue(slate), option.gameCondition, false);
			slate.Set<int>(this.storeGameConditionDurationAs.GetValue(slate), var, false);
			slate.Set<float>(this.storeGameConditionDifficultyAs.GetValue(slate), option.difficulty, false);
			return true;
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x00241A60 File Offset: 0x0023FC60
		private bool PossibleNow(GameConditionDef def, Slate slate)
		{
			if (def == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			if (map.gameConditionManager.ConditionIsActive(def))
			{
				return false;
			}
			IncidentDef incidentDef = null;
			List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].Worker is IncidentWorker_MakeGameCondition && allDefsListForReading[i].gameCondition == def)
				{
					incidentDef = allDefsListForReading[i];
					break;
				}
			}
			if (incidentDef != null)
			{
				if (Find.Storyteller.difficulty.difficulty < incidentDef.minDifficulty)
				{
					return false;
				}
				if (GenDate.DaysPassed < incidentDef.earliestDay)
				{
					return false;
				}
				if (incidentDef.Worker.FiredTooRecently(map))
				{
					return false;
				}
			}
			return (def != GameConditionDefOf.ColdSnap || IncidentWorker_ColdSnap.IsTemperatureAppropriate(map)) && (def != GameConditionDefOf.HeatWave || IncidentWorker_HeatWave.IsTemperatureAppropriate(map));
		}

		// Token: 0x04003F42 RID: 16194
		[NoTranslate]
		public SlateRef<string> storeGameConditionAs;

		// Token: 0x04003F43 RID: 16195
		[NoTranslate]
		public SlateRef<string> storeGameConditionDurationAs;

		// Token: 0x04003F44 RID: 16196
		[NoTranslate]
		public SlateRef<string> storeGameConditionDifficultyAs;

		// Token: 0x04003F45 RID: 16197
		private static List<QuestNode_GetRandomNegativeGameCondition.Option> options;

		// Token: 0x02001F3A RID: 7994
		private struct Option
		{
			// Token: 0x0600AC9F RID: 44191 RVA: 0x00321686 File Offset: 0x0031F886
			public Option(GameConditionDef gameCondition, FloatRange durationDaysRange, float difficulty, int challengeRating)
			{
				this.gameCondition = gameCondition;
				this.durationDaysRange = durationDaysRange;
				this.difficulty = difficulty;
				this.challengeRating = challengeRating;
			}

			// Token: 0x04007524 RID: 29988
			public GameConditionDef gameCondition;

			// Token: 0x04007525 RID: 29989
			public FloatRange durationDaysRange;

			// Token: 0x04007526 RID: 29990
			public float difficulty;

			// Token: 0x04007527 RID: 29991
			public int challengeRating;
		}
	}
}
