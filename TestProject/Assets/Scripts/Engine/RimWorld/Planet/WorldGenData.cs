using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200120E RID: 4622
	public class WorldGenData : WorldComponent
	{
		// Token: 0x06006AEB RID: 27371 RVA: 0x00254DDF File Offset: 0x00252FDF
		public WorldGenData(World world) : base(world)
		{
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x00254DFE File Offset: 0x00252FFE
		public override void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.roadNodes, "roadNodes", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x040042D1 RID: 17105
		public List<int> roadNodes = new List<int>();

		// Token: 0x040042D2 RID: 17106
		public List<int> ancientSites = new List<int>();
	}
}
