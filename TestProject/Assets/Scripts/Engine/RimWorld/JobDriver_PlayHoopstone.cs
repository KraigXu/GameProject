using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000647 RID: 1607
	public class JobDriver_PlayHoopstone : JobDriver_WatchBuilding
	{
		// Token: 0x06002BEF RID: 11247 RVA: 0x000FC5E8 File Offset: 0x000FA7E8
		protected override void WatchTickAction()
		{
			if (this.pawn.IsHashIntervalTick(400))
			{
				MoteMaker.ThrowStone(this.pawn, base.TargetA.Cell);
			}
			base.WatchTickAction();
		}

		// Token: 0x040019C2 RID: 6594
		private const int StoneThrowInterval = 400;
	}
}
