using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001139 RID: 4409
	public class QuestNode_GetRandomByCurve : QuestNode
	{
		// Token: 0x06006704 RID: 26372 RVA: 0x0024141F File Offset: 0x0023F61F
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x00241429 File Offset: 0x0023F629
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x00241438 File Offset: 0x0023F638
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

		// Token: 0x04003F2D RID: 16173
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F2E RID: 16174
		public SlateRef<SimpleCurve> curve;

		// Token: 0x04003F2F RID: 16175
		public SlateRef<bool> roundRandom;

		// Token: 0x04003F30 RID: 16176
		public SlateRef<float?> min;

		// Token: 0x04003F31 RID: 16177
		public SlateRef<float?> max;
	}
}
