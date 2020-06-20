using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100A RID: 4106
	public class StatPart_Rest : StatPart
	{
		// Token: 0x0600624A RID: 25162 RVA: 0x0022142C File Offset: 0x0021F62C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					val *= this.RestMultiplier(pawn.needs.rest.CurCategory);
				}
			}
		}

		// Token: 0x0600624B RID: 25163 RVA: 0x0022147C File Offset: 0x0021F67C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					return pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600624C RID: 25164 RVA: 0x002214EB File Offset: 0x0021F6EB
		private float RestMultiplier(RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return this.factorRested;
			case RestCategory.Tired:
				return this.factorTired;
			case RestCategory.VeryTired:
				return this.factorVeryTired;
			case RestCategory.Exhausted:
				return this.factorExhausted;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04003BF3 RID: 15347
		private float factorExhausted = 1f;

		// Token: 0x04003BF4 RID: 15348
		private float factorVeryTired = 1f;

		// Token: 0x04003BF5 RID: 15349
		private float factorTired = 1f;

		// Token: 0x04003BF6 RID: 15350
		private float factorRested = 1f;
	}
}
