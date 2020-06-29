using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x001115DD File Offset: 0x0010F7DD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.FermentingBarrel);
			for (int i = 0; i < list.Count; i++)
			{
				if (((Building_FermentingBarrel)list[i]).Fermented)
				{
					return false;
				}
			}
			return true;
		}

		
		// (get) Token: 0x0600314F RID: 12623 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			return building_FermentingBarrel != null && building_FermentingBarrel.Fermented && !t.IsBurning() && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
