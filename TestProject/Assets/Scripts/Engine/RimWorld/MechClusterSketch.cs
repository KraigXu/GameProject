using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA1 RID: 2721
	public class MechClusterSketch : IExposable
	{
		// Token: 0x06004012 RID: 16402 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public MechClusterSketch()
		{
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00155D4D File Offset: 0x00153F4D
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x00155D6A File Offset: 0x00153F6A
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		// Token: 0x04002546 RID: 9542
		public Sketch buildingsSketch;

		// Token: 0x04002547 RID: 9543
		public List<MechClusterSketch.Mech> pawns;

		// Token: 0x04002548 RID: 9544
		public bool startDormant;

		// Token: 0x02001A7C RID: 6780
		public struct Mech : IExposable
		{
			// Token: 0x0600978B RID: 38795 RVA: 0x002EB258 File Offset: 0x002E9458
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			// Token: 0x0600978C RID: 38796 RVA: 0x002EB26C File Offset: 0x002E946C
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			// Token: 0x04006490 RID: 25744
			public PawnKindDef kindDef;

			// Token: 0x04006491 RID: 25745
			public IntVec3 position;
		}
	}
}
