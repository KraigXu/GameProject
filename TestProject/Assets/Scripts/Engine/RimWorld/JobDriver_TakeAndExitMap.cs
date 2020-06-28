using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000675 RID: 1653
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000FEE2C File Offset: 0x000FD02C
		protected Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000FEE4D File Offset: 0x000FD04D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000FEE6F File Offset: 0x000FD06F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil toil = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			toil.AddPreTickAction(delegate
			{
				if (base.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
				}
			});
			toil.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			yield return toil;
			Toil toil2 = new Toil();
			toil2.initAction = delegate
			{
				if (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
				}
			};
			toil2.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x04001A0C RID: 6668
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04001A0D RID: 6669
		private const TargetIndex ExitCellInd = TargetIndex.B;
	}
}
