using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E12 RID: 3602
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x0600571A RID: 22298 RVA: 0x001CF7DC File Offset: 0x001CD9DC
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter, PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x001CF7EE File Offset: 0x001CD9EE
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter).ThenBy(PlayerPawnsDisplayOrderUtility.thingIDNumberGetter);
		}

		// Token: 0x04002F8A RID: 12170
		private static Func<Pawn, int> displayOrderGetter = delegate(Pawn x)
		{
			if (x.playerSettings == null)
			{
				return 999999;
			}
			return x.playerSettings.displayOrder;
		};

		// Token: 0x04002F8B RID: 12171
		private static Func<Pawn, int> thingIDNumberGetter = (Pawn x) => x.thingIDNumber;
	}
}
