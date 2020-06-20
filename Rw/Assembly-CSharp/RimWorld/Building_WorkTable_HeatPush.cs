using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8A RID: 3210
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x06004D54 RID: 19796 RVA: 0x0019E71B File Offset: 0x0019C91B
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (Find.TickManager.TicksGame % 30 == 4)
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x04002B3E RID: 11070
		private const int HeatPushInterval = 30;
	}
}
