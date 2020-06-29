using System;

namespace RimWorld
{
	
	[DefOf]
	public static class BodyTypeDefOf
	{
		
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDef));
		}

		
		public static BodyTypeDef Male;

		
		public static BodyTypeDef Female;

		
		public static BodyTypeDef Thin;

		
		public static BodyTypeDef Hulk;

		
		public static BodyTypeDef Fat;
	}
}
