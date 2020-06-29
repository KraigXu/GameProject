using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		
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
