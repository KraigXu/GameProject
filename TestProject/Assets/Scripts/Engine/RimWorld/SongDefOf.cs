using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class SongDefOf
	{
		
		static SongDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SongDefOf));
		}

		
		public static SongDef EntrySong;

		
		public static SongDef EndCreditsSong;
	}
}
