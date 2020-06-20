﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200068F RID: 1679
	public class JobDriver_Reign : JobDriver_Meditate
	{
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x0010132E File Offset: 0x000FF52E
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x0010133B File Offset: 0x000FF53B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00101360 File Offset: 0x000FF560
		public override string GetReport()
		{
			return string.Concat(new string[]
			{
				this.ReportStringProcessed(this.job.def.reportString),
				": ",
				this.Throne.LabelShort.CapitalizeFirst(),
				".",
				base.PsyfocusPerDayReport()
			});
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x001013BD File Offset: 0x000FF5BD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, this.Throne.InteractionCell + this.Throne.Rotation.FacingCell);
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = new Toil();
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_Passable)) != null);
			toil.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.tickAction = delegate
			{
				if (this.pawn.mindState.applyThroneThoughtsTick == 0)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 10000;
				}
				else if (this.pawn.mindState.applyThroneThoughtsTick <= Find.TickManager.TicksGame)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 60000;
					ThoughtDef thoughtDef = null;
					if (this.Throne.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.ThroneRoom)
					{
						thoughtDef = ThoughtDefOf.ReignedInThroneroom;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(this.Throne.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
						}
					}
				}
				this.rotateToFace = TargetIndex.B;
				base.MeditationTick();
				if (this.job.ignoreJoyTimeAssignment && this.pawn.IsHashIntervalTick(300))
				{
					this.pawn.jobs.CheckForJobOverride();
				}
			};
			yield return toil;
			yield break;
		}

		// Token: 0x04001A38 RID: 6712
		protected const TargetIndex FacingInd = TargetIndex.B;

		// Token: 0x04001A39 RID: 6713
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
