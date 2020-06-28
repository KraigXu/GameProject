using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072A RID: 1834
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x0010F268 File Offset: 0x0010D468
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x0010F274 File Offset: 0x0010D474
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			bool checkSkills = this.def.workType == WorkTypeDefOf.Construction;
			if (!GenConstruct.CanConstruct(frame, pawn, checkSkills, forced))
			{
				return null;
			}
			return base.ResourceDeliverJobFor(pawn, frame, true);
		}
	}
}
