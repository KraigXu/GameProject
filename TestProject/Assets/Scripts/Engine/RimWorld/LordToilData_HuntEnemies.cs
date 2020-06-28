using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000799 RID: 1945
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x060032AE RID: 12974 RVA: 0x00119D7C File Offset: 0x00117F7C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x04001B5F RID: 7007
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
