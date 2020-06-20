using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001E5 RID: 485
	public class WeatherEventHandler
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0004E4A1 File Offset: 0x0004C6A1
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0004E4A9 File Offset: 0x0004C6A9
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0004E4C0 File Offset: 0x0004C6C0
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0004E518 File Offset: 0x0004C718
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04000A7B RID: 2683
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
