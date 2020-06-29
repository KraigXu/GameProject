using System;

namespace RimWorld
{
	
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		
		public static PawnsArrivalModeDef EdgeWalkIn;

		
		public static PawnsArrivalModeDef CenterDrop;

		
		public static PawnsArrivalModeDef EdgeDrop;

		
		public static PawnsArrivalModeDef RandomDrop;
	}
}
