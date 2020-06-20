using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F65 RID: 3941
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x0600606C RID: 24684 RVA: 0x00216E8C File Offset: 0x0021508C
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x04003751 RID: 14161
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04003752 RID: 14162
		public static RecipeDef CookMealSimple;

		// Token: 0x04003753 RID: 14163
		public static RecipeDef InstallPegLeg;
	}
}
