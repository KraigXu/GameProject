using System;

namespace RimWorld
{
	// Token: 0x02000C55 RID: 3157
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x04002A91 RID: 10897
		NeedsPower = 1,
		// Token: 0x04002A92 RID: 10898
		PowerOff = 2,
		// Token: 0x04002A93 RID: 10899
		BurningWick = 4,
		// Token: 0x04002A94 RID: 10900
		Forbidden = 8,
		// Token: 0x04002A95 RID: 10901
		ForbiddenBig = 16,
		// Token: 0x04002A96 RID: 10902
		QuestionMark = 32,
		// Token: 0x04002A97 RID: 10903
		BrokenDown = 64,
		// Token: 0x04002A98 RID: 10904
		OutOfFuel = 128,
		// Token: 0x04002A99 RID: 10905
		ForbiddenRefuel = 256
	}
}
