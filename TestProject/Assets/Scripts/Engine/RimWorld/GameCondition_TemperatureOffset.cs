using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C0 RID: 2496
	public class GameCondition_TemperatureOffset : GameCondition
	{
		// Token: 0x06003BA2 RID: 15266 RVA: 0x0013AEBE File Offset: 0x001390BE
		public override void Init()
		{
			base.Init();
			this.tempOffset = this.def.temperatureOffset;
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x0013AED7 File Offset: 0x001390D7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x0013AEF5 File Offset: 0x001390F5
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x04002332 RID: 9010
		public float tempOffset;
	}
}
