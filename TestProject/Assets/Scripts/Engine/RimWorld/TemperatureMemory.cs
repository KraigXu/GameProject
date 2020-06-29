using System;
using Verse;

namespace RimWorld
{
	
	public class TemperatureMemory : IExposable
	{
		
		// (get) Token: 0x060040B9 RID: 16569 RVA: 0x0015AD66 File Offset: 0x00158F66
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x0015AD7A File Offset: 0x00158F7A
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}

		
		private Map map;

		
		private int growthSeasonUntilTick = -1;

		
		private int noSowUntilTick = -1;

		
		private const int TicksBuffer = 30000;
	}
}
