using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072B RID: 1835
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x0010F268 File Offset: 0x0010D468
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x0010F2D8 File Offset: 0x0010D4D8
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
			if (frame.MaterialsNeeded().Count > 0)
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, true, forced))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.FinishFrame, frame);
		}
	}
}
