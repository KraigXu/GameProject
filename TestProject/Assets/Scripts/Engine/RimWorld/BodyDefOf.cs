using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class BodyDefOf
	{
		
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		
		public static BodyDef Human;

		
		public static BodyDef MechanicalCentipede;
	}
}
