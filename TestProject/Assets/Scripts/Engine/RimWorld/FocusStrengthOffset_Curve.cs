using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		
		protected abstract float SourceValue(Thing parent);

		
		
		protected abstract string ExplanationKey { get; }

		
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		
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

		
		public SimpleCurve curve;
	}
}
