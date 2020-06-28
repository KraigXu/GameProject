using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF4 RID: 4084
	public class StatPart_Age : StatPart
	{
		// Token: 0x060061EC RID: 25068 RVA: 0x00220108 File Offset: 0x0021E308
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					val *= this.AgeMultiplier(pawn);
				}
			}
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x00220144 File Offset: 0x0021E344
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_AgeMultiplier".Translate(pawn.ageTracker.AgeBiologicalYears) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x002201AE File Offset: 0x0021E3AE
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x002201D3 File Offset: 0x0021E3D3
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x04003BD5 RID: 15317
		private SimpleCurve curve;
	}
}
