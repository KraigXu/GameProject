using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7A RID: 2938
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x060044C9 RID: 17609 RVA: 0x00173832 File Offset: 0x00171A32
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x060044CA RID: 17610 RVA: 0x0017383F File Offset: 0x00171A3F
		// (set) Token: 0x060044CB RID: 17611 RVA: 0x00173856 File Offset: 0x00171A56
		public int TimesTakenThisDay
		{
			get
			{
				if (this.thisDay != GenDate.DaysPassed)
				{
					return 0;
				}
				return this.timesTakenThisDayInt;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x0017386C File Offset: 0x00171A6C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x0400274C RID: 10060
		public ThingDef drug;

		// Token: 0x0400274D RID: 10061
		public int lastTakenTicks;

		// Token: 0x0400274E RID: 10062
		private int timesTakenThisDayInt;

		// Token: 0x0400274F RID: 10063
		private int thisDay;
	}
}
