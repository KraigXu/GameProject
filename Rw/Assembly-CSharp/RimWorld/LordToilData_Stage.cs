using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A3 RID: 1955
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x0011AF5C File Offset: 0x0011915C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x04001B73 RID: 7027
		public IntVec3 stagingPoint;
	}
}
