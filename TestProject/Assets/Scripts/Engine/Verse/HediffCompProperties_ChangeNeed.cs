using System;
using RimWorld;

namespace Verse
{
	
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		
		public NeedDef needDef;

		
		public float percentPerDay;
	}
}
