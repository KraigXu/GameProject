using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Usable : CompProperties
	{
		
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		
		public JobDef useJob;

		
		[MustTranslate]
		public string useLabel;

		
		public int useDuration = 100;
	}
}
