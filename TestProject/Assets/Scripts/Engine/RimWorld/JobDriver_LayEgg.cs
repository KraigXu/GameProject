using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200061D RID: 1565
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x06002AD8 RID: 10968 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000F9E79 File Offset: 0x000F8079
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish).SetForbiddenIfOutsideHomeArea();
			});
			yield break;
		}

		// Token: 0x04001978 RID: 6520
		private const int LayEgg = 500;

		// Token: 0x04001979 RID: 6521
		private const TargetIndex LaySpotInd = TargetIndex.A;
	}
}
