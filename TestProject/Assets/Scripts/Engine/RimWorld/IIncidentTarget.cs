using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public interface IIncidentTarget : ILoadReferenceable
	{
		
		// (get) Token: 0x06003BC5 RID: 15301
		int Tile { get; }

		
		// (get) Token: 0x06003BC6 RID: 15302
		StoryState StoryState { get; }

		
		// (get) Token: 0x06003BC7 RID: 15303
		GameConditionManager GameConditionManager { get; }

		
		// (get) Token: 0x06003BC8 RID: 15304
		float PlayerWealthForStoryteller { get; }

		
		// (get) Token: 0x06003BC9 RID: 15305
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		
		// (get) Token: 0x06003BCA RID: 15306
		FloatRange IncidentPointsRandomFactorRange { get; }

		
		// (get) Token: 0x06003BCB RID: 15307
		int ConstantRandSeed { get; }

		
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
