using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000535 RID: 1333
	public static class Toils_Goto
	{
		// Token: 0x06002633 RID: 9779 RVA: 0x000E17C0 File Offset: 0x000DF9C0
		public static Toil Goto(TargetIndex ind, PathEndMode peMode)
		{
			return Toils_Goto.GotoThing(ind, peMode);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000E17CC File Offset: 0x000DF9CC
		public static Toil GotoThing(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000E1834 File Offset: 0x000DFA34
		public static Toil GotoThing(TargetIndex ind, IntVec3 exactCell)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.pather.StartPath(exactCell, PathEndMode.OnCell);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ind);
			return toil;
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000E1890 File Offset: 0x000DFA90
		public static Toil GotoCell(TargetIndex ind, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				actor.pather.StartPath(actor.jobs.curJob.GetTarget(ind), peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000E18E8 File Offset: 0x000DFAE8
		public static Toil GotoCell(IntVec3 cell, PathEndMode peMode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.pather.StartPath(cell, peMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000E1940 File Offset: 0x000DFB40
		public static Toil MoveOffTargetBlueprint(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Thing thing = actor.jobs.curJob.GetTarget(targetInd).Thing as Blueprint;
				if (thing == null || !actor.Position.IsInside(thing))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				IntVec3 c;
				if (RCellFinder.TryFindGoodAdjacentSpotToTouch(actor, thing, out c))
				{
					actor.pather.StartPath(c, PathEndMode.OnCell);
					return;
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}
	}
}
