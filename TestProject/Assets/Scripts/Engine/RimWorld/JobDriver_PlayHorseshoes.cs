using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000648 RID: 1608
	public class JobDriver_PlayHorseshoes : JobDriver_WatchBuilding
	{
		// Token: 0x06002BF1 RID: 11249 RVA: 0x000FC630 File Offset: 0x000FA830
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowHorseshoe(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x040019C3 RID: 6595
		private const int HorseshoeThrowInterval = 400;
	}
}
