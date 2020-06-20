using System;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x0400090C RID: 2316
		None = 0,
		// Token: 0x0400090D RID: 2317
		Verts = 1,
		// Token: 0x0400090E RID: 2318
		Tris = 2,
		// Token: 0x0400090F RID: 2319
		Colors = 4,
		// Token: 0x04000910 RID: 2320
		UVs = 8,
		// Token: 0x04000911 RID: 2321
		All = 127
	}
}
