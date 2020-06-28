using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000654 RID: 1620
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x06002C38 RID: 11320 RVA: 0x000FD1EC File Offset: 0x000FB3EC
		protected override void WatchTickAction()
		{
			if (!((Building)base.TargetA.Thing).TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			base.WatchTickAction();
		}
	}
}
