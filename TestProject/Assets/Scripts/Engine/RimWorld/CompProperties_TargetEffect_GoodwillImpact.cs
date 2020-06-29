using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_TargetEffect_GoodwillImpact : CompProperties
	{
		
		public CompProperties_TargetEffect_GoodwillImpact()
		{
			this.compClass = typeof(CompTargetEffect_GoodwillImpact);
		}

		
		public int goodwillImpact = -200;
	}
}
