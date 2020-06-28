using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000055 RID: 85
	public static class WildManUtility
	{
		// Token: 0x060003C1 RID: 961 RVA: 0x000136C9 File Offset: 0x000118C9
		public static bool IsWildMan(this Pawn p)
		{
			return p.kindDef == PawnKindDefOf.WildMan;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000136D8 File Offset: 0x000118D8
		public static bool AnimalOrWildMan(this Pawn p)
		{
			return p.RaceProps.Animal || p.IsWildMan();
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000136EF File Offset: 0x000118EF
		public static bool NonHumanlikeOrWildMan(this Pawn p)
		{
			return !p.RaceProps.Humanlike || p.IsWildMan();
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013706 File Offset: 0x00011906
		public static bool WildManShouldReachOutsideNow(Pawn p)
		{
			return p.IsWildMan() && !p.mindState.WildManEverReachedOutside && (!p.IsPrisoner || p.guest.Released);
		}
	}
}
