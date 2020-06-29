using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ApparelLayerDefOf
	{
		
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		
		public static ApparelLayerDef OnSkin;

		
		public static ApparelLayerDef Shell;

		
		public static ApparelLayerDef Middle;

		
		public static ApparelLayerDef Belt;

		
		public static ApparelLayerDef Overhead;
	}
}
