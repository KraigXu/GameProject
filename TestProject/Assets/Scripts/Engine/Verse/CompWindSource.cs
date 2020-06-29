﻿using System;

namespace Verse
{
	
	public class CompWindSource : ThingComp
	{
		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		
		public float wind;
	}
}
