using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075A RID: 1882
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x00113376 File Offset: 0x00111576
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (Find.ResearchManager.currentProj == null)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003140 RID: 12608 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x00113392 File Offset: 0x00111592
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.currentProj == null;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001133A4 File Offset: 0x001115A4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj == null)
			{
				return false;
			}
			Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
			return building_ResearchBench != null && currentProj.CanBeResearchedAt(building_ResearchBench, false) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x001133EE File Offset: 0x001115EE
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Research, t);
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x00113400 File Offset: 0x00111600
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}
	}
}
