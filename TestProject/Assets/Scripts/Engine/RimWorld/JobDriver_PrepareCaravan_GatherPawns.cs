using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		
		// (get) Token: 0x06002B40 RID: 11072 RVA: 0x000FAE44 File Offset: 0x000F9044
		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.AnimalOrSlave, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			this.FailOn(() => this.AnimalOrSlave == null || this.AnimalOrSlave.GetLord() != this.job.lord);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(this.AnimalOrSlave));
			yield return this.SetFollowerToil();
			yield break;
		}

		
		private Toil SetFollowerToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, this.pawn);
					RestUtility.WakeUp(this.pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;
	}
}
