using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class ShaderTypeDefOf
	{
		
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDef));
		}

		
		public static ShaderTypeDef Cutout;

		
		public static ShaderTypeDef CutoutComplex;

		
		public static ShaderTypeDef Transparent;

		
		public static ShaderTypeDef MetaOverlay;

		
		public static ShaderTypeDef EdgeDetect;
	}
}
