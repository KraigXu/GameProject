using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000729 RID: 1833
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x0010F16D File Offset: 0x0010D36D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x0010F178 File Offset: 0x0010D378
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
			}
			bool flag = this.def.workType == WorkTypeDefOf.Construction;
			if (!GenConstruct.CanConstruct(blueprint, pawn, flag, forced))
			{
				return null;
			}
			if (!flag && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
			{
				return null;
			}
			Job job = base.RemoveExistingFloorJob(pawn, blueprint);
			if (job != null)
			{
				return job;
			}
			Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true);
			if (job2 != null)
			{
				return job2;
			}
			if (this.def.workType != WorkTypeDefOf.Hauling)
			{
				Job job3 = this.NoCostFrameMakeJobFor(pawn, blueprint);
				if (job3 != null)
				{
					return job3;
				}
			}
			return null;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x0010F222 File Offset: 0x0010D422
		private Job NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.MaterialsNeeded().Count == 0)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}
