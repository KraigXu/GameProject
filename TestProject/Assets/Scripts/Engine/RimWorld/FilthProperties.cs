using System;
using Verse;

namespace RimWorld
{
	
	public class FilthProperties
	{
		
		
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		
		public float cleaningWorkToReduceThickness = 35f;

		
		public bool canFilthAttach;

		
		public bool rainWashes;

		
		public bool allowsFire = true;

		
		public int maxThickness = 100;

		
		public FloatRange disappearsInDays = FloatRange.Zero;

		
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		
		public SoundDef cleaningSound;
	}
}
