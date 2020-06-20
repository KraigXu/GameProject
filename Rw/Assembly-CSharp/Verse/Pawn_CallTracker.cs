using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200029A RID: 666
	public class Pawn_CallTracker
	{
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0006C25C File Offset: 0x0006A45C
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x0006C2EC File Offset: 0x0006A4EC
		private float IdleCallVolumeFactor
		{
			get
			{
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					return 1f;
				case TimeSpeed.Normal:
					return 1f;
				case TimeSpeed.Fast:
					return 1f;
				case TimeSpeed.Superfast:
					return 0.25f;
				case TimeSpeed.Ultrafast:
					return 0.25f;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0006C343 File Offset: 0x0006A543
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0006C359 File Offset: 0x0006A559
		public void CallTrackerTick()
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall--;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0006C38D File Offset: 0x0006A58D
		private void ResetTicksToNextCall()
		{
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall /= 4;
			}
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0006C3C8 File Offset: 0x0006A5C8
		private void TryDoCall()
		{
			if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				return;
			}
			if (this.pawn.Downed || !this.pawn.Awake())
			{
				return;
			}
			if (this.pawn.Position.Fogged(this.pawn.Map))
			{
				return;
			}
			this.DoCall();
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0006C440 File Offset: 0x0006A640
		public void DoCall()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.PawnAggressive)
			{
				LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundAngry, 1f);
				return;
			}
			LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge ls) => ls.soundCall, this.IdleCallVolumeFactor);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0006C4C4 File Offset: 0x0006A6C4
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006C4E4 File Offset: 0x0006A6E4
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0006C510 File Offset: 0x0006A710
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04000CEF RID: 3311
		public Pawn pawn;

		// Token: 0x04000CF0 RID: 3312
		private int ticksToNextCall = -1;

		// Token: 0x04000CF1 RID: 3313
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x04000CF2 RID: 3314
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x04000CF3 RID: 3315
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x04000CF4 RID: 3316
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
