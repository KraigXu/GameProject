using System;

namespace Verse.AI
{
	// Token: 0x0200051C RID: 1308
	public class JobDriver_WaitDowned : JobDriver_Wait
	{
		// Token: 0x06002566 RID: 9574 RVA: 0x000DE007 File Offset: 0x000DC207
		public override void DecorateWaitToil(Toil wait)
		{
			base.DecorateWaitToil(wait);
			wait.AddFailCondition(() => !this.pawn.Downed);
		}
	}
}
