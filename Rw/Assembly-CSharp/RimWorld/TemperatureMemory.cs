using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAE RID: 2734
	public class TemperatureMemory : IExposable
	{
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060040B9 RID: 16569 RVA: 0x0015AD66 File Offset: 0x00158F66
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x0015AD7A File Offset: 0x00158F7A
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0015AD9F File Offset: 0x00158F9F
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0015ADBC File Offset: 0x00158FBC
		public void GrowthSeasonMemoryTick()
		{
			if (this.map.mapTemperature.OutdoorTemp > 0f && this.map.mapTemperature.OutdoorTemp < 58f)
			{
				this.growthSeasonUntilTick = Find.TickManager.TicksGame + 30000;
				return;
			}
			if (this.map.mapTemperature.OutdoorTemp < -2f)
			{
				this.growthSeasonUntilTick = -1;
				this.noSowUntilTick = Find.TickManager.TicksGame + 30000;
			}
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0015AE42 File Offset: 0x00159042
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}

		// Token: 0x04002595 RID: 9621
		private Map map;

		// Token: 0x04002596 RID: 9622
		private int growthSeasonUntilTick = -1;

		// Token: 0x04002597 RID: 9623
		private int noSowUntilTick = -1;

		// Token: 0x04002598 RID: 9624
		private const int TicksBuffer = 30000;
	}
}
