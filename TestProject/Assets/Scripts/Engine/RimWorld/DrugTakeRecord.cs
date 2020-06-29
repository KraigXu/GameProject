using System;
using Verse;

namespace RimWorld
{
	
	public class DrugTakeRecord : IExposable
	{
		
		// (get) Token: 0x060044C9 RID: 17609 RVA: 0x00173832 File Offset: 0x00171A32
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		
		public ThingDef drug;

		
		public int lastTakenTicks;

		
		private int timesTakenThisDayInt;

		
		private int thisDay;
	}
}
