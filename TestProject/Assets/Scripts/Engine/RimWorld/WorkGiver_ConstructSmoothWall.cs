using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_ConstructSmoothWall : WorkGiver_Scanner
	{
		
		
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.SmoothWall))
			{
				yield return designation.target.Cell;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.SmoothWall);
		}

		
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (c.IsForbidden(pawn) || pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall) == null)
			{
				return false;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null || !edifice.def.IsSmoothable)
			{
				Log.ErrorOnce("Failed to find valid edifice when trying to smooth a wall", 58988176, false);
				pawn.Map.designationManager.TryRemoveDesignation(c, DesignationDefOf.SmoothWall);
				return false;
			}
			return pawn.CanReserve(edifice, 1, -1, null, forced) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothWall, c.GetEdifice(pawn.Map));
		}
	}
}
