using System;

namespace Verse
{
	// Token: 0x020001E6 RID: 486
	public class WeatherEventMaker
	{
		// Token: 0x06000DB5 RID: 3509 RVA: 0x0004E560 File Offset: 0x0004C760
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x04000A7C RID: 2684
		public float averageInterval = 100f;

		// Token: 0x04000A7D RID: 2685
		public Type eventClass;
	}
}
