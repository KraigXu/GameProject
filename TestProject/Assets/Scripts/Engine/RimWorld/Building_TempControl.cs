using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class Building_TempControl : Building
	{
		// Token: 0x06004C32 RID: 19506 RVA: 0x0019979E File Offset: 0x0019799E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x04002AEC RID: 10988
		public CompTempControl compTempControl;

		// Token: 0x04002AED RID: 10989
		public CompPowerTrader compPowerTrader;
	}
}
