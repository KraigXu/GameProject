using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000739 RID: 1849
	internal class WorkGiver_CleanFilth : WorkGiver_Scanner
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003077 RID: 12407 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x0010FDB6 File Offset: 0x0010DFB6
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Filth);
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x0010FDBF File Offset: 0x0010DFBF
		public override int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x0010FDC2 File Offset: 0x0010DFC2
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x0010FDD4 File Offset: 0x0010DFD4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x0010FDF0 File Offset: 0x0010DFF0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && pawn.CanReserve(t, 1, -1, null, forced) && filth.TicksSinceThickened >= this.MinTicksSinceThickened;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x0010FE50 File Offset: 0x0010E050
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(TargetIndex.A, t);
			int num = 15;
			Map map = t.Map;
			Room room = t.GetRoom(RegionType.Set_Passable);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 intVec = t.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_Passable) == room)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (this.HasJobOnThing(pawn, thing, forced) && thing != t)
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}

		// Token: 0x04001AE7 RID: 6887
		private int MinTicksSinceThickened = 600;
	}
}
