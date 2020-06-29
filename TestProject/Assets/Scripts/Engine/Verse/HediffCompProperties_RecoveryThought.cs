using System;
using RimWorld;

namespace Verse
{
	
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		
		public ThoughtDef thought;
	}
}
