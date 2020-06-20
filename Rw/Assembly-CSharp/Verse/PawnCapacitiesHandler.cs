using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200028D RID: 653
	public class PawnCapacitiesHandler
	{
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x00062BCA File Offset: 0x00060DCA
		public bool CanBeAwake
		{
			get
			{
				return this.GetLevel(PawnCapacityDefOf.Consciousness) >= 0.3f;
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00062BE1 File Offset: 0x00060DE1
		public PawnCapacitiesHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00062BF0 File Offset: 0x00060DF0
		public void Clear()
		{
			this.cachedCapacityLevels = null;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00062BFC File Offset: 0x00060DFC
		public float GetLevel(PawnCapacityDef capacity)
		{
			if (this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.cachedCapacityLevels == null)
			{
				this.Notify_CapacityLevelsDirty();
			}
			PawnCapacitiesHandler.CacheElement cacheElement = this.cachedCapacityLevels[capacity];
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Caching)
			{
				Log.Error(string.Format("Detected infinite stat recursion when evaluating {0}", capacity), false);
				return 0f;
			}
			if (cacheElement.status == PawnCapacitiesHandler.CacheStatus.Uncached)
			{
				cacheElement.status = PawnCapacitiesHandler.CacheStatus.Caching;
				try
				{
					cacheElement.value = PawnCapacityUtility.CalculateCapacityLevel(this.pawn.health.hediffSet, capacity, null, false);
				}
				finally
				{
					cacheElement.status = PawnCapacitiesHandler.CacheStatus.Cached;
				}
			}
			return cacheElement.value;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00062CAC File Offset: 0x00060EAC
		public bool CapableOf(PawnCapacityDef capacity)
		{
			return this.GetLevel(capacity) > capacity.minForCapable;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00062CC0 File Offset: 0x00060EC0
		public void Notify_CapacityLevelsDirty()
		{
			if (this.cachedCapacityLevels == null)
			{
				this.cachedCapacityLevels = new DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement>();
			}
			for (int i = 0; i < this.cachedCapacityLevels.Count; i++)
			{
				this.cachedCapacityLevels[i].status = PawnCapacitiesHandler.CacheStatus.Uncached;
			}
		}

		// Token: 0x04000C76 RID: 3190
		private Pawn pawn;

		// Token: 0x04000C77 RID: 3191
		private DefMap<PawnCapacityDef, PawnCapacitiesHandler.CacheElement> cachedCapacityLevels;

		// Token: 0x02001452 RID: 5202
		private enum CacheStatus
		{
			// Token: 0x04004D29 RID: 19753
			Uncached,
			// Token: 0x04004D2A RID: 19754
			Caching,
			// Token: 0x04004D2B RID: 19755
			Cached
		}

		// Token: 0x02001453 RID: 5203
		private class CacheElement
		{
			// Token: 0x04004D2C RID: 19756
			public PawnCapacitiesHandler.CacheStatus status;

			// Token: 0x04004D2D RID: 19757
			public float value;
		}
	}
}
