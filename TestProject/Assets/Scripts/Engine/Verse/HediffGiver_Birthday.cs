using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000280 RID: 640
	public class HediffGiver_Birthday : HediffGiver
	{
		// Token: 0x06001127 RID: 4391 RVA: 0x00060BD0 File Offset: 0x0005EDD0
		public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
		{
			HediffGiver_Birthday.addedHediffs.Clear();
			if (!base.TryApply(pawn, HediffGiver_Birthday.addedHediffs))
			{
				return;
			}
			if (this.averageSeverityPerDayBeforeGeneration != 0f)
			{
				float num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
				if (num < 0f)
				{
					Log.Error(string.Concat(new object[]
					{
						"daysPassed < 0, pawn=",
						pawn,
						", gotAtAge=",
						gotAtAge
					}), false);
					return;
				}
				for (int i = 0; i < HediffGiver_Birthday.addedHediffs.Count; i++)
				{
					this.SimulateSeverityChange(pawn, HediffGiver_Birthday.addedHediffs[i], num, tryNotToKillPawn);
				}
			}
			HediffGiver_Birthday.addedHediffs.Clear();
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00060C84 File Offset: 0x0005EE84
		private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
		{
			float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
			num *= Rand.Range(0.5f, 1.4f);
			num += hediff.def.initialSeverity;
			if (tryNotToKillPawn)
			{
				this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
			}
			hediff.Severity = num;
			pawn.health.Notify_HediffChanged(hediff);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00060CE4 File Offset: 0x0005EEE4
		private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
		{
			if (stages.NullOrEmpty<HediffStage>())
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < stages.Count; i++)
			{
				if (stages[i].lifeThreatening)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				if (num == 0)
				{
					severity = Mathf.Min(severity, stages[num].minSeverity);
					return;
				}
				severity = Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00060D68 File Offset: 0x0005EF68
		public float DebugChanceToHaveAtAge(Pawn pawn, int age)
		{
			float num = 1f;
			for (int i = 1; i <= age; i++)
			{
				float x = (float)i / pawn.RaceProps.lifeExpectancy;
				num *= 1f - this.ageFractionChanceCurve.Evaluate(x);
			}
			return 1f - num;
		}

		// Token: 0x04000C56 RID: 3158
		public SimpleCurve ageFractionChanceCurve;

		// Token: 0x04000C57 RID: 3159
		public float averageSeverityPerDayBeforeGeneration;

		// Token: 0x04000C58 RID: 3160
		private static List<Hediff> addedHediffs = new List<Hediff>();
	}
}
