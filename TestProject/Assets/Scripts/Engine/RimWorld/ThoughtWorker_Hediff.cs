using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082F RID: 2095
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x0600346D RID: 13421 RVA: 0x0011FD04 File Offset: 0x0011DF04
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(Mathf.Min(new int[]
			{
				firstHediffOfDef.CurStageIndex,
				firstHediffOfDef.def.stages.Count - 1,
				this.def.stages.Count - 1
			}));
		}
	}
}
