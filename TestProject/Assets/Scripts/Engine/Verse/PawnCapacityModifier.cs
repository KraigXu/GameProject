using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B9 RID: 185
	public class PawnCapacityModifier
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001B717 File Offset: 0x00019917
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001B73B File Offset: 0x0001993B
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true));
		}

		// Token: 0x040003DC RID: 988
		public PawnCapacityDef capacity;

		// Token: 0x040003DD RID: 989
		public float offset;

		// Token: 0x040003DE RID: 990
		public float setMax = 999f;

		// Token: 0x040003DF RID: 991
		public float postFactor = 1f;

		// Token: 0x040003E0 RID: 992
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x040003E1 RID: 993
		public StatDef setMaxCurveEvaluateStat;
	}
}
