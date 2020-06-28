using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100D RID: 4109
	public class StatPart_Slaughtered : StatPart
	{
		// Token: 0x06006256 RID: 25174 RVA: 0x00221704 File Offset: 0x0021F904
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.CanApply(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x0022171A File Offset: 0x0021F91A
		public override string ExplanationPart(StatRequest req)
		{
			if (this.CanApply(req))
			{
				return "StatsReport_HasHediffExplanation".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x00221750 File Offset: 0x0021F950
		private bool CanApply(StatRequest req)
		{
			Pawn pawn;
			if (!req.HasThing || (pawn = (req.Thing as Pawn)) == null)
			{
				return false;
			}
			if (!pawn.def.race.Animal || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.ExecutionCut, false))
			{
				return false;
			}
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (typeof(Hediff_Injury).IsAssignableFrom(hediffs[i].def.hediffClass) && !hediffs[i].IsPermanent())
				{
					num++;
					if (num > 1)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04003BFB RID: 15355
		private float factor;
	}
}
