using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6D RID: 3181
	public class Building_Heater : Building_TempControl
	{
		// Token: 0x06004C30 RID: 19504 RVA: 0x00199698 File Offset: 0x00197898
		public override void TickRare()
		{
			if (this.compPowerTrader.PowerOn)
			{
				float ambientTemperature = base.AmbientTemperature;
				float num;
				if (ambientTemperature < 20f)
				{
					num = 1f;
				}
				else if (ambientTemperature > 120f)
				{
					num = 0f;
				}
				else
				{
					num = Mathf.InverseLerp(120f, 20f, ambientTemperature);
				}
				float energyLimit = this.compTempControl.Props.energyPerSecond * num * 4.16666651f;
				float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, this.compTempControl.targetTemperature);
				bool flag = !Mathf.Approximately(num2, 0f);
				CompProperties_Power props = this.compPowerTrader.Props;
				if (flag)
				{
					this.GetRoomGroup().Temperature += num2;
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption;
				}
				else
				{
					this.compPowerTrader.PowerOutput = -props.basePowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
				}
				this.compTempControl.operatingAtHighPower = flag;
			}
		}

		// Token: 0x04002AEB RID: 10987
		private const float EfficiencyFalloffSpan = 100f;
	}
}
