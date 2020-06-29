using System;
using System.Collections.Generic;

namespace Verse.AI
{
	
	public class PawnPathPool
	{
		
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000E87AF File Offset: 0x000E69AF
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		
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

		
		private Map map;

		
		private List<PawnPath> paths = new List<PawnPath>(64);

		
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
