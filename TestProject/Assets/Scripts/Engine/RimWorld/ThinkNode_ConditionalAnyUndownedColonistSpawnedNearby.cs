using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F1 RID: 2033
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x060033D8 RID: 13272 RVA: 0x0011DFD0 File Offset: 0x0011C1D0
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.Spawned)
			{
				return pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
			}
			return false;
		}
	}
}
