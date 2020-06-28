using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C8 RID: 2504
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003BC5 RID: 15301
		int Tile { get; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003BC6 RID: 15302
		StoryState StoryState { get; }

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003BC7 RID: 15303
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003BC8 RID: 15304
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003BC9 RID: 15305
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003BCA RID: 15306
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003BCB RID: 15307
		int ConstantRandSeed { get; }

		// Token: 0x06003BCC RID: 15308
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
