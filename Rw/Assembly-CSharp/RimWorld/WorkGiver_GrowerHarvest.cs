using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000744 RID: 1860
	public class WorkGiver_GrowerHarvest : WorkGiver_Grower
	{
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x001119A0 File Offset: 0x0010FBA0
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Plant plant = c.GetPlant(pawn.Map);
			return plant != null && !plant.IsForbidden(pawn) && plant.HarvestableNow && plant.LifeStage == PlantLifeStage.Mature && plant.CanYieldNow() && pawn.CanReserve(plant, 1, -1, null, forced);
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x001119FC File Offset: 0x0010FBFC
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.GetLord() != null || base.ShouldSkip(pawn, forced);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x00111A10 File Offset: 0x0010FC10
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Harvest);
			Map map = pawn.Map;
			Room room = c.GetRoom(map, RegionType.Set_Passable);
			float num = 0f;
			for (int i = 0; i < 40; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.GetRoom(map, RegionType.Set_Passable) == room && this.HasJobOnCell(pawn, intVec, false))
				{
					Plant plant = intVec.GetPlant(map);
					if (!(intVec != c) || plant.def == WorkGiver_Grower.CalculateWantedPlantDef(intVec, map))
					{
						num += plant.def.plant.harvestWork;
						if (intVec != c && num > 2400f)
						{
							break;
						}
						job.AddQueuedTarget(TargetIndex.A, plant);
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 3)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
