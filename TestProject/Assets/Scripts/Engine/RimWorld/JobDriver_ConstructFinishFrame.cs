using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000631 RID: 1585
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000FB20C File Offset: 0x000F940C
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000FB232 File Offset: 0x000F9432
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil();
			build.initAction = delegate
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickAction = delegate
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				if (frame.resourceContainer.Count > 0)
				{
					actor.skills.Learn(SkillDefOf.Construction, 0.25f, false);
				}
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToBuild = frame.WorkToBuild;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
					if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num / workToBuild))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
				}
				frame.workDone += num;
				if (frame.workDone >= workToBuild)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
					return;
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, true, false));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			build.activeSkill = (() => SkillDefOf.Construction);
			yield return build;
			yield break;
		}

		// Token: 0x0400199D RID: 6557
		private const int JobEndInterval = 5000;
	}
}
