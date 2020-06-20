using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000578 RID: 1400
	public class PawnPathPool
	{
		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000E87AF File Offset: 0x000E69AF
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000E87B6 File Offset: 0x000E69B6
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000E87E0 File Offset: 0x000E69E0
		public PawnPath GetEmptyPawnPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > this.map.mapPawns.AllPawnsSpawnedCount + 2)
			{
				Log.ErrorOnce("PawnPathPool leak: more paths than spawned pawns. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			PawnPath pawnPath = new PawnPath();
			this.paths.Add(pawnPath);
			pawnPath.inUse = true;
			return pawnPath;
		}

		// Token: 0x040017AE RID: 6062
		private Map map;

		// Token: 0x040017AF RID: 6063
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x040017B0 RID: 6064
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
