using System;

namespace Verse
{
	// Token: 0x02000058 RID: 88
	public enum TraverseMode : byte
	{
		// Token: 0x0400011B RID: 283
		ByPawn,
		// Token: 0x0400011C RID: 284
		PassDoors,
		// Token: 0x0400011D RID: 285
		NoPassClosedDoors,
		// Token: 0x0400011E RID: 286
		PassAllDestroyableThings,
		// Token: 0x0400011F RID: 287
		NoPassClosedDoorsOrWater,
		// Token: 0x04000120 RID: 288
		PassAllDestroyableThingsNotWater
	}
}
