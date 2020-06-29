using System;
using Verse;

namespace RimWorld
{
	
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		
		public ThingDef mote;

		
		public int moteCount = 3;

		
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		
		public ThingDef filth;

		
		public int filthCount = 4;

		
		public HediffDef injuryCreatedOnDeath;

		
		public IntRange injuryCount;

		
		public SoundDef sound;
	}
}
