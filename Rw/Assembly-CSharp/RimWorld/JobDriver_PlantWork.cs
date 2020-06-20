using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000681 RID: 1665
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002D52 RID: 11602 RVA: 0x000FFD38 File Offset: 0x000FDF38
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002D53 RID: 11603 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000FFD50 File Offset: 0x000FDF50
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid && !this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000FFDAE File Offset: 0x000FDFAE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation != null) ? ((Thing t) => this.Map.designationManager.DesignationOn(t, this.RequiredDesignation) != null) : null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			if (this.RequiredDesignation != null)
			{
				toil.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			yield return toil;
			Toil cut = new Toil();
			cut.tickAction = delegate
			{
				Pawn actor = cut.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Plants, this.xpPerTick, false);
				}
				float num = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				Plant plant = this.Plant;
				num *= Mathf.Lerp(3.3f, 1f, plant.Growth);
				this.workDone += num;
				if (this.workDone >= plant.def.plant.harvestWork)
				{
					if (plant.def.plant.harvestedThingDef != null)
					{
						if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && !plant.Blighted && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
						{
							MoteMaker.ThrowText((this.pawn.DrawPos + plant.DrawPos) / 2f, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
						}
						else
						{
							int num2 = plant.YieldNow();
							if (num2 > 0)
							{
								Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef, null);
								thing.stackCount = num2;
								if (actor.Faction != Faction.OfPlayer)
								{
									thing.SetForbidden(true, true);
								}
								Find.QuestManager.Notify_PlantHarvested(actor, thing);
								GenPlace.TryPlaceThing(thing, actor.Position, this.Map, ThingPlaceMode.Near, null, null, default(Rot4));
								actor.records.Increment(RecordDefOf.PlantsHarvested);
							}
						}
					}
					plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
					plant.PlantCollected();
					this.workDone = 0f;
					this.ReadyForNextToil();
					return;
				}
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			if (this.RequiredDesignation != null)
			{
				cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting);
			cut.activeSkill = (() => SkillDefOf.Plants);
			yield return cut;
			Toil toil2 = this.PlantWorkDoneToil();
			if (toil2 != null)
			{
				yield return toil2;
			}
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000FFDBE File Offset: 0x000FDFBE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void Init()
		{
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x04001A1F RID: 6687
		private float workDone;

		// Token: 0x04001A20 RID: 6688
		protected float xpPerTick;

		// Token: 0x04001A21 RID: 6689
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
