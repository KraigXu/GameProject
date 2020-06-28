using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200067E RID: 1662
	public static class Toils_Misc
	{
		// Token: 0x06002D46 RID: 11590 RVA: 0x000FFB2C File Offset: 0x000FDD2C
		public static Toil Learn(SkillDef skill, float xp)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.skills.Learn(skill, xp, false);
			};
			return toil;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000FFB78 File Offset: 0x000FDD78
		public static Toil SetForbidden(TargetIndex ind, bool forbidden)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				toil.actor.CurJob.GetTarget(ind).Thing.SetForbidden(forbidden, true);
			};
			return toil;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000FFBC4 File Offset: 0x000FDDC4
		public static Toil TakeItemFromInventoryToCarrier(Pawn pawn, TargetIndex itemInd)
		{
			return new Toil
			{
				initAction = delegate
				{
					Job curJob = pawn.CurJob;
					Thing thing = (Thing)curJob.GetTarget(itemInd);
					int count = Mathf.Min(thing.stackCount, curJob.count);
					pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, count, true);
					curJob.SetTarget(itemInd, pawn.carryTracker.CarriedThing);
				}
			};
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000FFBFC File Offset: 0x000FDDFC
		public static Toil ThrowColonistAttackingMote(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted && !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(target).Thing))
				{
					MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
				}
			};
			return toil;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000FFC40 File Offset: 0x000FDE40
		public static Toil FindRandomAdjacentReachableCell(TargetIndex adjacentToInd, TargetIndex cellInd)
		{
			Toil findCell = new Toil();
			findCell.initAction = delegate
			{
				Pawn actor = findCell.actor;
				Job curJob = actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(adjacentToInd);
				if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
				{
					Log.Error(string.Concat(new object[]
					{
						actor,
						" could not find standable cell adjacent to ",
						target,
						" because this thing is either unspawned or spawned somewhere else."
					}), false);
					actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
					return;
				}
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					num++;
					if (num > 100)
					{
						break;
					}
					if (target.HasThing)
					{
						c = target.Thing.RandomAdjacentCell8Way();
					}
					else
					{
						c = target.Cell.RandomAdjacentCell8Way();
					}
					if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) && actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						goto Block_7;
					}
				}
				Log.Error(actor + " could not find standable cell adjacent to " + target, false);
				actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
				return;
				Block_7:
				curJob.SetTarget(cellInd, c);
			};
			return findCell;
		}
	}
}
