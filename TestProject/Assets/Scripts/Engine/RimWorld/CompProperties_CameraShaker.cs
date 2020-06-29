using System;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_CameraShaker : CompProperties
	{
		
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		
		public float mag = 0.05f;
	}
}
