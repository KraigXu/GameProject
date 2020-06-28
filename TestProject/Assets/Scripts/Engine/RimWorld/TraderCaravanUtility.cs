using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DCA RID: 3530
	public static class TraderCaravanUtility
	{
		// Token: 0x060055A6 RID: 21926 RVA: 0x001C7040 File Offset: 0x001C5240
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			if (p.kindDef == PawnKindDefOf.Slave)
			{
				return TraderCaravanRole.Chattel;
			}
			if (p.kindDef.trader)
			{
				return TraderCaravanRole.Trader;
			}
			if (p.kindDef.RaceProps.packAnimal && p.inventory.innerContainer.Any)
			{
				return TraderCaravanRole.Carrier;
			}
			if (p.RaceProps.Animal)
			{
				return TraderCaravanRole.Chattel;
			}
			return TraderCaravanRole.Guard;
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x001C70A4 File Offset: 0x001C52A4
		public static Pawn FindTrader(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].GetTraderCaravanRole() == TraderCaravanRole.Trader)
				{
					return lord.ownedPawns[i];
				}
			}
			return null;
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x001C70E9 File Offset: 0x001C52E9
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
