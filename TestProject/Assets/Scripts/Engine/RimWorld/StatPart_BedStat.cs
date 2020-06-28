using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF6 RID: 4086
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x060061F7 RID: 25079 RVA: 0x002203D0 File Offset: 0x0021E5D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.BedMultiplier(pawn);
				}
			}
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x00220404 File Offset: 0x0021E604
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x0022045E File Offset: 0x0021E65E
		private float BedMultiplier(Pawn pawn)
		{
			if (pawn.InBed())
			{
				return pawn.CurrentBed().GetStatValue(this.stat, true);
			}
			if (pawn.InCaravanBed())
			{
				return pawn.CurrentCaravanBed().GetStatValue(this.stat, true);
			}
			return 1f;
		}

		// Token: 0x04003BD8 RID: 15320
		private StatDef stat;
	}
}
