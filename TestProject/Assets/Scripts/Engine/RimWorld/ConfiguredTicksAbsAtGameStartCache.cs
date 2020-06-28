using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE6 RID: 3046
	public class ConfiguredTicksAbsAtGameStartCache
	{
		// Token: 0x0600482D RID: 18477 RVA: 0x00186DEF File Offset: 0x00184FEF
		public bool TryGetCachedValue(GameInitData initData, out int ticksAbs)
		{
			if (initData.startingTile == this.cachedForStartingTile && initData.startingSeason == this.cachedForStartingSeason)
			{
				ticksAbs = this.cachedTicks;
				return true;
			}
			ticksAbs = -1;
			return false;
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x00186E1B File Offset: 0x0018501B
		public void Cache(int ticksAbs, GameInitData initData)
		{
			this.cachedTicks = ticksAbs;
			this.cachedForStartingTile = initData.startingTile;
			this.cachedForStartingSeason = initData.startingSeason;
		}

		// Token: 0x04002948 RID: 10568
		private int cachedTicks = -1;

		// Token: 0x04002949 RID: 10569
		private int cachedForStartingTile = -1;

		// Token: 0x0400294A RID: 10570
		private Season cachedForStartingSeason;
	}
}
