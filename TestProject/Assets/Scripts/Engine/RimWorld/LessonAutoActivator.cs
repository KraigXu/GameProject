using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0D RID: 3853
	public static class LessonAutoActivator
	{
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06005E5B RID: 24155 RVA: 0x0020A936 File Offset: 0x00208B36
		private static float SecondsSinceLesson
		{
			get
			{
				return LessonAutoActivator.timeSinceLastLesson;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06005E5C RID: 24156 RVA: 0x0020A93D File Offset: 0x00208B3D
		private static float RelaxDesire
		{
			get
			{
				return 100f - LessonAutoActivator.SecondsSinceLesson * 0.111111112f;
			}
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x0020A950 File Offset: 0x00208B50
		public static void Reset()
		{
			LessonAutoActivator.alertingConcepts.Clear();
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x0020A95C File Offset: 0x00208B5C
		public static void TeachOpportunity(ConceptDef conc, OpportunityType opp)
		{
			LessonAutoActivator.TeachOpportunity(conc, null, opp);
		}

		// Token: 0x06005E5F RID: 24159 RVA: 0x0020A968 File Offset: 0x00208B68
		public static void TeachOpportunity(ConceptDef conc, Thing subject, OpportunityType opp)
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return;
			}
			float value = 999f;
			switch (opp)
			{
			case OpportunityType.GoodToKnow:
				value = 60f;
				break;
			case OpportunityType.Important:
				value = 80f;
				break;
			case OpportunityType.Critical:
				value = 100f;
				break;
			default:
				Log.Error("Unknown need", false);
				break;
			}
			LessonAutoActivator.opportunities[conc] = value;
			if (opp >= OpportunityType.Important || Find.Tutor.learningReadout.ActiveConceptsCount < 4)
			{
				LessonAutoActivator.TryInitiateLesson(conc);
			}
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x0020A9EA File Offset: 0x00208BEA
		public static void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				LessonAutoActivator.opportunities[conc] = 0f;
			}
		}

		// Token: 0x06005E61 RID: 24161 RVA: 0x0020AA04 File Offset: 0x00208C04
		public static void LessonAutoActivatorUpdate()
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || Current.Game == null || Find.Tutor.learningReadout.ShowAllMode)
			{
				return;
			}
			LessonAutoActivator.timeSinceLastLesson += RealTime.realDeltaTime;
			if (Current.ProgramState == ProgramState.Playing && (Time.timeSinceLevelLoad < 8f || Find.WindowStack.SecondsSinceClosedGameStartDialog < 8f || Find.TickManager.NotPlaying))
			{
				return;
			}
			for (int i = LessonAutoActivator.alertingConcepts.Count - 1; i >= 0; i--)
			{
				if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[i]))
				{
					LessonAutoActivator.alertingConcepts.RemoveAt(i);
				}
			}
			if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
			{
				for (int j = 0; j < DefDatabase<ConceptDef>.AllDefsListForReading.Count; j++)
				{
					ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[j];
					if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
					{
						float num = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
						num -= 0.00015f * Time.deltaTime * 15f;
						if (num < 0f)
						{
							num = 0f;
						}
						PlayerKnowledgeDatabase.SetKnowledge(conceptDef, num);
						if (conceptDef.opportunityDecays)
						{
							float num2 = LessonAutoActivator.GetOpportunity(conceptDef);
							num2 -= 0.4f * Time.deltaTime * 15f;
							if (num2 < 0f)
							{
								num2 = 0f;
							}
							LessonAutoActivator.opportunities[conceptDef] = num2;
						}
					}
				}
				if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
				{
					ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
					if (conceptDef2 != null)
					{
						float desire = LessonAutoActivator.GetDesire(conceptDef2);
						if (desire > 0.1f && LessonAutoActivator.RelaxDesire < desire)
						{
							LessonAutoActivator.TryInitiateLesson(conceptDef2);
							return;
						}
					}
				}
				else
				{
					LessonAutoActivator.SetLastLessonTimeToNow();
				}
			}
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x0020ABAC File Offset: 0x00208DAC
		private static ConceptDef MostDesiredConcept()
		{
			float num = -9999f;
			ConceptDef result = null;
			List<ConceptDef> allDefsListForReading = DefDatabase<ConceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num && (!conceptDef.needsOpportunity || LessonAutoActivator.GetOpportunity(conceptDef) >= 0.1f) && PlayerKnowledgeDatabase.GetKnowledge(conceptDef) <= 0.15f)
				{
					num = desire;
					result = conceptDef;
				}
			}
			return result;
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x0020AC1C File Offset: 0x00208E1C
		private static float GetDesire(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return 0f;
			}
			if (Find.Tutor.learningReadout.IsActive(conc))
			{
				return 0f;
			}
			if (Current.ProgramState != conc.gameMode)
			{
				return 0f;
			}
			if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.1f)
			{
				return 0f;
			}
			return (0f + conc.priority + LessonAutoActivator.GetOpportunity(conc) / 100f * 60f) * (1f - PlayerKnowledgeDatabase.GetKnowledge(conc));
		}

		// Token: 0x06005E64 RID: 24164 RVA: 0x0020ACAC File Offset: 0x00208EAC
		private static float GetOpportunity(ConceptDef conc)
		{
			float result;
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out result))
			{
				return result;
			}
			LessonAutoActivator.opportunities[conc] = 0f;
			return 0f;
		}

		// Token: 0x06005E65 RID: 24165 RVA: 0x0020ACDF File Offset: 0x00208EDF
		private static void TryInitiateLesson(ConceptDef conc)
		{
			if (Find.Tutor.learningReadout.TryActivateConcept(conc))
			{
				LessonAutoActivator.SetLastLessonTimeToNow();
			}
		}

		// Token: 0x06005E66 RID: 24166 RVA: 0x0020ACF8 File Offset: 0x00208EF8
		private static void SetLastLessonTimeToNow()
		{
			LessonAutoActivator.timeSinceLastLesson = 0f;
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x0020AD04 File Offset: 0x00208F04
		public static void Notify_TutorialEnding()
		{
			LessonAutoActivator.SetLastLessonTimeToNow();
		}

		// Token: 0x06005E68 RID: 24168 RVA: 0x0020AD0C File Offset: 0x00208F0C
		public static string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("RelaxDesire: " + LessonAutoActivator.RelaxDesire);
			foreach (ConceptDef conceptDef in from co in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(co) descending
			select co)
			{
				if (PlayerKnowledgeDatabase.IsComplete(conceptDef))
				{
					stringBuilder.AppendLine(conceptDef.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						conceptDef.defName,
						"\n   know ",
						PlayerKnowledgeDatabase.GetKnowledge(conceptDef).ToString("F3"),
						"\n   need ",
						LessonAutoActivator.opportunities[conceptDef].ToString("F3"),
						"\n   des ",
						LessonAutoActivator.GetDesire(conceptDef).ToString("F3")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005E69 RID: 24169 RVA: 0x0020AE40 File Offset: 0x00209040
		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson((from def in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(def) descending
			select def).First<ConceptDef>());
		}

		// Token: 0x0400335F RID: 13151
		private static Dictionary<ConceptDef, float> opportunities = new Dictionary<ConceptDef, float>();

		// Token: 0x04003360 RID: 13152
		private static float timeSinceLastLesson = 10000f;

		// Token: 0x04003361 RID: 13153
		private static List<ConceptDef> alertingConcepts = new List<ConceptDef>();

		// Token: 0x04003362 RID: 13154
		private const float MapStartGracePeriod = 8f;

		// Token: 0x04003363 RID: 13155
		private const float KnowledgeDecayRate = 0.00015f;

		// Token: 0x04003364 RID: 13156
		private const float OpportunityDecayRate = 0.4f;

		// Token: 0x04003365 RID: 13157
		private const float OpportunityMaxDesireAdd = 60f;

		// Token: 0x04003366 RID: 13158
		private const int CheckInterval = 15;

		// Token: 0x04003367 RID: 13159
		private const float MaxLessonInterval = 900f;
	}
}
