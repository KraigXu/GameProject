using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A4 RID: 1956
	public class LordToil_StealCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x0011AF83 File Offset: 0x00119183
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Steal;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x0011AF8C File Offset: 0x0011918C
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing != null)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			return StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out target, pawn, alreadyTakenTargets);
		}
	}
}
