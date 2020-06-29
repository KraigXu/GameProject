using System;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_EvaluateSimpleCurve : QuestNode
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
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<SimpleCurve> curve;

		
		public SlateRef<float> value;

		
		public SlateRef<bool> roundRandom;
	}
}
