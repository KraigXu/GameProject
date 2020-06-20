using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D23 RID: 3363
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		// Token: 0x060051CD RID: 20941
		protected abstract float SourceValue(Thing parent);

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060051CE RID: 20942
		protected abstract string ExplanationKey { get; }

		// Token: 0x060051CF RID: 20943 RVA: 0x001B5F33 File Offset: 0x001B4133
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x001B5F58 File Offset: 0x001B4158
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x001B5F8C File Offset: 0x001B418C
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x001B6028 File Offset: 0x001B4228
		public override float MaxOffset(bool forAbstract = false)
		{
			float num = 0f;
			for (int i = 0; i < this.curve.PointsCount; i++)
			{
				float y = this.curve[i].y;
				if (Mathf.Abs(y) > Mathf.Abs(num))
				{
					num = y;
				}
			}
			return num;
		}

		// Token: 0x04002D29 RID: 11561
		public SimpleCurve curve;
	}
}
