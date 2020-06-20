using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001134 RID: 4404
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		// Token: 0x060066F0 RID: 26352 RVA: 0x00240EC0 File Offset: 0x0023F0C0
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x00240ECA File Offset: 0x0023F0CA
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x00240ED8 File Offset: 0x0023F0D8
		private void SetVars(Slate slate)
		{
			float x = slate.Get<float>("points", 0f, false);
			float num = this.pointsCurve.GetValue(slate).Evaluate(x);
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			int num2;
			if (this.challengeRating.TryGetValue(slate, out num2))
			{
				if (num2 == 1)
				{
					num = Mathf.Min(num, (float)this.maxCountOneStar.GetValue(slate));
				}
				else if (num2 == 2)
				{
					num = Mathf.Min(num, (float)this.maxCountTwoStar.GetValue(slate));
				}
				else
				{
					num = Mathf.Min(num, (float)this.maxCountThreeStar.GetValue(slate));
				}
			}
			SimpleCurve value = this.chancesCurve.GetValue(slate);
			for (int i = value.Points.Count - 1; i >= 0; i--)
			{
				if (value.Points[i].x <= num)
				{
					value.Points.Insert(i + 1, new CurvePoint(num + 1f, 0f));
					break;
				}
				value.Points[i] = new CurvePoint(0f, 0f);
			}
			float num3 = Rand.ByCurve(value);
			if (this.roundRandom.GetValue(slate))
			{
				num3 = (float)GenMath.RoundRandom(num3);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), Mathf.Clamp(num3, 1f, num), false);
		}

		// Token: 0x04003F16 RID: 16150
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F17 RID: 16151
		public SlateRef<int> challengeRating;

		// Token: 0x04003F18 RID: 16152
		public SlateRef<int> maxCountOneStar;

		// Token: 0x04003F19 RID: 16153
		public SlateRef<int> maxCountTwoStar;

		// Token: 0x04003F1A RID: 16154
		public SlateRef<int> maxCountThreeStar;

		// Token: 0x04003F1B RID: 16155
		public SlateRef<SimpleCurve> pointsCurve;

		// Token: 0x04003F1C RID: 16156
		public SlateRef<SimpleCurve> chancesCurve;

		// Token: 0x04003F1D RID: 16157
		public SlateRef<bool> roundRandom;
	}
}
