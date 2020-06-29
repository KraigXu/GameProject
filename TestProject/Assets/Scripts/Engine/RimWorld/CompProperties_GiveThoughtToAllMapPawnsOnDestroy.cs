using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_GiveThoughtToAllMapPawnsOnDestroy : CompProperties
	{
		
		public CompProperties_GiveThoughtToAllMapPawnsOnDestroy()
		{
			this.compClass = typeof(CompGiveThoughtToAllMapPawnsOnDestroy);
		}

		
		public ThoughtDef thought;

		
		[MustTranslate]
		public string message;
	}
}
