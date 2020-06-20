using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001005 RID: 4101
	public class StatPart_Mood : StatPart
	{
		// Token: 0x06006232 RID: 25138 RVA: 0x00220D89 File Offset: 0x0021EF89
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x00220D9C File Offset: 0x0021EF9C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.FactorFromMood(pawn);
				}
			}
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x00220DD8 File Offset: 0x0021EFD8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_MoodMultiplier".Translate(pawn.needs.mood.CurLevel.ToStringPercent()) + ": x" + this.FactorFromMood(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x00220E4D File Offset: 0x0021F04D
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x00220E5D File Offset: 0x0021F05D
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}

		// Token: 0x04003BE1 RID: 15329
		private SimpleCurve factorFromMoodCurve;
	}
}
