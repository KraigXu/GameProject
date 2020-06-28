using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200050F RID: 1295
	public class JobDriver_GotoMindControlled : JobDriver_Goto
	{
		// Token: 0x06002513 RID: 9491 RVA: 0x000DBEDA File Offset: 0x000DA0DA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return toil;
			if (this.job.def.waitAfterArriving > 0)
			{
				yield return Toils_General.Wait(this.job.def.waitAfterArriving, TargetIndex.None);
			}
			yield break;
		}
	}
}
