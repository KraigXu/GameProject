using System;

namespace Verse
{
	
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		
		public DamageDef damageDefIncoming;

		
		public BodyPartDef createHediffOn;

		
		public HediffDef createHediff;

		
		public bool vomit;
	}
}
