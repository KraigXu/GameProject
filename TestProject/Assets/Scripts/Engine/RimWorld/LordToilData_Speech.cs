using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public class LordToilData_Speech : LordToilData
	{
		// Token: 0x0600330A RID: 13066 RVA: 0x0011B74C File Offset: 0x0011994C
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		// Token: 0x04001B81 RID: 7041
		public CellRect spectateRect;

		// Token: 0x04001B82 RID: 7042
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04001B83 RID: 7043
		public SpectateRectSide spectateRectPreferredSide;
	}
}
