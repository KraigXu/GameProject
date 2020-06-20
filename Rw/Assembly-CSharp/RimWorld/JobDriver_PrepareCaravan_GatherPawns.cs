using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200062B RID: 1579
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002B40 RID: 11072 RVA: 0x000FAE44 File Offset: 0x000F9044
		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000FAE6A File Offset: 0x000F906A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.AnimalOrSlave, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000FAE8C File Offset: 0x000F908C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			this.FailOn(() => this.AnimalOrSlave == null || this.AnimalOrSlave.GetLord() != this.job.lord);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(this.AnimalOrSlave));
			yield return this.SetFollowerToil();
			yield break;
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000FAE9C File Offset: 0x000F909C
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

		// Token: 0x04001995 RID: 6549
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;
	}
}
