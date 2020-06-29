using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class WeatherDefOf
	{
		
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		
		public static WeatherDef Clear;
	}
}
