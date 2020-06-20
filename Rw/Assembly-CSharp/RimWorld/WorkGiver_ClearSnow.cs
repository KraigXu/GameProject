using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073A RID: 1850
	public class WorkGiver_ClearSnow : WorkGiver_Scanner
	{
		// Token: 0x0600307F RID: 12415 RVA: 0x0010FF7B File Offset: 0x0010E17B
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowClear.ActiveCells;
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x0010FF92 File Offset: 0x0010E192
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowClear.TrueCount == 0;
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x0010FFAC File Offset: 0x0010E1AC
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.snowGrid.GetDepth(c) >= 0.2f && !c.IsForbidden(pawn) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x0010FFE8 File Offset: 0x0010E1E8
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearSnow, c);
		}
	}
}
