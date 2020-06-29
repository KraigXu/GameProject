using System;
using Verse;

namespace RimWorld
{
	
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		
		public bool destroyGear;

		
		public bool destroyBody;

		
		public float explosionRadius;

		
		public DamageDef damageDef;

		
		public int damageAmount = -1;
	}
}
