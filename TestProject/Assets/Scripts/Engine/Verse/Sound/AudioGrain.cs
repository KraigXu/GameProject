using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020004CC RID: 1228
	public abstract class AudioGrain
	{
		// Token: 0x06002429 RID: 9257
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();
	}
}
