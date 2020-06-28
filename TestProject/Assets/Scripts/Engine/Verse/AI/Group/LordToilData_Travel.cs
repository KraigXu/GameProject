using System;

namespace Verse.AI.Group
{
	// Token: 0x020005DB RID: 1499
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x060029BD RID: 10685 RVA: 0x000F5114 File Offset: 0x000F3314
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04001903 RID: 6403
		public IntVec3 dest;
	}
}
