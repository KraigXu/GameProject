using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200079A RID: 1946
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060032B0 RID: 12976 RVA: 0x00119DA7 File Offset: 0x00117FA7
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x00119DAE File Offset: 0x00117FAE
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00119DB8 File Offset: 0x00117FB8
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			Pawn pawn2;
			bool result = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
			target = pawn2;
			return result;
		}
	}
}
