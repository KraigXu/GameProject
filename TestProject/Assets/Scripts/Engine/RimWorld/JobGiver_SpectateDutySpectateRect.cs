using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			IntVec3 c;
			if ((duty.spectateRectPreferredSide == SpectateRectSide.None || !SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out c, duty.spectateRectPreferredSide, 1, null)) && !SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out c, duty.spectateRectAllowedSides, 1, null))
			{
				return null;
			}
			IntVec3 centerCell = duty.spectateRect.CenterCell;
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isSittable && pawn.CanReserve(edifice, 1, -1, null, false))
			{
				return JobMaker.MakeJob(JobDefOf.SpectateCeremony, edifice, centerCell);
			}
			return JobMaker.MakeJob(JobDefOf.SpectateCeremony, c, centerCell);
		}
	}
}
