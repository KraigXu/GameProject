using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Schedule : CompProperties
	{
		
		public CompProperties_Schedule()
		{
			this.compClass = typeof(CompSchedule);
		}

		
		public float startTime;

		
		public float endTime = 1f;

		
		[MustTranslate]
		public string offMessage;
	}
}
