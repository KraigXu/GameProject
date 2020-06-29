using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetPawnCountByPointsWeighted : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<int> challengeRating;

		
		public SlateRef<int> maxCountOneStar;

		
		public SlateRef<int> maxCountTwoStar;

		
		public SlateRef<int> maxCountThreeStar;

		
		public SlateRef<SimpleCurve> pointsCurve;

		
		public SlateRef<SimpleCurve> chancesCurve;

		
		public SlateRef<bool> roundRandom;
	}
}
