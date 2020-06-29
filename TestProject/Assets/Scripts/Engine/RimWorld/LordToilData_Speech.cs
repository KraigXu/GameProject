using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class LordToilData_Speech : LordToilData
	{
		
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		
		public CellRect spectateRect;

		
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		
		public SpectateRectSide spectateRectPreferredSide;
	}
}
