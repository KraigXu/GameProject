using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000FAFD1 File Offset: 0x000F91D1
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
