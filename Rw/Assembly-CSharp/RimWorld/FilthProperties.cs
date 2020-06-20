using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	public class FilthProperties
	{
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600354E RID: 13646 RVA: 0x001232D5 File Offset: 0x001214D5
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		// Token: 0x04001CBB RID: 7355
		public float cleaningWorkToReduceThickness = 35f;

		// Token: 0x04001CBC RID: 7356
		public bool canFilthAttach;

		// Token: 0x04001CBD RID: 7357
		public bool rainWashes;

		// Token: 0x04001CBE RID: 7358
		public bool allowsFire = true;

		// Token: 0x04001CBF RID: 7359
		public int maxThickness = 100;

		// Token: 0x04001CC0 RID: 7360
		public FloatRange disappearsInDays = FloatRange.Zero;

		// Token: 0x04001CC1 RID: 7361
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		// Token: 0x04001CC2 RID: 7362
		public SoundDef cleaningSound;
	}
}
