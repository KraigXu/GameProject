using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000831 RID: 2097
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x06003471 RID: 13425 RVA: 0x0011FDEC File Offset: 0x0011DFEC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
