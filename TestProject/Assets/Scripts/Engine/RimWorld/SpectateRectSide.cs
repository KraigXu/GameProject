using System;

namespace RimWorld
{
	// Token: 0x02000F3C RID: 3900
	[Flags]
	public enum SpectateRectSide
	{
		// Token: 0x040033E9 RID: 13289
		None = 0,
		// Token: 0x040033EA RID: 13290
		Up = 1,
		// Token: 0x040033EB RID: 13291
		Right = 2,
		// Token: 0x040033EC RID: 13292
		Down = 4,
		// Token: 0x040033ED RID: 13293
		Left = 8,
		// Token: 0x040033EE RID: 13294
		Vertical = 5,
		// Token: 0x040033EF RID: 13295
		Horizontal = 10,
		// Token: 0x040033F0 RID: 13296
		All = 15
	}
}
