using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x06003BB6 RID: 15286 RVA: 0x0013B24F File Offset: 0x0013944F
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value < 0.5f) ? 0 : 7200000);
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x0013B26B File Offset: 0x0013946B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0013B285 File Offset: 0x00139485
		public override float TemperatureOffset()
		{
			return Mathf.Sin((GenDate.YearsPassedFloat + (float)this.ticksOffset / 3600000f) / 4f * 3.14159274f * 2f) * 20f;
		}

		// Token: 0x04002336 RID: 9014
		private int ticksOffset;

		// Token: 0x04002337 RID: 9015
		private const float PeriodYears = 4f;

		// Token: 0x04002338 RID: 9016
		private const float MaxTempOffset = 20f;
	}
}
