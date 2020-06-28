using System;

namespace Verse
{
	// Token: 0x020000FE RID: 254
	[Flags]
	public enum WorkTags
	{
		// Token: 0x0400066B RID: 1643
		None = 0,
		// Token: 0x0400066C RID: 1644
		ManualDumb = 2,
		// Token: 0x0400066D RID: 1645
		ManualSkilled = 4,
		// Token: 0x0400066E RID: 1646
		Violent = 8,
		// Token: 0x0400066F RID: 1647
		Caring = 16,
		// Token: 0x04000670 RID: 1648
		Social = 32,
		// Token: 0x04000671 RID: 1649
		Commoner = 64,
		// Token: 0x04000672 RID: 1650
		Intellectual = 128,
		// Token: 0x04000673 RID: 1651
		Animals = 256,
		// Token: 0x04000674 RID: 1652
		Artistic = 512,
		// Token: 0x04000675 RID: 1653
		Crafting = 1024,
		// Token: 0x04000676 RID: 1654
		Cooking = 2048,
		// Token: 0x04000677 RID: 1655
		Firefighting = 4096,
		// Token: 0x04000678 RID: 1656
		Cleaning = 8192,
		// Token: 0x04000679 RID: 1657
		Hauling = 16384,
		// Token: 0x0400067A RID: 1658
		PlantWork = 32768,
		// Token: 0x0400067B RID: 1659
		Mining = 65536,
		// Token: 0x0400067C RID: 1660
		Hunting = 131072,
		// Token: 0x0400067D RID: 1661
		AllWork = 262144
	}
}
