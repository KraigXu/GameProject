using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A9 RID: 1961
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x060032FE RID: 13054 RVA: 0x0011B3FC File Offset: 0x001195FC
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}

		// Token: 0x04001B7B RID: 7035
		public CellRect spectateRect;

		// Token: 0x04001B7C RID: 7036
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;
	}
}
