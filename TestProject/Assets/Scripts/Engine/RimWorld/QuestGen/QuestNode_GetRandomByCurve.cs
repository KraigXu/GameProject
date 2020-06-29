using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetRandomByCurve : QuestNode
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
			float num = Rand.ByCurve(this.curve.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			if (this.min.GetValue(slate) != null)
			{
				num = Mathf.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Mathf.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<SimpleCurve> curve;

		
		public SlateRef<bool> roundRandom;

		
		public SlateRef<float?> min;

		
		public SlateRef<float?> max;
	}
}
