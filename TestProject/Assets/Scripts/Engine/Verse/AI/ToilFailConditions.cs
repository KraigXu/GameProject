using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200052D RID: 1325
	public static class ToilFailConditions
	{
		// Token: 0x060025F4 RID: 9716 RVA: 0x000E07B8 File Offset: 0x000DE9B8
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition(delegate
			{
				if (condition(toil))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return toil;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000E07F8 File Offset: 0x000DE9F8
		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (condition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000E082C File Offset: 0x000DEA2C
		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTarget(ind).Thing.DestroyedOrNull())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000E0870 File Offset: 0x000DEA70
		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				if (thing == null && target.IsValid)
				{
					return JobCondition.Ongoing;
				}
				if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000E08B4 File Offset: 0x000DEAB4
		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				Thing thing = target.Thing;
				if (thing == null && target.IsValid)
				{
					return JobCondition.Ongoing;
				}
				if (thing == null || !thing.Spawned || thing.Map != f.GetActor().Map)
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000E0900 File Offset: 0x000DEB00
		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000E094C File Offset: 0x000DEB4C
		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000E0990 File Offset: 0x000DEB90
		public static T FailOnDownedOrDead<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Thing thing = f.GetActor().jobs.curJob.GetTarget(ind).Thing;
				if (((Pawn)thing).Downed || ((Pawn)thing).Dead)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000E09D4 File Offset: 0x000DEBD4
		public static T FailOnMobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).health.State == PawnHealthState.Mobile)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000E0A18 File Offset: 0x000DEC18
		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000E0A5C File Offset: 0x000DEC5C
		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Awake())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000E0AA0 File Offset: 0x000DECA0
		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).CanCasuallyInteractNow(false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000E0AE4 File Offset: 0x000DECE4
		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000E0B28 File Offset: 0x000DED28
		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000E0B6C File Offset: 0x000DED6C
		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && pawn.HostileTo(f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000E0BB0 File Offset: 0x000DEDB0
		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing != null && actor.Map.physicalInteractionReservationManager.IsReserved(thing) && !actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000E0BF4 File Offset: 0x000DEDF4
		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				if (actor.Faction != Faction.OfPlayer)
				{
					return JobCondition.Ongoing;
				}
				if (actor.jobs.curJob.ignoreForbidden)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null)
				{
					return JobCondition.Ongoing;
				}
				if (thing.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000E0C38 File Offset: 0x000DEE38
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000E0C4B File Offset: 0x000DEE4B
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000E0C60 File Offset: 0x000DEE60
		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000E0CAC File Offset: 0x000DEEAC
		public static T FailOnThingHavingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) != null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000E0CF8 File Offset: 0x000DEEF8
		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				if (actor.Map.designationManager.DesignationAt(curJob.GetTarget(ind).Cell, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000E0D44 File Offset: 0x000DEF44
		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTarget(ind).ToTargetInfo(f.GetActor().Map).IsBurning())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000E0D88 File Offset: 0x000DEF88
		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().CanReachImmediate(f.GetActor().jobs.curJob.GetTarget(ind), peMode))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000E0DD4 File Offset: 0x000DEFD4
		public static T FailOnIncapable<T>(this T f, PawnCapacityDef pawnCapacity) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().health.capacities.CapableOf(pawnCapacity))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000E0E18 File Offset: 0x000DF018
		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil)
		{
			toil.AddFailCondition(delegate
			{
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					return false;
				}
				for (int i = 0; i < toil.actor.jobs.curJob.placedThings.Count; i++)
				{
					ThingCountClass thingCountClass = toil.actor.jobs.curJob.placedThings[i];
					if (thingCountClass.thing == null || !thingCountClass.thing.Spawned || thingCountClass.thing.Map != toil.actor.Map || (!toil.actor.CurJob.ignoreForbidden && thingCountClass.thing.IsForbidden(toil.actor)))
					{
						return true;
					}
				}
				return false;
			});
			return toil;
		}
	}
}
