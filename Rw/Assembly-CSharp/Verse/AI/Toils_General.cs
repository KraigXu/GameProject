using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000531 RID: 1329
	public static class Toils_General
	{
		// Token: 0x06002618 RID: 9752 RVA: 0x000E1100 File Offset: 0x000DF300
		public static Toil Wait(int ticks, TargetIndex face = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickAction = delegate
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			return toil;
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000E1188 File Offset: 0x000DF388
		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor", false);
						return;
					}
					PawnUtility.ForceWait(pawn, ticks, null, maintainPosture);
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000E1238 File Offset: 0x000DF438
		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000E127C File Offset: 0x000DF47C
		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000E12C0 File Offset: 0x000DF4C0
		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			};
			return toil;
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000E12FB File Offset: 0x000DF4FB
		public static Toil Do(Action action)
		{
			return new Toil
			{
				initAction = action
			};
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000E1309 File Offset: 0x000DF509
		public static Toil DoAtomic(Action action)
		{
			return new Toil
			{
				initAction = action,
				atomicWithPrevious = true
			};
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000E1320 File Offset: 0x000DF520
		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = new Toil();
			open.initAction = delegate
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000E136E File Offset: 0x000DF56E
		public static Toil Label()
		{
			return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
