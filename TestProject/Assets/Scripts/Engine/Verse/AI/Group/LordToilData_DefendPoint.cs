using System;

namespace Verse.AI.Group
{
	// Token: 0x020005D2 RID: 1490
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x0600299B RID: 10651 RVA: 0x000F4C44 File Offset: 0x000F2E44
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
		}

		// Token: 0x040018F9 RID: 6393
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x040018FA RID: 6394
		public float defendRadius = 28f;
	}
}
