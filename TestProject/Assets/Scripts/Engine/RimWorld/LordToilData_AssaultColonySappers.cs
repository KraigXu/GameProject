using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200078E RID: 1934
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x06003280 RID: 12928 RVA: 0x0011904C File Offset: 0x0011724C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x04001B59 RID: 7001
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}
