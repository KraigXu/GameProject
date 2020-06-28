using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8A RID: 3978
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x06006091 RID: 24721 RVA: 0x00217101 File Offset: 0x00215301
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x04003A20 RID: 14880
		public static MapGeneratorDef Encounter;

		// Token: 0x04003A21 RID: 14881
		public static MapGeneratorDef Base_Player;

		// Token: 0x04003A22 RID: 14882
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04003A23 RID: 14883
		public static MapGeneratorDef EscapeShip;
	}
}
