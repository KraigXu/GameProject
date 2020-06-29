using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		
		public static ImplementOwnerTypeDef Weapon;

		
		public static ImplementOwnerTypeDef Bodypart;

		
		public static ImplementOwnerTypeDef Hediff;

		
		public static ImplementOwnerTypeDef Terrain;

		
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
