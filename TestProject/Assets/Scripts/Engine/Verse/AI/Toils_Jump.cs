using System;

namespace Verse.AI
{
	// Token: 0x02000534 RID: 1332
	public static class Toils_Jump
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x000E1564 File Offset: 0x000DF764
		public static Toil Jump(Toil jumpTarget)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
			};
			return toil;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000E15A8 File Offset: 0x000DF7A8
		public static Toil JumpIf(Toil jumpTarget, Func<bool> condition)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				if (condition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpTarget);
				}
			};
			return toil;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000E15F4 File Offset: 0x000DF7F4
		public static Toil JumpIfTargetDespawnedOrNull(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing == null || !thing.Spawned)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000E1640 File Offset: 0x000DF840
		public static Toil JumpIfTargetInvalid(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				if (!toil.actor.jobs.curJob.GetTarget(ind).IsValid)
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000E168C File Offset: 0x000DF88C
		public static Toil JumpIfTargetNotHittable(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (curJob.verbToUse == null || !curJob.verbToUse.IsStillUsableBy(actor) || !curJob.verbToUse.CanHitTarget(target))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000E16D8 File Offset: 0x000DF8D8
		public static Toil JumpIfTargetDowned(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.Downed)
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000E1724 File Offset: 0x000DF924
		public static Toil JumpIfHaveTargetInQueue(TargetIndex ind, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				if (!actor.jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000E1770 File Offset: 0x000DF970
		public static Toil JumpIfCannotTouch(TargetIndex ind, PathEndMode peMode, Toil jumpToil)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (!actor.CanReachImmediate(target, peMode))
				{
					actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			};
			return toil;
		}
	}
}
