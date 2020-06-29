using System;
using Verse;

namespace RimWorld
{
	
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		
		private const int StoneThrowInterval = 400;
	}
}
