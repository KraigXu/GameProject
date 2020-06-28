using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA6 RID: 3238
	public class IntermittentSteamSprayer
	{
		// Token: 0x06004E4D RID: 20045 RVA: 0x001A5150 File Offset: 0x001A3350
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x001A516C File Offset: 0x001A336C
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < 0.6f)
				{
					MoteMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
				}
				if (Find.TickManager.TicksGame % 20 == 0)
				{
					GenTemperature.PushHeat(this.parent, 40f);
				}
				if (this.sprayTicksLeft <= 0)
				{
					if (this.endSprayCallback != null)
					{
						this.endSprayCallback();
					}
					this.ticksUntilSpray = Rand.RangeInclusive(500, 2000);
					return;
				}
			}
			else
			{
				this.ticksUntilSpray--;
				if (this.ticksUntilSpray <= 0)
				{
					if (this.startSprayCallback != null)
					{
						this.startSprayCallback();
					}
					this.sprayTicksLeft = Rand.RangeInclusive(200, 500);
				}
			}
		}

		// Token: 0x04002BFA RID: 11258
		private Thing parent;

		// Token: 0x04002BFB RID: 11259
		private int ticksUntilSpray = 500;

		// Token: 0x04002BFC RID: 11260
		private int sprayTicksLeft;

		// Token: 0x04002BFD RID: 11261
		public Action startSprayCallback;

		// Token: 0x04002BFE RID: 11262
		public Action endSprayCallback;

		// Token: 0x04002BFF RID: 11263
		private const int MinTicksBetweenSprays = 500;

		// Token: 0x04002C00 RID: 11264
		private const int MaxTicksBetweenSprays = 2000;

		// Token: 0x04002C01 RID: 11265
		private const int MinSprayDuration = 200;

		// Token: 0x04002C02 RID: 11266
		private const int MaxSprayDuration = 500;

		// Token: 0x04002C03 RID: 11267
		private const float SprayThickness = 0.6f;
	}
}
