using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F88 RID: 3976
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x0600608F RID: 24719 RVA: 0x002170DF File Offset: 0x002152DF
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x04003A13 RID: 14867
		public static WeatherDef Clear;
	}
}
