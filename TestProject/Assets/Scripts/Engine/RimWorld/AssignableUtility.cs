using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CEE RID: 3310
	public static class AssignableUtility
	{
		// Token: 0x0600508D RID: 20621 RVA: 0x001B1684 File Offset: 0x001AF884
		public static Pawn GetAssignedPawn(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawnsForReading[0];
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001B16B8 File Offset: 0x001AF8B8
		public static IEnumerable<Pawn> GetAssignedPawns(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawns;
		}
	}
}
