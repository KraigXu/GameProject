using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Reign : JobDriver_Meditate
	{
		
		
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		
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

		
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		
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

		
		protected const TargetIndex FacingInd = TargetIndex.B;

		
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
