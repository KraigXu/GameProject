using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100B RID: 4107
	public class StatPart_Resting : StatPart
	{
		// Token: 0x0600624E RID: 25166 RVA: 0x0022155C File Offset: 0x0021F75C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.RestingMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x00221590 File Offset: 0x0021F790
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Resting".Translate() + ": x" + this.RestingMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x002215E4 File Offset: 0x0021F7E4
		private float RestingMultiplier(Pawn pawn)
		{
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed) || (pawn.IsCaravanMember() && !pawn.GetCaravan().pather.MovingNow) || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x04003BF7 RID: 15351
		public float factor = 1f;
	}
}
