using System;

namespace RimWorld
{
	// Token: 0x020009C3 RID: 2499
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0013B220 File Offset: 0x00139420
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x0013B227 File Offset: 0x00139427
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		// Token: 0x04002334 RID: 9012
		private const float MaxTempOffset = 17f;
	}
}
