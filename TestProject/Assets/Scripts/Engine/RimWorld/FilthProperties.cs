using System;
using Verse;

namespace RimWorld
{
	
	public class FilthProperties
	{
		
		// (get) Token: 0x0600354E RID: 13646 RVA: 0x001232D5 File Offset: 0x001214D5
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
