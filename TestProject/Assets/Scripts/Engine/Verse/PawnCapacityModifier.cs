using System;
using RimWorld;

namespace Verse
{
	
	public class PawnCapacityModifier
	{
		
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001B717 File Offset: 0x00019917
		public bool SetMaxDefined
		{
			get
			{
				return this.setMax != 999f || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true));
		}

		
		public PawnCapacityDef capacity;

		
		public float offset;

		
		public float setMax = 999f;

		
		public float postFactor = 1f;

		
		public SimpleCurve setMaxCurveOverride;

		
		public StatDef setMaxCurveEvaluateStat;
	}
}
