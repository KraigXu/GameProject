using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000689 RID: 1673
	public static class Toils_Interpersonal
	{
		// Token: 0x06002D71 RID: 11633 RVA: 0x00100190 File Offset: 0x000FE390
		public static Toil GotoInteractablePosition(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(pawn, PathEndMode.Touch);
			};
			toil.tickAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				Map map = actor.Map;
				if (InteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (!actor.pather.Moving)
				{
					IntVec3 intVec = IntVec3.Invalid;
					int num = 0;
					while (num < 9 && (num != 8 || !intVec.IsValid))
					{
						IntVec3 intVec2 = pawn.Position + GenAdj.AdjacentCellsAndInside[num];
						if (intVec2.InBounds(map) && intVec2.Walkable(map) && intVec2 != actor.Position && InteractionUtility.IsGoodPositionForInteraction(intVec2, pawn.Position, map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
						{
							intVec = intVec2;
						}
						num++;
					}
					if (intVec.IsValid)
					{
						actor.pather.StartPath(intVec, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x00100204 File Offset: 0x000FE404
		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || (mode != PrisonerInteractionModeDefOf.Execution && !talkee.Awake()) || !talkee.IsPrisonerOfColony || (talkee.guest == null || talkee.guest.interactionMode != mode));
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x00100264 File Offset: 0x000FE464
		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			return new Toil
			{
				initAction = delegate
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				tickAction = delegate
				{
					if (!pawn.interactions.InteractedTooRecentlyToInteract())
					{
						pawn.jobs.curDriver.ReadyForNextToil();
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x001002B8 File Offset: 0x000FE4B8
		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.BuildRapport))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			return toil;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0010031C File Offset: 0x000FE51C
		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0010036C File Offset: 0x000FE56C
		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = pawn.AnimalOrWildMan() ? InteractionDefOf.TameAttempt : InteractionDefOf.RecruitAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			return toil;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x001003D8 File Offset: 0x000FE5D8
		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.RaceProps.wildness);
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936, false);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall();
						}
						text = "TextMote_TrainSuccess".Translate(trainableDef.LabelCap, num.ToStringPercent());
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(trainableDef.LabelCap, num.ToStringPercent());
					}
					text = string.Concat(new object[]
					{
						text,
						"\n",
						pawn.training.GetSteps(trainableDef),
						" / ",
						trainableDef.steps
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			return toil;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x00100434 File Offset: 0x000FE634
		public static Toil Interact(TargetIndex otherPawnInd, InteractionDef interaction)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, interaction);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 60;
			return toil;
		}
	}
}
