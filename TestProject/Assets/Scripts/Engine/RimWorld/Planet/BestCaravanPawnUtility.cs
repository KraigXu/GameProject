using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001225 RID: 4645
	public static class BestCaravanPawnUtility
	{
		// Token: 0x06006BC6 RID: 27590 RVA: 0x002598B7 File Offset: 0x00257AB7
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.NegotiationAbility, null);
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x002598C8 File Offset: 0x00257AC8
		public static Pawn FindBestNegotiator(Caravan caravan, Faction negotiatingWith = null, TraderKindDef trader = null)
		{
			Predicate<Pawn> pawnValidator = null;
			if (negotiatingWith != null)
			{
				pawnValidator = ((Pawn p) => p.CanTradeWith(negotiatingWith, trader));
			}
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement, pawnValidator);
		}

		// Token: 0x06006BC8 RID: 27592 RVA: 0x0025990C File Offset: 0x00257B0C
		public static Pawn FindBestEntertainingPawnFor(Caravan caravan, Pawn forPawn)
		{
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn2 = caravan.pawns[i];
				if (pawn2 != forPawn && pawn2.RaceProps.Humanlike && !pawn2.Dead && !pawn2.Downed && !pawn2.InMentalState && pawn2.IsPrisoner == forPawn.IsPrisoner && !StatDefOf.SocialImpact.Worker.IsDisabledFor(pawn2))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06006BC9 RID: 27593 RVA: 0x002599AC File Offset: 0x00257BAC
		public static Pawn FindPawnWithBestStat(Caravan caravan, StatDef stat, Predicate<Pawn> pawnValidator = null)
		{
			Pawn pawn = null;
			float num = -1f;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (BestCaravanPawnUtility.IsConsciousOwner(pawn2, caravan) && !stat.Worker.IsDisabledFor(pawn2) && (pawnValidator == null || pawnValidator(pawn2)))
				{
					float statValue = pawn2.GetStatValue(stat, true);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06006BCA RID: 27594 RVA: 0x00259A22 File Offset: 0x00257C22
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
