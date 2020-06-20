using System;

namespace Verse
{
	// Token: 0x02000188 RID: 392
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x0400091B RID: 2331
		None = 0,
		// Token: 0x0400091C RID: 2332
		Things = 1,
		// Token: 0x0400091D RID: 2333
		FogOfWar = 2,
		// Token: 0x0400091E RID: 2334
		Buildings = 4,
		// Token: 0x0400091F RID: 2335
		GroundGlow = 8,
		// Token: 0x04000920 RID: 2336
		Terrain = 16,
		// Token: 0x04000921 RID: 2337
		Roofs = 32,
		// Token: 0x04000922 RID: 2338
		Snow = 64,
		// Token: 0x04000923 RID: 2339
		Zone = 128,
		// Token: 0x04000924 RID: 2340
		PowerGrid = 256,
		// Token: 0x04000925 RID: 2341
		BuildingsDamage = 512
	}
}
