using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		
		private const int HorseshoeThrowInterval = 400;
	}
}
