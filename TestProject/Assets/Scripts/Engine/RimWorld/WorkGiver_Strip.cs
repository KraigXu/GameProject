using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Strip))
			{
				if (!designation.target.HasThing)
				{
					Log.ErrorOnce("Strip designation has no target.", 63126, false);
				}
				else
				{
					yield return designation.target.Thing;
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		
		
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Strip);
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) != null && pawn.CanReserve(t, 1, -1, null, forced) && StrippableUtility.CanBeStrippedByColony(t);
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Strip, t);
		}
	}
}
