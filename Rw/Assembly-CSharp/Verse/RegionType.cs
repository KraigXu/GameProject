using System;

namespace Verse
{
	// Token: 0x020001C7 RID: 455
	[Flags]
	public enum RegionType
	{
		// Token: 0x040009F6 RID: 2550
		None = 0,
		// Token: 0x040009F7 RID: 2551
		ImpassableFreeAirExchange = 1,
		// Token: 0x040009F8 RID: 2552
		Normal = 2,
		// Token: 0x040009F9 RID: 2553
		Portal = 4,
		// Token: 0x040009FA RID: 2554
		Set_Passable = 6,
		// Token: 0x040009FB RID: 2555
		Set_Impassable = 1,
		// Token: 0x040009FC RID: 2556
		Set_All = 7
	}
}
