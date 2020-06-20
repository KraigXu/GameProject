using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000691 RID: 1681
	public static class Toils_Bed
	{
		// Token: 0x06002DBE RID: 11710 RVA: 0x00101628 File Offset: 0x000FF828
		public static Toil GotoBed(TargetIndex bedIndex)
		{
			Toil gotoBed = new Toil();
			gotoBed.initAction = delegate
			{
				Pawn actor = gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
			};
			gotoBed.tickAction = delegate
			{
				Pawn actor = gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			};
			gotoBed.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoBed.FailOnBedNoLongerUsable(bedIndex);
			return gotoBed;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x001016A0 File Offset: 0x000FF8A0
		public static Toil ClaimBedIfNonMedical(TargetIndex ind, TargetIndex claimantIndex = TargetIndex.None)
		{
			Toil claim = new Toil();
			claim.initAction = delegate
			{
				Pawn actor = claim.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null)
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(ind).Thing);
				}
			};
			claim.FailOnDespawnedOrNull(ind);
			return claim;
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x001016FC File Offset: 0x000FF8FC
		public static T FailOnNonMedicalBedNotOwned<T>(this T f, TargetIndex bedIndex, TargetIndex claimantIndex = TargetIndex.None) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null)
				{
					Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
					if (building_Bed.Medical)
					{
						if ((!pawn.InBed() || pawn.CurrentBed() != building_Bed) && !building_Bed.AnyUnoccupiedSleepingSlot)
						{
							return JobCondition.Incompletable;
						}
					}
					else
					{
						if (!building_Bed.OwnersForReading.Contains(pawn))
						{
							return JobCondition.Incompletable;
						}
						if (pawn.InBed() && pawn.CurrentBed() == building_Bed)
						{
							int curOccupantSlotIndex = building_Bed.GetCurOccupantSlotIndex(pawn);
							if (curOccupantSlotIndex >= building_Bed.OwnersForReading.Count || building_Bed.OwnersForReading[curOccupantSlotIndex] != pawn)
							{
								return JobCondition.Incompletable;
							}
						}
					}
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x00101748 File Offset: 0x000FF948
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex)
		{
			toil.FailOnDespawnedOrNull(bedIndex);
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).IsBurning());
			toil.FailOn(() => ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).ForPrisoners != toil.actor.IsPrisoner);
			toil.FailOnNonMedicalBedNotOwned(bedIndex, TargetIndex.None);
			toil.FailOn(() => !HealthAIUtility.ShouldSeekMedicalRest(toil.actor) && !HealthAIUtility.ShouldSeekMedicalRestUrgent(toil.actor) && ((Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing).Medical);
			toil.FailOn(() => toil.actor.IsColonist && !toil.actor.CurJob.ignoreForbidden && !toil.actor.Downed && toil.actor.CurJob.GetTarget(bedIndex).Thing.IsForbidden(toil.actor));
		}
	}
}
