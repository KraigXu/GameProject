using System;
using System.Collections.Generic;

namespace Verse
{
	
	public class WeatherEventHandler
	{
		
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0004E4A1 File Offset: 0x0004C6A1
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		
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

		
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
