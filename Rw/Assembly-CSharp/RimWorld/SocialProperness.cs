using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007BF RID: 1983
	public static class SocialProperness
	{
		// Token: 0x06003369 RID: 13161 RVA: 0x0011D2D9 File Offset: 0x0011B4D9
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x0011D2EC File Offset: 0x0011B4EC
		public static bool IsSociallyProper(this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false)
		{
			if (!animalsCare && p != null && !p.RaceProps.Humanlike)
			{
				return true;
			}
			if (!t.def.socialPropernessMatters)
			{
				return true;
			}
			if (!t.Spawned)
			{
				return true;
			}
			IntVec3 intVec = t.def.hasInteractionCell ? t.InteractionCell : t.Position;
			if (forPrisoner)
			{
				return p == null || intVec.GetRoom(t.Map, RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable);
			}
			return !intVec.IsInPrisonCell(t.Map);
		}
	}
}
