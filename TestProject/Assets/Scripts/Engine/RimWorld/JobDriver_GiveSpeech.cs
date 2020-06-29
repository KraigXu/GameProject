using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		
		
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, this.Throne.InteractionCell + this.Throne.Rotation.FacingCell);
			});
			Toil toil = new Toil();
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_Passable)) != null);
			toil.tickAction = delegate
			{
				this.pawn.GainComfortFromCellIfPossible(false);
				this.pawn.skills.Learn(SkillDefOf.Social, 0.3f, false);
				if (this.pawn.IsHashIntervalTick(JobDriver_GiveSpeech.MoteInterval.RandomInRange))
				{
					MoteMaker.MakeSpeechBubble(this.pawn, JobDriver_GiveSpeech.moteIcon);
				}
				this.rotateToFace = TargetIndex.B;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		
		private const TargetIndex ThroneIndex = TargetIndex.A;

		
		private const TargetIndex FacingIndex = TargetIndex.B;

		
		private static readonly IntRange MoteInterval = new IntRange(300, 500);

		
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
