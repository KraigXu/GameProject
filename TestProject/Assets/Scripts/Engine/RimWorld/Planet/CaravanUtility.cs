using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200123D RID: 4669
	public static class CaravanUtility
	{
		// Token: 0x06006CC2 RID: 27842 RVA: 0x0025F74A File Offset: 0x0025D94A
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.", false);
				return false;
			}
			return !pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null;
		}

		// Token: 0x06006CC3 RID: 27843 RVA: 0x0025F778 File Offset: 0x0025D978
		public static Caravan GetCaravan(this Pawn pawn)
		{
			return pawn.ParentHolder as Caravan;
		}

		// Token: 0x06006CC4 RID: 27844 RVA: 0x0025F785 File Offset: 0x0025D985
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x0025F790 File Offset: 0x0025D990
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x0025F7B0 File Offset: 0x0025D9B0
		public static int BestGotoDestNear(int tile, Caravan c)
		{
			Predicate<int> predicate = (int t) => !Find.World.Impassable(t) && c.CanReach(t);
			if (predicate(tile))
			{
				return tile;
			}
			int result;
			GenWorldClosest.TryFindClosestTile(tile, predicate, out result, 50, true);
			return result;
		}

		// Token: 0x06006CC7 RID: 27847 RVA: 0x0025F7F0 File Offset: 0x0025D9F0
		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].IsPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x0025F82C File Offset: 0x0025DA2C
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x0025F867 File Offset: 0x0025DA67
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}
	}
}
