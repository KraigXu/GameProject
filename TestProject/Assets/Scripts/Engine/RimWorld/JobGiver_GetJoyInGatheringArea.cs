using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C4 RID: 1732
	public class JobGiver_GetJoyInGatheringArea : JobGiver_GetJoy
	{
		// Token: 0x06002E8C RID: 11916 RVA: 0x00105B14 File Offset: 0x00103D14
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevelPercentage > 0.92f)
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return def.Worker.TryGiveJobInGatheringArea(pawn, cell);
		}
	}
}
