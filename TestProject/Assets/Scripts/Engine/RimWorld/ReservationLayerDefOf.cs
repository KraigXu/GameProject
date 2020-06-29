using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ReservationLayerDefOf
	{
		
		static ReservationLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ReservationLayerDefOf));
		}

		
		public static ReservationLayerDef Floor;

		
		public static ReservationLayerDef Ceiling;
	}
}
