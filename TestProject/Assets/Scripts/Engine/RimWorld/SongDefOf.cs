using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7C RID: 3964
	[DefOf]
	public static class SongDefOf
	{
		// Token: 0x06006083 RID: 24707 RVA: 0x00217013 File Offset: 0x00215213
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		// Token: 0x04003905 RID: 14597
		public static SongDef EntrySong;

		// Token: 0x04003906 RID: 14598
		public static SongDef EndCreditsSong;
	}
}
