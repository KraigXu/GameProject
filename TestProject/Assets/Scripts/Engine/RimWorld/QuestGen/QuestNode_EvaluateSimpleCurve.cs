using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200111C RID: 4380
	public class QuestNode_EvaluateSimpleCurve : QuestNode
	{
		// Token: 0x06006685 RID: 26245 RVA: 0x0023E88D File Offset: 0x0023CA8D
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x0023E897 File Offset: 0x0023CA97
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x0023E8A4 File Offset: 0x0023CAA4
		private void SetVars(Slate slate)
		{
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04003EB0 RID: 16048
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003EB1 RID: 16049
		public SlateRef<SimpleCurve> curve;

		// Token: 0x04003EB2 RID: 16050
		public SlateRef<float> value;

		// Token: 0x04003EB3 RID: 16051
		public SlateRef<bool> roundRandom;
	}
}
