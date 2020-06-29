using System;
using Verse;

namespace RimWorld
{
	
	[DefOf]
	public static class LogEntryDefOf
	{
		
		static LogEntryDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LogEntryDef));
		}

		
		public static LogEntryDef MeleeAttack;
	}
}
