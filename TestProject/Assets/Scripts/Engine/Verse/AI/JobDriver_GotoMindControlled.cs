using System;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public class JobDriver_GotoMindControlled : JobDriver_Goto
	{
		
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
