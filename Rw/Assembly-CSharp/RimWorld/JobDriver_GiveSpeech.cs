using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200063D RID: 1597
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000FB93A File Offset: 0x000F9B3A
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000FB947 File Offset: 0x000F9B47
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000FB969 File Offset: 0x000F9B69
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

		// Token: 0x040019AA RID: 6570
		private const TargetIndex ThroneIndex = TargetIndex.A;

		// Token: 0x040019AB RID: 6571
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x040019AC RID: 6572
		private static readonly IntRange MoteInterval = new IntRange(300, 500);

		// Token: 0x040019AD RID: 6573
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
