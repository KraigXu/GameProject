using System;

namespace Verse.AI.Group
{
	// Token: 0x020005D6 RID: 1494
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x060029A8 RID: 10664 RVA: 0x000F4E24 File Offset: 0x000F3024
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		// Token: 0x040018FB RID: 6395
		public LocomotionUrgency locomotion;

		// Token: 0x040018FC RID: 6396
		public bool canDig;

		// Token: 0x040018FD RID: 6397
		public bool interruptCurrentJob;
	}
}
