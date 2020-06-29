using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x0010F268 File Offset: 0x0010D468
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		
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
