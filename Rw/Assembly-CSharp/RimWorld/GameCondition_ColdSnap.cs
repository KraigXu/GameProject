using System;

namespace RimWorld
{
	// Token: 0x020009C4 RID: 2500
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x0013B220 File Offset: 0x00139420
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0013B23B File Offset: 0x0013943B
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		// Token: 0x04002335 RID: 9013
		private const float MaxTempOffset = -20f;
	}
}
