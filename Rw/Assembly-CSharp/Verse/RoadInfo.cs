using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000183 RID: 387
	public class RoadInfo : MapComponent
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x0003C2F0 File Offset: 0x0003A4F0
		public RoadInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0003C304 File Offset: 0x0003A504
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.roadEdgeTiles, "roadEdgeTiles", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000903 RID: 2307
		public List<IntVec3> roadEdgeTiles = new List<IntVec3>();
	}
}
