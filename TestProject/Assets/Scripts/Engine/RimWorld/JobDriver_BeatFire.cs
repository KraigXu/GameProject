using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_BeatFire : JobDriver
	{
		
		// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000FB804 File Offset: 0x000F9A04
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil();
			approach.initAction = delegate
			{
				if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
				{
					this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null, true);
				}
				this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
			};
			approach.tickAction = delegate
			{
				if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
				}
				if (this.pawn.Position != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate
			{
				if (!this.pawn.CanReachImmediate(this.TargetFire, PathEndMode.Touch))
				{
					this.JumpToToil(approach);
					return;
				}
				if (this.pawn.Position != this.TargetFire.Position && this.StartBeatingFireIfAnyAt(this.pawn.Position, beat))
				{
					return;
				}
				this.pawn.natives.TryBeatFire(this.TargetFire);
				if (this.TargetFire.Destroyed)
				{
					this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield break;
		}

		
		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}
	}
}
