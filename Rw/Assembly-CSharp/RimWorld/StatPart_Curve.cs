using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFA RID: 4090
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x06006203 RID: 25091
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06006204 RID: 25092
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x06006205 RID: 25093
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x06006206 RID: 25094 RVA: 0x0022061E File Offset: 0x0021E81E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x00220649 File Offset: 0x0021E849
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				return this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			return null;
		}

		// Token: 0x04003BD9 RID: 15321
		protected SimpleCurve curve;
	}
}
