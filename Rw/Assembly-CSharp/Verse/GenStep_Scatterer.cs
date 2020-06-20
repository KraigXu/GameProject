using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A5 RID: 421
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x06000BCD RID: 3021 RVA: 0x00042FA4 File Offset: 0x000411A4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!this.allowInWaterBiome && map.TileInfo.WaterCovered)
			{
				return;
			}
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00043008 File Offset: 0x00041208
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearWith(map.Center, (IntVec3 x) => this.CanScatterAt(x, map), map, out result, 3, 2147483647))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.", false);
			}
			return false;
		}

		// Token: 0x06000BCF RID: 3023
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000430D0 File Offset: 0x000412D0
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				return false;
			}
			if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				return false;
			}
			if (this.NearUsedSpot(loc, this.minSpacing))
			{
				return false;
			}
			if ((map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
			{
				return false;
			}
			if (this.spotMustBeStandable && !loc.Standable(map))
			{
				return false;
			}
			if (!this.allowFoggedPositions && loc.Fogged(map))
			{
				return false;
			}
			if (this.validators != null)
			{
				for (int i = 0; i < this.validators.Count; i++)
				{
					if (!this.validators[i].Allows(loc, map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000431A8 File Offset: 0x000413A8
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000431EE File Offset: 0x000413EE
		protected int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
			}
			return this.count;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00043214 File Offset: 0x00041414
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0004324C File Offset: 0x0004144C
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x04000966 RID: 2406
		public int count = -1;

		// Token: 0x04000967 RID: 2407
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04000968 RID: 2408
		public bool nearPlayerStart;

		// Token: 0x04000969 RID: 2409
		public bool nearMapCenter;

		// Token: 0x0400096A RID: 2410
		public float minSpacing = 10f;

		// Token: 0x0400096B RID: 2411
		public bool spotMustBeStandable;

		// Token: 0x0400096C RID: 2412
		public int minDistToPlayerStart;

		// Token: 0x0400096D RID: 2413
		public int minEdgeDist;

		// Token: 0x0400096E RID: 2414
		public int extraNoBuildEdgeDist;

		// Token: 0x0400096F RID: 2415
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04000970 RID: 2416
		public bool allowInWaterBiome = true;

		// Token: 0x04000971 RID: 2417
		public bool allowFoggedPositions = true;

		// Token: 0x04000972 RID: 2418
		public bool warnOnFail = true;

		// Token: 0x04000973 RID: 2419
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04000974 RID: 2420
		private const int ScatterNearPlayerRadius = 20;
	}
}
