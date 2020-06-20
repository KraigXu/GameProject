using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AB8 RID: 2744
	public static class WeatherPartPool
	{
		// Token: 0x060040F7 RID: 16631 RVA: 0x0015BEE4 File Offset: 0x0015A0E4
		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T t = WeatherPartPool.instances[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			SkyOverlay skyOverlay = Activator.CreateInstance<T>();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}

		// Token: 0x040025B7 RID: 9655
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
