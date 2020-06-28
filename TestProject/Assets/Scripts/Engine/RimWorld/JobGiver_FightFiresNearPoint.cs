using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006CB RID: 1739
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		// Token: 0x06002E9F RID: 11935 RVA: 0x00105F9C File Offset: 0x0010419C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00105FB8 File Offset: 0x001041B8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = (Thing t) => !(((AttachableThing)t).parent is Pawn) && pawn.CanReserve(t, 1, -1, null, false) && !pawn.WorkTagIsDisabled(WorkTags.Firefighting);
			Thing thing = GenClosest.ClosestThingReachable(pawn.GetLord().CurLordToil.FlagLoc, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), this.maxDistFromPoint, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing);
			}
			return null;
		}

		// Token: 0x04001A75 RID: 6773
		public float maxDistFromPoint = -1f;
	}
}
