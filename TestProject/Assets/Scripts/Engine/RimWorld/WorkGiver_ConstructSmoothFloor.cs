using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000FAFD1 File Offset: 0x000F91D1
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x0010E83E File Offset: 0x0010CA3E
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
