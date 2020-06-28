using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000723 RID: 1827
	public class WorkGiver_BuildRoof : WorkGiver_Scanner
	{
		// Token: 0x0600300F RID: 12303 RVA: 0x0010E633 File Offset: 0x0010C833
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.BuildRoof.ActiveCells;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x0010E64A File Offset: 0x0010C84A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.BuildRoof.TrueCount == 0;
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x0010E664 File Offset: 0x0010C864
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!pawn.Map.areaManager.BuildRoof[c])
			{
				return false;
			}
			if (c.Roofed(pawn.Map))
			{
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced))
			{
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn) && this.BuildingToTouchToBeAbleToBuildRoof(c, pawn) == null)
			{
				return false;
			}
			if (!RoofCollapseUtility.WithinRangeOfRoofHolder(c, pawn.Map, false))
			{
				return false;
			}
			if (!RoofCollapseUtility.ConnectedToRoofHolder(c, pawn.Map, true))
			{
				return false;
			}
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			return thing == null || RoofUtility.CanHandleBlockingThing(thing, pawn, forced);
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x0010E720 File Offset: 0x0010C920
		private Building BuildingToTouchToBeAbleToBuildRoof(IntVec3 c, Pawn pawn)
		{
			if (c.Standable(pawn.Map))
			{
				return null;
			}
			Building edifice = c.GetEdifice(pawn.Map);
			if (edifice == null)
			{
				return null;
			}
			if (!pawn.CanReach(edifice, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				return null;
			}
			return edifice;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x0010E76C File Offset: 0x0010C96C
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			LocalTargetInfo targetB = c;
			Thing thing = RoofUtility.FirstBlockingThing(c, pawn.Map);
			if (thing != null)
			{
				return RoofUtility.HandleBlockingThingJob(thing, pawn, forced);
			}
			if (!pawn.CanReach(c, PathEndMode.Touch, pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
			{
				targetB = this.BuildingToTouchToBeAbleToBuildRoof(c, pawn);
			}
			return JobMaker.MakeJob(JobDefOf.BuildRoof, c, targetB);
		}
	}
}
