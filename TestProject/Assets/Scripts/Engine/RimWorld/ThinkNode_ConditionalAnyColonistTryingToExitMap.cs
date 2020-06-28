using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F0 RID: 2032
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		// Token: 0x060033D6 RID: 13270 RVA: 0x0011DF5C File Offset: 0x0011C15C
		protected override bool Satisfied(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in mapHeld.mapPawns.FreeColonistsSpawned)
			{
				Job curJob = pawn2.CurJob;
				if (curJob != null && curJob.exitMapOnArrival)
				{
					return true;
				}
			}
			return false;
		}
	}
}
