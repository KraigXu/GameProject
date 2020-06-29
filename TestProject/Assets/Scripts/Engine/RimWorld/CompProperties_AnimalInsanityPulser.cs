﻿using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_AnimalInsanityPulser : CompProperties
	{
		
		public CompProperties_AnimalInsanityPulser()
		{
			this.compClass = typeof(CompAnimalInsanityPulser);
		}

		
		public IntRange pulseInterval = new IntRange(60000, 150000);

		
		public int radius = 25;
	}
}
